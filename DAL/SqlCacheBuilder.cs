using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using 手写ORM.Model;
using 手写ORM.ORMFramework.Extends;

namespace 手写ORM.DAL
{
    //约束一下类型
    public class SqlCacheBuilder<T> where T : EntityBase, new()
    {
        private static string _InsertSql = null;
        private static string _FindOneSql = null;
        private static string _FindAll = null;
        private static string _UpdateSql = null;
        private static string _DeleteSql = null;

        static SqlCacheBuilder()
        {
            Type t = typeof(T);
            {
                string filedString = string.Join(",", t.GetProperties().Select(r => $"[{r.GetMappingName()}]"));
                _FindOneSql = $@"SELECT {filedString} FROM [{t.GetMappingName()}] WHERE ID =@ID";
            }
            {
                string filedString = string.Join(",", t.GetProperties().Select(r => $"[{r.GetMappingName()}]"));
                _FindAll = $@"SELECT {filedString} FROM [{t.GetMappingName()}] WHERE ";
            }
            {
                string filedString = string.Join(",", t.GetPropertiesWithNotPrimayKey().Select(r => $"[{r.GetMappingName()}]"));
                string valueString = string.Join(",", t.GetPropertiesWithNotPrimayKey().Select(r => $"@{r.GetMappingName()}"));
                _InsertSql = $"INSERT INTO [{t.GetMappingName()}] ({filedString}) VALUES ({valueString});SELECT @@IDENTITY";
            }
            {
                string columnValueString = string.Join(",", t.GetPropertiesWithNotPrimayKey().Select(p => $"{p.GetMappingName()}=@{p.GetMappingName()}"));
                _UpdateSql = $"Update [{t.GetMappingName()}] Set {columnValueString} Where Id=@Id;";
            }
            {
                _DeleteSql = $"Delete From [{t.GetMappingName()}] Where Id=@Id";
            }
        }

        internal static string GetSql(SqlCacheBuilderType sqlType)
        {
            switch (sqlType)
            {
                case SqlCacheBuilderType.FindOne:
                    return _FindOneSql;
                case SqlCacheBuilderType.FindAll:
                    return _FindAll;
                case SqlCacheBuilderType.Insert:
                    return _InsertSql;
                case SqlCacheBuilderType.Update:
                    return _UpdateSql;
                case SqlCacheBuilderType.Delete:
                    return _DeleteSql;
                default:
                    throw new Exception("Unknown sqlCacheBuildertType!");
            }
        }
    }

    internal enum SqlCacheBuilderType
    {
        FindOne,
        FindAll,
        Insert,
        Update,
        Delete
    }

    /// <summary>
    /// 常规的字典缓存--存入+获取模式--缓存的思路是一样的
    /// 优势：灵活方便，数据的保存以key为准,
    /// 劣势：性能问题--性能主要是数据超过1w(大概)以上会有下降
    ///       hash存储--增删改查性能都差不多--但是最怕数据太多，多了有损耗
    /// 下面的例子可将赋值后的sql语句存到字典中，通过Key进行获取，不用枚举类参与了
    /// </summary>
    internal class CustomCache
    {
        private static Dictionary<string, string> CustomCacheDictionary = new Dictionary<string, string>();

        public static void Add(string key, string value)
        {
            CustomCacheDictionary.Add(key, value);
        }

        public static string Get(string key)
        {
            return CustomCacheDictionary[key];
        }
    }
}
