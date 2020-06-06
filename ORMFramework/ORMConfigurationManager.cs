using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace 手写ORM.ORMFramework
{
    public class ORMConfigurationManager
    {
        private static string _SqlConnectionWriteString = string.Empty;
        private static string[] _SqlConnectionReadStrings = null;

        //添加引用Microsoft.Extensions.Configuration/File/Json 
        static ORMConfigurationManager()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();
            _SqlConnectionWriteString = configuration["DBConnectionStrings:Demo:Write"];
            _SqlConnectionReadStrings = configuration.GetSection("DBConnectionStrings").GetSection("Demo").GetSection("Read").GetChildren().Select(r => r.Value).ToArray();
        }

        public static string SqlConnStringWrite
        {
            get { return _SqlConnectionWriteString; }

        }

        public static string[] SqlConnStringRead
        {
            get { return _SqlConnectionReadStrings; }
        }
    }
}
