using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.Application.Threading;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.ActionsRevised.Handlers;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.Application.UI.ActionSystem.ActionsRevised.Menu;
using JetBrains.DocumentManagers.Transactions.Actions;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ProjectModel.PropertiesExtender;
using JetBrains.ReSharper.Feature.Services.Actions;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.Intentions;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Asp;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.Util;

namespace RazorConverter
{
    [Action("Convert ASP.NET WebForm(s) to Razor")]
    public class ConvertWebFormsToRazorAction: IExecutableAction, IActionWithUpdateRequirement
    {
        public ConvertWebFormsToRazorAction()
        {
        }

        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            var solution = context.GetData(ProjectModelDataConstants.SOLUTION);
            if (solution != null)
            {
                presentation.Text = "Convert ASP.NET WebForms to Razor for solution";
                var sourceFiles = solution
                    .GetAllProjects()
                    .SelectMany(p => p.GetSubItemsRecursively())
                    .OfType<IProjectFile>()
                    .SelectMany(pf => pf.ToSourceFiles().ToArray());
                return CheckHasAspWebFormsFile(sourceFiles);
            }

            var project = context.GetData(ProjectModelDataConstants.PROJECT);
            if (project != null)
            {
                presentation.Text = "Convert ASP.NET WebForms to Razor for selected project";
                var sourceFiles = project
                    .GetSubItemsRecursively()
                    .OfType<IProjectFile>()
                    .SelectMany(pf => pf.ToSourceFiles().ToArray());
                return CheckHasAspWebFormsFile(sourceFiles);
            }

            if (context.GetData(ProjectModelDataConstants.PROJECT_MODEL_ELEMENT) is IProjectFile projectFile)
            {
                presentation.Text = "Convert ASP.NET WebForm to Razor";
                var sourceFiles = projectFile.ToSourceFiles().ToArray();
                return CheckHasAspWebFormsFile(sourceFiles);
            }

            return false;
        }

        private bool CheckHasAspWebFormsFile<TResult>(IEnumerable<TResult> sourceFiles)
        {
            if (sourceFiles == null)
            {
                return false;
            }

            return sourceFiles
                .OfType<IPsiSourceFile>()
                .Any(sourceFile => sourceFile.IsLanguageSupported<AspLanguage>());
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            throw new NotImplementedException();
        }

        public IActionRequirement GetRequirement(IDataContext dataContext)
        {
            return CurrentPsiFileRequirement.FromDataContext(dataContext);
        }
    }
}
