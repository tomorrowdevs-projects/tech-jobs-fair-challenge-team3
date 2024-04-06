using Fleck;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RubricaTelefonicaAziendale.Entities;
using RubricaTelefonicaAziendale.Handlers;
using RubricaTelefonicaAziendale.Models;
using RubricaTelefonicaAziendale.Services;

// Inizializzo un websocket
var WsServer = new WebSocketServer("ws://0.0.0.0:1402");
WsServer.Start(ws =>
{
    ws.OnOpen = () =>
    {
        WebSocketHandler.WsConnections.Add(ws);
    };
    ws.OnMessage = message =>
    {
        WsMessage? msg = JsonConvert.DeserializeObject<WsMessage>(message);
        if (msg != null) WebSocketHandler.WsMessageIn.Add(msg);
    };
    ws.OnError = error =>
    {
        // salvo nei logs
    };
});

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RubricaTelefonicaAziendale", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"JWT Authorization header using the Bearer scheme.<br><br> 
                        Enter 'Bearer' [space] and then your token in the text input below.<br><br>
                        Example: 'Bearer 12345abcdef'",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

// SMTP settings
var MailSettings = builder.Configuration.GetSection("MailSettings").Get<MailSettings>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// JWT settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(configureOptions =>
{
    configureOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = jwtSettings!.ValidAudience,
        ValidIssuer = jwtSettings.ValidIssuer,
        ValidateAudience = jwtSettings.ValidateAudience,
        ValidateIssuer = jwtSettings.ValidateIssuer,
        ValidateLifetime = jwtSettings.ValidateLifetime,
        ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSettings.IssuerSigningKey)),
        RequireExpirationTime = jwtSettings.RequireExpirationTime,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddDbContext<TjfChallengeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnectionString")));

// Add Project Services
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPeopleService, PeopleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();
//}

app.UseCors(options => options.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());

app.UseHttpsRedirection();

app.MapControllers();

app.Run();