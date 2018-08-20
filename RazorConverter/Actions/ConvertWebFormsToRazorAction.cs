using System;
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
    public class ConvertWebFormsToRazorAction
        : IExecutableAction
        , IInsertLast<MainMenuToolsGroup>
        , IInsertLast<ToolsMenu>
    {
        private static readonly ProjectFileLocationEqualityComparer ProjectFileLocationEqualityComparer = new ProjectFileLocationEqualityComparer();

        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            var selectedProjectElements = context.GetData(ProjectModelDataConstants.PROJECT_MODEL_ELEMENTS);
            return GetSourceFilesToConvert(selectedProjectElements).Any();
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            var elementsToConvert = GetSourceFileToConvert(context).ToArray();

            MessageBox.ShowInfo(elementsToConvert.Length > 0
                ? $"{elementsToConvert.Length} files can be converter"
                : $"No file can be converted");
        }

        public static IEnumerable<IProjectFile> GetSourceFileToConvert(IDataContext context)
        {
            var selectedProjectElements = context.GetData(ProjectModelDataConstants.PROJECT_MODEL_ELEMENTS);

            if (selectedProjectElements == null)
            {
                return Array.Empty<IProjectFile>();
            }

            return selectedProjectElements
                .SelectMany(GetSourceFilesToConvert)
                .Distinct(ProjectFileLocationEqualityComparer);
        }

        private static IEnumerable<IProjectFile> GetSourceFilesToConvert(IEnumerable<IProjectModelElement> elements)
        {
            return elements.SelectMany(GetSourceFilesToConvert);
        }

        private static IEnumerable<IProjectFile> GetSourceFilesToConvert(IProjectModelElement element)
        {
            switch (element)
            {
                case IProjectFile projectFile when CheckIsAspWebFormsFile(projectFile):
                    yield return projectFile;
                    yield break;

                case IProjectFolder projectFolder:
                    foreach (var file in GetSourceFilesToConvert(projectFolder.GetSubItems()))
                    {
                        yield return file;
                    }
                    break;

                case ISolution solution:
                    foreach (var file in GetSourceFilesToConvert(solution.GetAllProjects()))
                    {
                        yield return file;
                    }
                    break;
            }
        }

        private static bool CheckIsAspWebFormsFile(IProjectFile sourceFile)
        {
            return sourceFile != null &&
                   sourceFile.LanguageType.Is<AspProjectFileType>() &&
                   sourceFile.Location.ExtensionWithDot != ".asax";
        }
    }

    internal class ProjectFileLocationEqualityComparer : IEqualityComparer<IProjectFile>
    {
        public bool Equals(IProjectFile x, IProjectFile y)
        {
            if (x == null || y == null)
                return false;
            return Equals(x.Location, y.Location);
        }

        public int GetHashCode(IProjectFile obj)
        {
            return obj.Location.GetHashCode();
        }
    }
}
