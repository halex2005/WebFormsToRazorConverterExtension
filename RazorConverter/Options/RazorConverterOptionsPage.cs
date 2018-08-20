using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionPages;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Resources;

namespace RazorConverter.Options
{
	[OptionsPage(Pid,
		"ASP.NET WebForms to Razor Converter Plugin",
		typeof(FeaturesEnvironmentOptionsThemedIcons.Quickfixes),
		ParentId = ToolsPage.PID)]
	public class RazorConverterOptionsPage : SimpleOptionsPage
	{
		private const string Pid = "RazorConverterOptions";

		public RazorConverterOptionsPage(
			[NotNull] Lifetime lifetime,
			[NotNull] OptionsSettingsSmartContext optionsSettingsSmartContext)
			: base(lifetime, optionsSettingsSmartContext)
		{
			IProperty<bool> deleteOriginalFile = new Property<bool>(
				lifetime,
				"RazorConverterOptionsPage::DeleteOriginalFile");

			deleteOriginalFile.SetValue(optionsSettingsSmartContext
				.StoreOptionsTransactionContext
				.GetValue((RazorConverterSettings key) => key.DeleteOriginalFile));

			deleteOriginalFile.Change.Advise(lifetime, a =>
			{
				if (!a.HasNew) return;
				optionsSettingsSmartContext
					.StoreOptionsTransactionContext
					.SetValue((RazorConverterSettings key) => key.DeleteOriginalFile, a.New);
			});

			AddBoolOption((RazorConverterSettings key) => key.DeleteOriginalFile, "Delete original file");
		}
	}
}