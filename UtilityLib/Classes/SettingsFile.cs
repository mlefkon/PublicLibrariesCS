// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace UtilityLib {
    public class SettingsFile {
        private Dictionary<string, string> _settings;
        private string _settingsFileName;
        private object _rwLock = new object();
        
        public SettingsFile(string CompanyName, string AppName, string FileName) : this(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + CompanyName + "\\" + AppName + "\\" + FileName) {}
        public SettingsFile(string AppName, string FileName)                     : this(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\"                      + AppName + "\\" + FileName) {}
        public SettingsFile(string FilePathAndName) {
            _settingsFileName = FilePathAndName;
            Load();
        }
        public void Load() {
            lock (_rwLock) {
                _settings = new Dictionary<string, string>();
                XmlDocument xmlSettings = new XmlDocument();
                try { xmlSettings.Load(_settingsFileName);
                    } catch (Exception) { return; }
                if (xmlSettings.SelectSingleNode("/Settings/*") == null) return;
                xmlSettings.SelectNodes("/Settings/*").Cast<XmlNode>().ForEach(n => _settings.Add(n.Name, n.InnerText.Trim()));
            }
        }
        public void Save() {
            lock (_rwLock) {
                XmlDocument xmlSettings = new XmlDocument();
                xmlSettings.AppendChild(xmlSettings.CreateElement("Settings", null));
                foreach (var kv in _settings.OrderBy(n => n.Key)) {
                    XmlNode node = xmlSettings.CreateNode(XmlNodeType.Element, kv.Key, null);
                    node.InnerText = kv.Value.Trim();
                    xmlSettings.DocumentElement.SelectSingleNode("/Settings").AppendChild(node);
                }
                try { string settingsPath = Path.GetDirectoryName(_settingsFileName);
                      if (!Directory.Exists(settingsPath)) Directory.CreateDirectory(settingsPath);
                      xmlSettings.Save(_settingsFileName);
                } catch (Exception) {}
            }
        }
        public T GetSetting<T>(string SettingName, T Default = default(T)) {
            lock (_rwLock) {
                if (!_settings.ContainsKey(SettingName)) return Default;
                // Note: there are a few Types that Convert.ChangeType won't work for (so don't use these types): TimeSpan
                try {                 return (T)(Convert.ChangeType(_settings[SettingName], typeof(T) ));
                } catch (Exception) { return Default; }
            }
        }
        public void SetSetting<T>(string SettingName, T Value) {
            lock (_rwLock) {
                if (!_settings.ContainsKey(SettingName)) _settings.Add(SettingName, "");
                _settings[SettingName] = Value.ToString();
            }
        }
    }
}
