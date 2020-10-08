using System;
using System.Diagnostics;
using System.Windows.Controls;
using Stylet;

namespace OCRProj.Views
{
    public partial class ShellViewModel : Screen
    {
        /// <summary>
        ///  选择内容更改
        /// </summary>
        public void SelectionUpdate(object sender, EventArgs e)
        {
            if (sender is ListBoxItem sItem)
            {
                CurSelectedIndex = int.Parse(sItem.Tag.ToString());
                IsSelectedChanged();
            }
            Debug.WriteLine($"CurIndex: {CurSelectedIndex}");
        }

        // 改变节点选择状态
        private void IsSelectedChanged()
        {
            foreach (var fileItem in ParseFileList)
            {
                fileItem.IsSelected = fileItem.Index == CurSelectedIndex;
            }
        }
    }
}