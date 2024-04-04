using System.Data;
using System.Data.Common;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RubricaTelefonicaAziendale.Entities;

namespace RubricaTelefonicaAziendale.Models
{
    public class StoredProceduresHandler
    {
        protected TjfChallengeContext DbContext { get; }

        public StoredProceduresHandler(TjfChallengeContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// Execute procedure from database using it's name and params that is protected from the SQL injection attacks.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp_name">Name of the procedure that should be executed.</param>
        /// <param name="sp_params">Dictionary of params that procedure takes. </param>
        /// <returns>List of objects that are mapped in T type, returned by procedure.</returns>
        public async Task<List<T>> ExecuteStoredProcedure<T>(String sp_name, List<StoredProcedureParams> paramsList) where T : class
        {
            DbConnection conn = DbContext.Database.GetDbConnection();
            try
            {
                if (conn.State != ConnectionState.Open)
                    await conn.OpenAsync();
                await using DbCommand command = conn.CreateCommand();
                command.CommandText = sp_name;
                command.CommandType = CommandType.StoredProcedure;
                foreach (StoredProcedureParams spp in paramsList)
                {
                    DbParameter param = command.CreateParameter();
                    param.ParameterName = spp.ParamName;
                    param.Value = spp.ParamValue ?? DBNull.Value;
                    param.DbType = spp.ParamType;
                    command.Parameters.Add(param);
                }
                DbDataReader reader = await command.ExecuteReaderAsync();
                List<T> objList = new();
                IEnumerable<PropertyInfo> props = typeof(T).GetRuntimeProperties();
                Dictionary<string, DbColumn> colMapping = reader.GetColumnSchema()
                                                                .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
                                                                .ToDictionary(key => key.ColumnName.ToLower());
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        T obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in props)
                        {
                            object? val = reader.GetValue(colMapping![prop!.Name!.ToLower()].ColumnOrdinal!.Value);
                            prop.SetValue(obj, val == DBNull.Value ? null : val);
                        }
                        objList.Add(obj);
                    }
                }
                reader.Dispose();
                return objList;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message, e.InnerException);
            }
            finally
            {
                conn.Close();
            }
            return new List<T>();
        }
    }

    public class StoredProcedureParams
    {
        public String ParamName { get; set; } = "";
        public Object? ParamValue { get; set; }
        public DbType ParamType { get; set; }
    }
}