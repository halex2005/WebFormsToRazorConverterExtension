using JetBrains.Application.Settings;
using JetBrains.Application.Settings.WellKnownRootKeys;

namespace RazorConverter.Options
{
    [SettingsKey(typeof(EnvironmentSettings), "RazorConverter settings")]
    public class RazorConverterSettings
    {
        [SettingsEntry(false, "Delete original file")]
        public bool DeleteOriginalFile;
    }
}
