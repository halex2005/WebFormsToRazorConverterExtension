using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.Application.Progress;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.Actions.MenuGroups;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.Application.UI.ActionSystem.ActionsRevised.Menu;
using JetBrains.Application.UI.PopupLayout;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Feature.Services.Menu;
using JetBrains.ReSharper.Resources.Shell;
using RazorConverter.Actions;
using RazorConverter.Options;

namespace RazorConverter
{
    [ActionAttribute("Convert ASP.NET WebForm(s) to Razor", Id = 2018081800)]
    public class ConvertWebFormsToRazorAction
        : IExecutableAction
        , IInsertLast<MainMenuToolsGroup>
        , IInsertLast<ToolsMenu>
    {
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            return context.GetSourceFilesToConvert().Any();
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            var solution = context.GetData(ProjectModelDataConstants.SOLUTION);
            if (solution == null)
            {
                return;
            }

            var workflow = new ConvertWebFormsToRazorRefactoringWorkflow(
                solution,
                nameof(ConvertWebFormsToRazorAction),
                context.GetComponent<RazorConverterSettingsStore>(),
                Shell.Instance.GetComponent<IMainWindowPopupWindowContext>());

            if (!workflow.IsAvailable(context) || !workflow.Initialize(context))
            {
                return;
            }

            using (var pi = NullProgressIndicator.Create())
            {
                workflow.PreExecute(pi);
                workflow.Execute(pi);
                workflow.PostExecute(pi);
            }
        }
    }

}
