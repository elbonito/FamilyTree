using System;
using System.Collections.Generic;
using System.Text;
using AppCoreX.Interface;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace AppCoreX.Helpers
{
    public class AppSettings : IAppSettings
    {
        private readonly ISettings _settings;

        public AppSettings(ISettings settings)
        {
            _settings = settings;
        }

        #region Setting Constants

        //private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public string DatabaseName
        {
            get => _settings.GetValueOrDefault(nameof(DatabaseName), SettingsDefault);
            set => _settings.AddOrUpdateValue(nameof(DatabaseName), value);
        }



        public string DocumentCollectionName
        {
            get => _settings.GetValueOrDefault(nameof(DocumentCollectionName), SettingsDefault);
            set => _settings.AddOrUpdateValue(nameof(DocumentCollectionName), value);
        }

    }
}
