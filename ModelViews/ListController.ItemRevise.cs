using System;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using OCRProj.Models;
using Stylet;

namespace OCRProj.Views
{
    public partial class ShellViewModel : Screen
    {


        /// <summary>
        /// 添加内容
        /// </summary>
        public void AddContent()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Supported files (*.PNG;*.JPG;*.TXT;)|*.PNG;*.JPG;*.TXT|All files (*.*)|*.*",
                Multiselect = true,
                Title = "选择文本或图片"
            };

            if (openFileDialog.ShowDialog() != true) return;

            foreach (var fileName in openFileDialog.FileNames)
            {
                if (IsRedundancy(fileName)) continue;
                ParseFileList.Add(new FileContent(fileName, ParseFileList.Count));
            }
            SetDefaultIndex();
            IsSelectedChanged();
        }

        /// <summary>
        /// 删除内容
        /// </summary>
        public void DeleteContent()
        {
            if (ParseFileList.Count <= 0) return;

            try
            {
                ParseFileList.RemoveAt(CurSelectedIndex);
            }
            catch
            {
                return;
            }
            finally
            {
                Sort();
                IsSelectedChanged();
            }
            SetDefaultIndex();
        }

        // 设置默认节点, 用于节点删除
        private void SetDefaultIndex()
        {
            CurSelectedIndex = ParseFileList.Count - 1;
        }

        // 判断是否有添加过的文件, 用于节点添加
        private bool IsRedundancy(string filePath)
            => ParseFileList.Any(fileItem => NormalizePath(fileItem.FilePath) == NormalizePath(filePath));

        // 标准化文件路径
        private static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                .ToUpperInvariant();
        }

        // 内容索引重新排序, 用于删除节点
        private void Sort()
        {
            for (var i = 0; i < ParseFileList.Count; i++)
            {
                ParseFileList[i].Index = i;
            }
        }
    }
}