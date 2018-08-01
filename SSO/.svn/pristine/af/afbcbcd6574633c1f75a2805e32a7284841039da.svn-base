using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRMS.Framework.Dapper
{
    public class DapperFactory
    {
        private static readonly string mysqlconnectionString = ConfigurationManager.AppSettings["MySqlConnection"];

        public static MySqlConnection CreateMySqlConnection()
        {
            var connection = new MySqlConnection(mysqlconnectionString);
            connection.Open();
            return connection;
        }
    }
}