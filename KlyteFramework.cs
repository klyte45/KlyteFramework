using Klyte.Commons.Extensors;
using Klyte.Commons.i18n;
using Klyte.Commons.Interfaces;
using Klyte.Commons.Utils;
using Klyte.Framework.Utils;
using System.IO;
using System.Reflection;
using UnityEngine;

[assembly: AssemblyVersion("1.0.0.0")]
namespace Klyte.Framework
{
    public class KlyteFramework : BasicIUserModSimplified<KlyteFramework, KFResourceLoader, MonoBehaviour>
    {
        public KlyteFramework() => Construct();

        private bool m_alreadyLoadedTranslations = false;

        public override string SimpleName => "Klyte's Framework";

        public override string Description => "Allow to any asset to use the Klyte mods common features, like the translation";

        public override void DoErrorLog(string fmt, params object[] args) => LogUtils.DoErrorLog(fmt, args);

        public override void DoLog(string fmt, params object[] args) => LogUtils.DoLog(fmt, args);
        public override void LoadSettings() { }
        public override void TopSettingsUI(UIHelperExtension ext)
        {
            if (!m_alreadyLoadedTranslations)
            {
                ScanTranslations();
            }
            ext.AddButton("Reload translation files", ScanTranslations);
        }

        private void ScanTranslations()
        {
            FileUtils.ScanPrefabsFoldersDirectoryNoLoad("kf_translationFiles", (path, pack, asset) =>
            {
                foreach (var lang in KlyteLocaleManager.locales)
                {
                    var fileName = Path.Combine(path, $"{lang}.txt");
                    LogUtils.DoLog("PATH: " + fileName);
                    if (File.Exists(fileName))
                    {
                        var content = File.ReadAllText(fileName);
                        if (content != null)
                        {
                            File.WriteAllText($"{KlyteLocaleManager.m_translateFilesPath}{lang}{Path.DirectorySeparatorChar}9_KF_{(asset.isWorkshopAsset ? $"{(~pack.GetPublishedFileID().AsUInt64).ToString("00000000000000000")}_{asset.name}" : asset.name)}.txt", content);
                        }
                    }

                }
            });
            KlyteLocaleManager.ReloadLanguage(true);
            m_alreadyLoadedTranslations = true;
        }
    }
}