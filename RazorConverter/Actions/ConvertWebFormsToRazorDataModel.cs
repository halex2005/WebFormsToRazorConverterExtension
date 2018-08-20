using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.Util;

namespace RazorConverter.Actions
{
    public class ConvertWebFormsToRazorDataModel
    {
        public ConvertSingleWebFormToRazorDataModel[] FilesToConvert { get; }

        public ConvertWebFormsToRazorDataModel(IEnumerable<IProjectFile> filesToConvert)
        {
            FilesToConvert = filesToConvert
                .Select(file => new ConvertSingleWebFormToRazorDataModel(file))
                .ToArray();
        }
    }

    public class ConvertSingleWebFormToRazorDataModel
    {
        public IProjectFile OriginalFile { get; }

        [CanBeNull]
        public FileSystemPath ConvertedFileLocation { get; set; }

        [CanBeNull]
        public IProjectFile ConvertedFile { get; set; }

        public ConvertSingleWebFormToRazorDataModel(IProjectFile originalFile)
        {
            OriginalFile = originalFile;
        }
    }
}
