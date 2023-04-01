using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DosTranV2.Core
{
    internal static class ApplicationConstant
    {
        public const string VER = "2.0.0";
        public const string CONN_STRING = @"ConnString";
        public static readonly List<EnvironmentModel> EnvironmentList = new List<EnvironmentModel> {
                new EnvironmentModel { Name="Dev", IP= "DevIP" },
                new EnvironmentModel { Name="Test", IP= "TestIP" },
                new EnvironmentModel { Name="Prod", IP= "ProdIP" }
            };
    }

    public class EnvironmentModel
    {
        public string Name { get; set; }
        public string IP { get; set; }
    }
}
