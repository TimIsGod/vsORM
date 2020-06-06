using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using 手写ORM.Model;
using 手写ORM.ORMFramework;
using 手写ORM.ORMFramework.Extends;
using System.Linq;
using System.Linq.Expressions;
using ExpressionSql;

namespace 手写ORM.DAL
{
    public class SqlHelper
    {
        //private static string _ConnectionStringDemo = ORMConfigurationManager.SqlConnStringDemo;

        public T Find<T>(long id) where T : EntityBase, new()
        {
            Type type = typeof(T);
            string sql = SqlCacheBuilder<T>.GetSql(SqlCacheBuilderType.FindOne);
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@ID",id)
            };
            return this.ExecuteSql<T>(sql, sqlParams, cmd =>
            {
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    T t = new T();
                    foreach (var item in type.GetProperties())
                    {
                        string propertyName = item.GetMappingName();
                        item.SetValue(t, reader[propertyName] is DBNull ? null : reader[propertyName]);
                    }
                    return t;
                }
                return null;
            }, SqlConnectionPool.GetConnectionString(DBOPerateType.Write));

        }

        public List<T> Find<T>(Expression<Func<T ,bool>> lambda) where T : EntityBase, new()
        {
            Type type = typeof(T);
            string sql = SqlCacheBuilder<T>.GetSql(SqlCacheBuilderType.FindAll);
            ConvertToSqlExpressionVisitor visitor = new ConvertToSqlExpressionVisitor();
            visitor.Visit(lambda);
            string where = visitor.GetWhere();
            sql += where;
            return this.ExecuteSql<List<T>>(sql, null, cmd =>
            {
                List<T> list = new List<T>();
                var reader =  cmd.ExecuteReader();
                while (reader.Read())
                {
                    T t = new T();
                    foreach (var item in type.GetProperties())
                    {
                        string propertyName = item.GetMappingName();
                        item.SetValue(t, reader[propertyName] is DBNull ? null : reader[propertyName]);
                    }
                    list.Add(t);
                }
                return list;
            },SqlConnectionPool.GetConnectionString(DBOPerateType.Write));

        }
        public long Insert<T>(T t) where T : EntityBase, new()
        {
            Type type = typeof(T);

            string sql = SqlCacheBuilder<T>.GetSql(SqlCacheBuilderType.Insert);
            var sqlParams = type.GetProperties().Select(r => new SqlParameter($"@{r.GetMappingName()}", r.GetValue(t) ?? DBNull.Value)).ToArray();
            //注意指定的返回值泛型类型要和方法的返回值类型一致
            return this.ExecuteSql<long>(sql, sqlParams, cmd =>
            {
                object result = cmd.ExecuteScalar();//这里必须在insert语句后加一句查询id
                long id = 0L;
                long.TryParse(result.ToString(), out id);
                return id;
            }, SqlConnectionPool.GetConnectionString(DBOPerateType.Write));//因为懒得建立slave库，就全用写的了
        }

        public bool Update<T>(T t) where T : EntityBase, new()
        {
            Type type = typeof(T);
            string sql = SqlCacheBuilder<T>.GetSql(SqlCacheBuilderType.Update);
            var SqlParams = type.GetProperties().Select(r => new SqlParameter($"@{r.GetMappingName()}", r.GetValue(t) ?? DBNull.Value)).ToArray();
            return this.ExecuteSql<bool>(sql, SqlParams, cmd => cmd.ExecuteNonQuery() == 1, SqlConnectionPool.GetConnectionString(DBOPerateType.Write));
        }

        public bool Delete<T>(T t) where T : EntityBase, new()
        {
            Type typte = typeof(T);
            string sql = SqlCacheBuilder<T>.GetSql(SqlCacheBuilderType.Delete);
            var sqlParams = new SqlParameter("@ID", t.Id);
            return this.ExecuteSql<bool>(sql, new SqlParameter[] { sqlParams }, cmd => cmd.ExecuteNonQuery() == 1, SqlConnectionPool.GetConnectionString(DBOPerateType.Write));
        }


        /// <summary>
        /// 将通用的Sql对象创建的代码进行封装
        /// 对于sql对象使用不同地方，用委托额方式传入使用方法
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="sql"></param>
        /// <param name="sqlParams"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private S ExecuteSql<S>(string sql, SqlParameter[] sqlParams, Func<SqlCommand, S> func, string connString)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (sqlParams != null)
                {
                    cmd.Parameters.AddRange(sqlParams);
                }
                conn.Open();
                //使用委托的方法执行
                return func.Invoke(cmd);
            }

        }
    }
}
