using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RubricaTelefonicaAziendale.Entities;
using RubricaTelefonicaAziendale.Models;
using RubricaTelefonicaAziendale.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
// builder.Services.AddScoped<IMailSettingsService, MailSettingsService>();
// builder.Services.AddScoped<IMailTemplateService, MailTemplateService>();
// builder.Services.AddScoped<IPersoneService, PersoneService>();
// builder.Services.AddScoped<IPersoneIndirizziService, PersoneIndirizziService>();
// builder.Services.AddScoped<IPersoneRecapitiService, PersoneRecapitiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
//}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();