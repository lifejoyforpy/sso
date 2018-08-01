using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRMS.Framework.Dapper
{
    public interface IDbConnect
    {
        IDataReader ExecuteReader();
        void Close();
    }

    public class MySqlConnect : IDbConnect
    {
        private readonly string _sql;
        private readonly object _param;

        private MySqlConnection _cnn = null;
        public MySqlConnect(string sql, object param)
        {
            _sql = sql;
            _param = param;
            _cnn = DapperFactory.CreateMySqlConnection();
            if (_cnn.State != ConnectionState.Open)
                _cnn.Open();
        }

        public void Close()
        {
            if (_cnn.State != ConnectionState.Closed)
                _cnn.Close();
        }

        public IDataReader ExecuteReader()
        {
            var reader = _cnn.ExecuteReader(_sql, _param);
            return reader;
        }
    }
}
