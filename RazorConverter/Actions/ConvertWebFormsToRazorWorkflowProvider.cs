using System.Collections.Generic;
using JetBrains.Application.DataContext;
using JetBrains.Application.UI.PopupLayout;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.ReSharper.Resources.Shell;
using RazorConverter.Options;

namespace RazorConverter.Actions
{
    [RefactoringWorkflowProvider]
    public class ConvertWebFormsToRazorWorkflowProvider : IRefactoringWorkflowProvider
    {
        private readonly RazorConverterSettingsStore optionsStore;

        public const string ActionId = "ConvertWebFormsToRazorRefactoring";

        public ConvertWebFormsToRazorWorkflowProvider(RazorConverterSettingsStore optionsStore)
        {
            this.optionsStore = optionsStore;
        }

        public IEnumerable<IRefactoringWorkflow> CreateWorkflow(IDataContext dataContext)
        {
            var solution = dataContext.GetData(ProjectModelDataConstants.SOLUTION);
            if (solution == null)
            {
                yield break;
            }

            yield return new ConvertWebFormsToRazorRefactoringWorkflow(
                solution,
                ActionId,
                optionsStore,
                Shell.Instance.GetComponent<IMainWindowPopupWindowContext>());
        }
    }
}
