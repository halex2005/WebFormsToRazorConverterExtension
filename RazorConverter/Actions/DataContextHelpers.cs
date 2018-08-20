using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.DataContext;

namespace RazorConverter.Actions
{
    public static class DataContextHelpers
    {
        private static readonly ProjectFileLocationEqualityComparer ProjectFileLocationComparer = new ProjectFileLocationEqualityComparer();

        public static IEnumerable<IProjectFile> GetSourceFilesToConvert(this IDataContext context)
        {
            var selectedProjectElements = context.GetData(ProjectModelDataConstants.PROJECT_MODEL_ELEMENTS);

            if (selectedProjectElements == null)
            {
                return Array.Empty<IProjectFile>();
            }

            return selectedProjectElements
                .SelectMany(GetSourceFilesToConvert)
                .Distinct(ProjectFileLocationComparer);
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


}
