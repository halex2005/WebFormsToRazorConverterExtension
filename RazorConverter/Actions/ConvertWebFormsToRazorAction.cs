using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.Actions.MenuGroups;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.Application.UI.ActionSystem.ActionsRevised.Menu;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Feature.Services.Menu;
using JetBrains.Util;

namespace RazorConverter
{
    [ActionAttribute("Convert ASP.NET WebForm(s) to Razor", Id = 2018081800)]
    public class ConvertWebFormsToRazorAction: IExecutableAction, IInsertLast<MainMenuToolsGroup>, IInsertLast<ToolsMenu>
    {
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            if (context.GetData(ProjectModelDataConstants.PROJECT_MODEL_ELEMENT) is IProjectFile projectFile)
            {
                presentation.Text = "Convert ASP.NET WebForm to Razor";
                var sourceFiles = new [] { projectFile };
                return CheckHasAspWebFormsFile(sourceFiles);
            }

            return false;
        }

        private bool CheckHasAspWebFormsFile(IEnumerable<IProjectFile> sourceFiles)
        {
            if (sourceFiles == null)
            {
                return false;
            }

            return sourceFiles.Any(s => s.LanguageType.Is<AspProjectFileType>());
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            var file = context.GetData(ProjectModelDataConstants.PROJECT_MODEL_ELEMENT);
            var projectFile = file as IProjectFile;
            MessageBox.ShowInfo(projectFile?.Location != null
                ? $"{projectFile.Location} file is opened"
                : $"No file is opened: {file?.Name}, {file}");
        }
    }
}
