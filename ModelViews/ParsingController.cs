using OCRProj.Models;
using Stylet;

namespace OCRProj.Views
{
    public enum ParsingResult
    {
        Success,
        Fail,
        Cancel,
    }
    public partial class ShellViewModel : Screen
    {
        public BindableCollection<AnalysisResult> ParseResultList { get; set; } = new BindableCollection<AnalysisResult>();
        public bool IsParsing { get; set; }
        public string ParsingButtonStatus => IsParsing ? "处理中 ... 点击取消" : "开始处理";

        public bool IsActiveSnackBar { get; set; } = false;
        public string ParsingResultMessage { get; set; } 
        
    }
}
