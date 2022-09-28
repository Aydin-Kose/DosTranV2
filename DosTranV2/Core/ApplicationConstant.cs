using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DosTranV2.Core
{
    internal static class ApplicationConstant
    {
        public const string VER = "2.0.0";
        public const string CONN_STRING = @"Server=TORTAKDB02;Database=Insankaynaklari;User Id=perfusr; Password='PERF1usr37 ';Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True";
        public static readonly List<EnvironmentModel> EnvironmentList = new List<EnvironmentModel> {
                new EnvironmentModel { Name="Dev", IP= "ftp://10.4.141.100" },
                new EnvironmentModel { Name="Test", IP= "ftp://10.4.141.100" },
                new EnvironmentModel { Name="Prod", IP= "ftp://10.4.141.100" }
            };
    }

    public class EnvironmentModel
    {
        public string Name { get; set; }
        public string IP { get; set; }
    }
}
