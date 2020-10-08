using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Stylet;

namespace OCRProj.Views
{
    public partial class ShellViewModel : Screen
    {
        /// <summary>
        /// 开始处理
        /// </summary>
        public void ParsingContent()
        {
            ToggleParsingButton();
            Task.Delay(0100).ContinueWith(_ =>
            {
                GetParsingContent();
                ToggleParsingButton();
            });
            ToggleSnackBar(ParsingResult.Success);
        }

        /// <summary>
        /// 获得处理的结果
        /// </summary>
        private void GetParsingContent()
        {
            ParseResultList.Clear();
            var invalidFileList = (from parsingContent in ParseFileList where !parsingContent.Check() select parsingContent.FileName).ToList();

            if (invalidFileList.Count != 0)
            {
                var result = new StringBuilder("");
                foreach (var invalidFile in invalidFileList) result.Append($" \n\t[{invalidFile}]");
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