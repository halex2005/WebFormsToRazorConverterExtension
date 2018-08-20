using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.Application.Progress;
using JetBrains.Application.UI.PopupLayout;
using JetBrains.DocumentManagers.Transactions;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Navigation;
using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;
using RazorConverter.Options;

namespace RazorConverter.Actions
{
    public class ConvertWebFormsToRazorRefactoringWorkflow : DrivenRefactoringWorkflow
    {
        private readonly RazorConverterSettingsStore optionsStore;
        private readonly IMainWindowPopupWindowContext mainWindowPopupWindowContext;

        public ConvertWebFormsToRazorDataModel DataModel { get; private set; }

        public ConvertWebFormsToRazorRefactoringWorkflow(
            [NotNull] ISolution solution,
            [CanBeNull] string actionId,
            RazorConverterSettingsStore optionsStore,
            IMainWindowPopupWindowContext mainWindowPopupWindowContext)
            : base(solution, actionId)
        {
            this.optionsStore = optionsStore;
            this.mainWindowPopupWindowContext = mainWindowPopupWindowContext;
        }

        public override bool Initialize(IDataContext context)
        {
            return DataModel.FilesToConvert != null && Enumerable.Any(DataModel.FilesToConvert);
        }

        public override string HelpKeyword => null;
        public override IRefactoringPage FirstPendingRefactoringPage => null;
        public override bool MightModifyManyDocuments => true;
        public override string Title => "Convert WebForm To Razor";
        public override RefactoringActionGroup ActionGroup => RefactoringActionGroup.IntroduceEntity;

        public override bool IsAvailable(IDataContext context)
        {
            DataModel = new ConvertWebFormsToRazorDataModel(ConvertWebFormsToRazorAction.GetSourceFileToConvert(context));
            return DataModel.FilesToConvert.Length > 0;
        }

        public override IRefactoringExecuter CreateRefactoring(IRefactoringDriver driver)
        {
            return new ConvertWebFormsToRazorRefactoringExecutor(this, driver);
        }

        public override bool PostExecute(IProgressIndicator pi)
        {
            AddAllConvertedFilesToProject();
            Solution.GetPsiServices().Files.CommitAllDocuments();
            DeleteOriginalFiles();
            return true;
        }

        private void DeleteOriginalFiles()
        {
            if (optionsStore.GetSettings().DeleteOriginalFile)
            {
                using (var transactionCookie = Solution.CreateTransactionCookie(DefaultAction.Commit, "Refactroring"))
                {
                    foreach (var file in DataModel.FilesToConvert)
                    {
                        transactionCookie.Remove(file.OriginalFile, true);
                    }
                }
            }
        }

        private void AddAllConvertedFilesToProject()
        {
            foreach (var file in DataModel.FilesToConvert)
            {
                var targetFolder = file.OriginalFile.ParentFolder;
                if (targetFolder == null || file.ConvertedFileLocation == null)
                {
                    continue;
                }

                using (var transactionCookie = targetFolder
                    .GetSolution()
                    .CreateTransactionCookie(DefaultAction.Commit, "Add converted file to project"))
                {
                    file.ConvertedFile = transactionCookie.AddFile(targetFolder, file.ConvertedFileLocation);
                }
            }
        }

        public override void SuccessfulFinish(IProgressIndicator pi)
        {
            if (DataModel.FilesToConvert.Length == 1)
            {
                var target = new ProjectFileNavigationPoint(DataModel.FilesToConvert[0].ConvertedFile);
                NavigationManager
                    .GetInstance(Solution)
                    .Navigate(target, NavigationOptions.FromWindowContext(mainWindowPopupWindowContext.Source, ""));
            }

            base.SuccessfulFinish(pi);
        }

    }
}
