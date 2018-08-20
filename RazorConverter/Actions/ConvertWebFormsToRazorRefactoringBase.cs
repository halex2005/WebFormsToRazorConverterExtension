using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace RazorConverter.Actions
{
    public class ConvertWebFormsToRazorRefactoringBase
        : RefactoringExecBase<ConvertWebFormsToRazorRefactoringWorkflow, ConvertWebFormsToRazorRefactoringExecutor>
    {
        public ConvertWebFormsToRazorRefactoringBase(
            ConvertWebFormsToRazorRefactoringWorkflow workflow,
            ISolution solution,
            IRefactoringDriver driver)
            : base(workflow, solution, driver)
        {
        }
    }
}