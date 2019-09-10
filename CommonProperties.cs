using Klyte.Framework;
using Klyte.Framework.Utils;

namespace Klyte.Commons
{
    public static class CommonProperties
    {
        public static bool DebugMode => KlyteFramework.DebugMode;
        public static string Version => KlyteFramework.Version;
        public static string ModName => KlyteFramework.Instance.SimpleName;
        public static string Acronym => "KF";
        public static string ResourceBasePath => KFResourceLoader.instance.Prefix;
    }
}