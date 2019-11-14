using ColossalFramework.UI;
using Klyte.Commons.Extensors;
using Klyte.Commons.i18n;
using Klyte.Commons.Interfaces;
using Klyte.Commons.Utils;
using Klyte.Framework.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static ColossalFramework.UI.UITextureAtlas;

[assembly: AssemblyVersion("1.1.0.0")]
namespace Klyte.Framework
{
    public class KlyteFramework : BasicIUserModSimplified<KlyteFramework, KFResourceLoader, MonoBehaviour>
    {
        public KlyteFramework() => Construct();

        internal static readonly string m_localImageFilesPath = $"{FileUtils.BASE_FOLDER_PATH}kf_imageFiles{Path.DirectorySeparatorChar}";
        internal static readonly string m_atlasExportPath = $"{FileUtils.BASE_FOLDER_PATH}kf_exportedAtlas{Path.DirectorySeparatorChar}";

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
            ScanImagesForTextureAtlas();

            ext.AddButton("Go to GitHub page!", () => Application.OpenURL("https://github.com/klyte45/KlyteFramework"));
            ext.AddButton("Reload translation files", ScanTranslations);
            ext.AddButton("Reload images files", ScanImagesForTextureAtlas);
            ext.AddButton("Export game images", ExportGameImages);
        }

        private void ScanTranslations()
        {
            FileUtils.ScanPrefabsFoldersDirectoryNoLoad("kf_translationFiles", (path, pack, asset) =>
            {
                foreach (string lang in KlyteLocaleManager.locales)
                {
                    foreach (string file in Directory.GetFiles($"{KlyteLocaleManager.m_translateFilesPath}{lang}{Path.DirectorySeparatorChar}", $"9_KF_*.txt"))
                    {
                        File.Delete(file);
                    }
                    string fileName = Path.Combine(path, $"{lang}.txt");
                    LogUtils.DoLog("PATH: " + fileName);
                    if (File.Exists(fileName))
                    {
                        string content = File.ReadAllText(fileName);
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

        private void ScanImagesForTextureAtlas()
        {
            var newSprites = new List<SpriteInfo>();
            FileUtils.ScanPrefabsFoldersDirectoryNoLoad("kf_imageFiles", (path, pack, asset) =>
            {
                try
                {
                    TextureAtlasUtils.LoadPathTexturesIntoInGameTextureAtlas($"PACK.{(asset.isWorkshopAsset ? $"{(pack.GetPublishedFileID().AsUInt64).ToString("00000000000000000")}" : asset.name)}", path, ref newSprites);
                }
                catch (Exception e)
                {
                    LogUtils.DoErrorLog($"An error occurred trying to load path \"{path}\": {e.GetType()} => {e.Message}");
                }
            });
            TextureAtlasUtils.LoadPathTexturesIntoInGameTextureAtlas($"KF_LOCAL", m_localImageFilesPath, ref newSprites);
            if (newSprites.Count > 0)
            {
                TextureAtlasUtils.RegenerateDefaultTextureAtlas(newSprites);
            }
        }

        private void ExportGameImages()
        {
            var borders = new Dictionary<string, RectOffset>();
            UITextureAtlas defaultTextureAtlas = UIView.GetAView().defaultAtlas;
            if (defaultTextureAtlas == null)
            {
                return;
            }
            if (Directory.Exists(m_atlasExportPath))
            {
                Directory.Delete(m_atlasExportPath, true);
            }
            Directory.CreateDirectory(m_atlasExportPath);
            foreach (SpriteInfo spriteInfo in defaultTextureAtlas.sprites)
            {
                if (spriteInfo.texture == null)
                {
                    continue;
                }
                File.WriteAllBytes($"{m_atlasExportPath}%{spriteInfo.name}.png", spriteInfo.texture.EncodeToPNG());
                if (spriteInfo.border != null && (spriteInfo.border.left > 0 || spriteInfo.border.top > 0 || spriteInfo.border.right > 0 || spriteInfo.border.bottom > 0))
                {
                    borders[spriteInfo.name] = spriteInfo.border;
                }
            }
            if (borders.Count > 0)
            {
                File.WriteAllLines($"{m_atlasExportPath}{TextureAtlasUtils.BORDER_FILENAME}", borders.Select(x => $"{x.Key}={x.Value.left},{x.Value.right},{x.Value.top},{x.Value.bottom}").ToArray());
            }

        }
    }
}