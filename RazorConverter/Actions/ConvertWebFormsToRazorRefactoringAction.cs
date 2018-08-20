using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.UI.RichText;

namespace RazorConverter.Actions
{
    [Action("ConvertWebFormsToRazorRefactoring", "Convert ASP.NET WebForm to Razor", Id = 2018082000)]
    public class ConvertWebFormsToRazorRefactoringAction : ExtensibleRefactoringAction<ConvertWebFormsToRazorWorkflowProvider>
    {
        protected override RichText Caption => "Convert ASP.NET WebForm to Razor";
    }
}
