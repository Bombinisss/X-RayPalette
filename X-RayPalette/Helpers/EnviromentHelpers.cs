using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_RayPalette.Helpers
{
    public class EnviromentHelpers
    {
        public static bool IsSupportedOS(int major=10, int build =19041)
        {
            return Environment.OSVersion.Version.Major >= major && Environment.OSVersion.Version.Build >= build;
        }
    }
}
