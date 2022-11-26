using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanglaConverter
{
    internal static class SettingsManager
    {
        /// <summary>
        /// Represents the name of an option that the user can set.
        /// </summary>
        public enum Setting
        {
            TextColor,
            BackgroundColor,
            FormColor,
            FontSize
        }

        private static Dictionary<Setting, object> currentSettings = new Dictionary<Setting, object>
        {
            [Setting.TextColor] = SystemColors.WindowText,
            [Setting.BackgroundColor] = SystemColors.Window,
            [Setting.FormColor] = SystemColors.Control,
            [Setting.FontSize] = 12.0F
        };

        public static object GetSetting(Setting setting)
        {
            return currentSettings[setting];
        }

        public static void SetSetting(Setting setting, object value)
        {
            currentSettings[setting] = value;
        }

    }
}
