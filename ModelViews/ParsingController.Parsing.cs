using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using Stylet;

namespace OCRProj.Views
{
    public partial class ShellViewModel : Screen
    {
        public void ParsingContent()
        {
            ToggleParsingButton();
            Task.Delay(2000).ContinueWith(_ =>
            {
                GetParsingContent();
                ToggleParsingButton();
                ToggleSnackBar(ParsingResult.Success);
            });
        }

        private void GetParsingContent()
        {
            ParseResultList.Clear();
            var invalidFileList = (from parsingContent in ParseFileList where !parsingContent.Check() select parsingContent.FileName).ToList();

            if (invalidFileList.Count != 0)
            {
                var result = new StringBuilder("");
                foreach (var invalidFile in invalidFileList)
                {
                    result.Append($" \n[{invalidFile}]");
                }
                MessageBox.Show($"对于文件:{result}\n 算法必须选择至少一个");
                return;
            }

            foreach (var parsingContent in ParseFileList)
            {
                var analysisResult = parsingContent.Parse();
                ParseResultList.Add(analysisResult);
            }
        }
    }

}