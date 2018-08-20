using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Text;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.ReSharper.Refactorings.Workflow;
using JetBrains.Util;
using Telerik.RazorConverter;
using Telerik.RazorConverter.Razor.DOM;

namespace RazorConverter.Actions
{
    public class ConvertWebFormsToRazorRefactoringExecutor
        : DrivenRefactoring<ConvertWebFormsToRazorRefactoringWorkflow, ConvertWebFormsToRazorRefactoringBase>
    {
        [Import] private IWebFormsParser Parser { get; set; }
        [Import] private IWebFormsConverter<IRazorNode> Converter { get; set; }
        [Import] private IRenderer<IRazorNode> Renderer { get; set; }

        public ConvertWebFormsToRazorRefactoringExecutor(ConvertWebFormsToRazorRefactoringWorkflow workflow, IRefactoringDriver driver)
            : base(workflow, workflow.Solution, driver)
        {
        }

        public override bool Execute(IProgressIndicator pi)
        {
            var catalog = new AssemblyCatalog(typeof(IWebFormsParser).Assembly);
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);

            pi.Start(Workflow.DataModel.FilesToConvert.Length);
            try
            {
                foreach (var file in Workflow.DataModel.FilesToConvert)
                {
                    using (new SubProgressIndicator(pi, 1))
                    {
                        ConvertWebFormFileToRazor(file);
                    }
                }
            }
            finally
            {
                pi.Stop();
            }

            return true;
        }

        private void ConvertWebFormFileToRazor(ConvertSingleWebFormToRazorDataModel file)
        {
            var webFormsPageSource = File.ReadAllText(file.OriginalFile.Location.FullPath, Encoding.UTF8);
            var webFormsDocument = Parser.Parse(webFormsPageSource);
            var razorDom = Converter.Convert(webFormsDocument);
            var razorPage = Renderer.Render(razorDom);

            var outputFile = Path.Combine(
                file.OriginalFile.Location.Directory.FullPath,
                file.OriginalFile.Location.NameWithoutExtension + ".cshtml");
            File.WriteAllText(outputFile, razorPage, Encoding.UTF8);

            file.ConvertedFileLocation = FileSystemPath.TryParse(outputFile);
        }
    }
}
