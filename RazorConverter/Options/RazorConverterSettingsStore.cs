using JetBrains.Application;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;

namespace RazorConverter.Options
{
    [ShellComponent]
    public class RazorConverterSettingsStore
    {
        private readonly ISettingsStore settingsStore;
        private readonly DataContexts dataContexts;

        public RazorConverterSettingsStore(ISettingsStore settingsStore, DataContexts dataContexts)
        {
            this.settingsStore = settingsStore;
            this.dataContexts = dataContexts;
        }

        public RazorConverterSettings GetSettings()
        {
            var boundSettings = BindSettingsStore();
            return boundSettings.GetKey<RazorConverterSettings>(SettingsOptimization.OptimizeDefault);
        }

        // Set-tastic
        public void SetSettings(RazorConverterSettings settings)
        {
            var boundSettings = BindSettingsStore();
            boundSettings.SetKey(settings, SettingsOptimization.OptimizeDefault);
        }

        private IContextBoundSettingsStore BindSettingsStore()
        {
            return settingsStore.BindToContextTransient(ContextRange.Smart((l, _) => dataContexts.CreateOnSelection(l)));
        }
    }
}
