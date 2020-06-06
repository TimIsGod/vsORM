using ExpressionSql;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using 手写ORM.DAL;
using 手写ORM.Model;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            EntityCompany company = new EntityCompany();
            company.Name = "asdf";
            company.Number = "1ads23";
            company.CreatedDate = DateTime.Now;

            SqlHelper sh = new SqlHelper();
            //long id = sh.Insert<EntityCompany>(company);

            ConvertToSqlExpressionVisitor visitor = new ConvertToSqlExpressionVisitor();
            //List<long> ids = new List<long>();
            //ids.Add(3);
            //ids.Add(4);
            //ids.Add(5);
            //Expression<Func<EntityCompany, bool>> expression = c => (c.Id > 1
            //            && c.Id < 10
            //            || c.Id == 5)
            //            && c.Number.StartsWith("1")
            //            && (c.Number.EndsWith("2")
            //            || c.Number.Contains("3"))
            //            && ids.Contains(c.Id);

            Expression<Func<EntityCompany, bool>> expression = r => r.Id > 2 && r.Id < 10 && r.Number.Contains("1");
            var list =  sh.Find<EntityCompany>(expression);

            Console.WriteLine( list.Count );

            Console.Read();

        }
    }
}
