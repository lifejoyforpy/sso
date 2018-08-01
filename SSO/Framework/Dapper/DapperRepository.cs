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
    public class DapperRepository
    {
        /// <summary>
        /// list返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param">参数对象</param>
        /// <returns></returns>
        public static List<T> Query<T>(string sql, object param)
        {
            using (MySqlConnection conn = DapperFactory.CreateMySqlConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var result = conn.Query<T>(sql, param).ToList();

                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                return result;
            }
        }

        /// <summary>
        /// execute a Retrieve command with parameter, return a single result
        /// </summary>
        /// <typeparam name="T">Generic Type, to mapping the result</typeparam>
        /// <param name="sql">sql command</param>
        /// <param name="param">parameter</param>
        /// <returns></returns>
        public static T QuerySingle<T>(string sql, object param)
        {
            T t;

            using (MySqlConnection conn = DapperFactory.CreateMySqlConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                t = conn.Query<T>(sql, param).SingleOrDefault();

                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return t;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="currentindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Pagination<T> PageQuery<T>(string sql, object param, int currentindex, int pagesize)
            where T : class, new()
        {
            using (MySqlConnection conn = DapperFactory.CreateMySqlConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                var executeCountsql = $"SELECT COUNT(1) AS CountNum FROM ({sql}) t ";
                var totalcount = conn.Query<int>(executeCountsql, param).SingleOrDefault();

                int start = (currentindex - 1) * pagesize;
                int end = currentindex * pagesize;

                var exectePagesql = $"{sql} limit {start},{pagesize}";

                var models = conn.Query<T>(exectePagesql, param).ToList();

                if (conn.State != ConnectionState.Closed)
                    conn.Close();

                var result = new Pagination<T> { TotalCount = totalcount, Models = models };
                return result;
            }
        }

        /// <summary>
        /// 执行单个sql返回true or false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool Execute<T>(T t, string sql)
        {
            int result = -1;

            using (MySqlConnection conn = DapperFactory.CreateMySqlConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    result = conn.Execute(sql, t, trans, 30, CommandType.Text);
                    if (result <= 0)
                        trans.Rollback();
                    else
                        trans.Commit();
                }

                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return result > 0;
        }

        /// <summary>
        /// 事务下操作多个sql返回true or false
        /// </summary>
        /// <returns></returns>
        public static bool Execute(List<KeyValuePair<string, object>> vals)
        {
            //ar result = -1;

            using (MySqlConnection conn = DapperFactory.CreateMySqlConnection())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    foreach (var val in vals)
                    {
                        try
                        {
                            conn.Execute(val.Key, val.Value, trans, 30, CommandType.Text);
                        }
                        catch (Exception e)
                        {
                            //if (result <= 0)
                            trans.Rollback();
                            return false;
                        }
                    }

                    trans.Commit();
                }

                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return true;
        }
    }

    public class DbContext : IDisposable
    {
        private readonly MySqlConnection _conn;

        public DbContext()
        {
            _conn = DapperFactory.CreateMySqlConnection();
            if (_conn.State != ConnectionState.Open)
                _conn.Open();
        }

        public List<T> Query<T>(string sql, object param)
        {
            var result = _conn.Query<T>(sql, param).ToList();
            return result;
        }

        public T QuerySingle<T>(string sql, object param)
        {
            T t;

            t = _conn.Query<T>(sql, param).SingleOrDefault();

            return t;
        }

        public bool Execute<T>(List<T> list, string sql)
        {
            var result = -1;

            using (var trans = _conn.BeginTransaction())
            {
                foreach (var item in list)
                {
                    try
                    {
                        result = _conn.Execute(sql, item, trans, 30, CommandType.Text);
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        return false;
                    }

                }

                trans.Commit();
            }
            return true;
        }

        public bool Execute<T>(T t, string sql)
        {
            int result = -1;

            using (var trans = _conn.BeginTransaction())
            {
                result = _conn.Execute(sql, t, trans, 30, CommandType.Text);
                if (result <= 0)
                    trans.Rollback();
                else
                    trans.Commit();
            }
            return result > 0;
        }

        public bool Execute(List<KeyValuePair<string, object>> vals)
        {
            using (var trans = _conn.BeginTransaction())
            {
                foreach (var val in vals)
                {
                    try
                    {
                        _conn.Execute(val.Key, val.Value, trans, 30, CommandType.Text);
                    }
                    catch (Exception e)
                    {
                        //if (result <= 0)
                        trans.Rollback();
                        return false;
                    }
                }

                trans.Commit();
            }
            return true;
        }

        public void Dispose()
        {
            if (_conn.State != ConnectionState.Closed)
                _conn.Close();
        }
    }

    /// <summary>
    /// 分页实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pagination<T>
    {
        public List<T> Models { get; set; }

        public int TotalCount { get; set; }
    }
}
