using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using 手写ORM.ORMFramework;

namespace 手写ORM.DAL
{
    public static class SqlConnectionPool
    {
        private static int _Index = 0;
        internal static string GetConnectionString(DBOPerateType operate)
        {
            switch (operate)
            {
                case DBOPerateType.Write:
                    {
                        return ORMConfigurationManager.SqlConnStringWrite;
                    }
                    break;
                case DBOPerateType.Read:
                    {
                        return DispatcherConn();
                    }
                    break;
                default:
                    break;
            }
            return null;
        }
        //轮询访问只读库
        private static string DispatcherConn()
        {
            return ORMConfigurationManager.SqlConnStringRead[_Index % ORMConfigurationManager.SqlConnStringRead.Length];
        }
    }

    internal enum DBOPerateType
    {
        Write,
        Read
    }
}
