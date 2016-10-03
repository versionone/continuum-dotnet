using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuumDotNet.Api
{
    public class Constants
    {
        public static string ContinuumApiPath = "api";
        public static string UltimateProduct = "Ultimate";
        public static string EnterpriseProduct = "Enterprise";
        public static string TeamProduct = "Team";
        public static string CatalystProduct = "Catalyst";
        public static List<string> AllProductEditions = 
            new List<string>()
            {
                UltimateProduct,
                EnterpriseProduct,
                TeamProduct,
                CatalystProduct
            };

        public static string AvailableServerKey = "available_servers";
    }
}
