using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptingLib.DAL
{
    public class DbAccessProvider
    {
        public string ConnectionString { get; private set; }

        public DbAccessProvider(string conS)
        {
            ConnectionString = conS;
        }

        public int Execute(string query)
        {
            int result = 0;
            using (var connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    result = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {

                }
            }
            return result;
        }

        public dynamic[] Query(string query)
        {
            List<dynamic> returnObjs = new List<dynamic>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var newObj = (IDictionary<String, Object>)new ExpandoObject();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            newObj.Add(reader.GetName(i), reader[i]);
                        }

                        returnObjs.Add((ExpandoObject)newObj);
                    }

                    return returnObjs.ToArray();
                }
                catch (Exception e)
                {
                    return new ExpandoObject[0];
                }
            }
        }

        private T Convert<T>(ExpandoObject source, T type) where T : class
        {
            IDictionary<string, object> dict = source;

            var ctor = type.GetType().GetConstructors().Single();

            var parameters = ctor.GetParameters();

            var parameterValues = parameters.Select(p => dict[p.Name]).ToArray();

            return (T)ctor.Invoke(parameterValues);
        }
    }
}
