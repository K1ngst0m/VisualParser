using OCRProj.Models;
using Stylet;

namespace OCRProj.Views
{
    public partial class ShellViewModel: Screen
    {
        // 节点列表
        public BindableCollection<FileContent> ParseFileList { get; set; } = new BindableCollection<FileContent>();
        // 当前选择节点序号
        private int CurSelectedIndex { get; set; }
        // 当前选择节点
        public FileContent CurSelectedFileContent => ParseFileList.Count != 0 ? ParseFileList[CurSelectedIndex] : null;
    }
}