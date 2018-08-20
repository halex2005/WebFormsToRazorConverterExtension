using JetBrains.Application.Progress;
using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.ReSharper.Refactorings.Workflow;
using JetBrains.Util;

namespace RazorConverter.Actions
{
    public class ConvertWebFormsToRazorRefactoringExecutor
        : DrivenRefactoring<ConvertWebFormsToRazorRefactoringWorkflow, ConvertWebFormsToRazorRefactoringBase>
    {
        public ConvertWebFormsToRazorRefactoringExecutor(ConvertWebFormsToRazorRefactoringWorkflow workflow, IRefactoringDriver driver)
            : base(workflow, workflow.Solution, driver)
        {
        }

        public override bool Execute(IProgressIndicator pi)
        {
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
            MessageBox.ShowInfo($"Will convert file {file.OriginalFile.Location.FullPath}");
            
            
//            var webFormsPageSource = File.ReadAllText(file, Encoding.UTF8);
//            var webFormsDocument = Parser.Parse(webFormsPageSource);
//            var razorDom = Converter.Convert(webFormsDocument);
//            var razorPage = Renderer.Render(razorDom);
//
//            var outputFile = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(file) + ".cshtml");
//            File.WriteAllText(outputFile, razorPage, Encoding.UTF8);

        }
    }
}
