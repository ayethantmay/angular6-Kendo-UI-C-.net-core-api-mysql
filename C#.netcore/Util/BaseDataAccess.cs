using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace dotnet2_1WebAPI
{
    public class BaseDataAccess
    {
        public string ConnectionString { get; set; }

        public BaseDataAccess(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public dynamic ExecuteReader(string qry)
        {
            var retObject = new List<dynamic>();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(qry, conn);
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            // var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            var dataRow = new DynamicObject();
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                // one can modify the next line to
                                //   if (dataReader.IsDBNull(iFiled))
                                //       dataRow.Add(dataReader.GetName(iFiled), dataReader[iFiled]);
                                // if one want don't fill the property for NULL
                                dataRow.AddProperty(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                );
                            }

                            retObject.Add(dataRow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Globalfunction.WriteSystemLog(ex.Message);
            }
            return retObject;
        }
    }
}