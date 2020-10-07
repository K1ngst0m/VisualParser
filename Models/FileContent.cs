using System;
using System.IO;
using System.Windows;
using Stylet;

namespace OCRProj.Models
{
    /// <summary>
    /// 解析文件类型
    /// </summary>
    public enum ParseFileType
    {
        Text, // 文字
        Pic,  // 图片
        UnSupport,  // 其他不支持的格式
    }
    
    /// <summary>
    /// 文件内容
    /// </summary>
    public class FileContent : PropertyChangedBase
    {
        public int Index { get; set; }
        public bool IsSelected { get; set; }
        public Visibility IsShowText => FileType == ParseFileType.Text ? Visibility.Visible : Visibility.Hidden;
        public Visibility IsShowPic => FileType == ParseFileType.Pic ? Visibility.Visible : Visibility.Hidden;

        public string BorderColor => IsSelected ? "IndianRed" : "{x:Null}";
        private readonly string _fileSize;
        public ParseFileType FileType { get; }

        // 文件属性
        public string FilePath { get; }
        public string FileName { get; }
        public string FileSize => $"{_fileSize}KB";
        public string FileFirstDate { get; }
        public string FileLastDate { get; }
        public string FileInfo
        {
            get
            {
                return FileType switch
                {
                    ParseFileType.Text => $"字数:{_fileSize}",
                    ParseFileType.Pic => $"尺寸:{_fileSize}",
                    ParseFileType.UnSupport => "不支持的格式",
                    _ => "未知错误"
                };
            }
        }
        public string FileTypeShow
        {
            get
            {
                return FileType switch
                {
                    ParseFileType.Text => "文本",
                    ParseFileType.Pic => "图片",
                    ParseFileType.UnSupport => "不支持的格式",
                    _ => "未知错误"
                };
            }
        }

        public string ShowContent =>
            FileType switch
            {
                ParseFileType.Text => File.ReadAllText(FilePath),
                ParseFileType.Pic => FilePath,
                ParseFileType.UnSupport => "Unsupported files",
                _ => throw new ArgumentOutOfRangeException()
            };

        public string FileIcon =>
            FileType switch
            {
                ParseFileType.Text => "TextBoxOutline",
                ParseFileType.Pic => "ImageOutline",
                ParseFileType.UnSupport => "FileQuestionOutline",
                _ => throw new ArgumentOutOfRangeException()
            };

        public bool IsEnableOcr => FileType == ParseFileType.Pic;
        public bool IsEnableJudge => FileType == ParseFileType.Pic;
        
        // 解析类型
        public bool IsFilter
        {
            get
            {
                return FileType switch
                {
                    ParseFileType.Text => TextItem.IsFilter,
                    ParseFileType.Pic => PicItem.IsFilter,
                    _ => false
                };
            }
            set
            {
                switch (FileType)
                {
                    case ParseFileType.Text:
                        TextItem.IsFilter = value;
                        break;
                    case ParseFileType.Pic:
                        PicItem.IsFilter = value;
                        break;
                }
            }
        }

        public bool IsOcr
        {
            get => PicItem.IsOcr;
            set => PicItem.IsOcr = value;
        }

        public bool IsJudge
        {
            get => PicItem.IsJudge;
            set => PicItem.IsJudge = value;
        }



        public AnalysisPic PicItem { get; } = new AnalysisPic();
        public AnalysisText TextItem { get; } = new AnalysisText();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="index">分配节点索引号</param>
        public FileContent(string filePath, int index)
        {
            var fi = new FileInfo(filePath);
            // var fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            FilePath = filePath;
            Index = index;
            FileName = fi.Name;
            _fileSize = fi.Length.ToString();
            FileFirstDate = fi.CreationTime.ToShortDateString();
            FileLastDate = fi.LastWriteTime.ToShortDateString();
            FileType = fi.Extension switch
            {
                ".txt" => ParseFileType.Text,
                ".jpg" => ParseFileType.Pic,
                ".png" => ParseFileType.Pic,
                _ => ParseFileType.UnSupport
            };
            switch (FileType)
            {
                case ParseFileType.Text:
                    TextItem = new AnalysisText();
                    break;
                case ParseFileType.Pic:
                    PicItem = new AnalysisPic();
                    break;
            }
        }

        /// <summary>
        /// 解析内容
        /// </summary>
        /// <returns>返回解析结果</returns>
        public AnalysisResult Parse()
        {
            var result = new AnalysisResult(this);
            switch (FileType)
            {
                case ParseFileType.Text :
                    TextItem.Parse();
                    break;
                case ParseFileType.Pic :
                    PicItem.Parse();
                    break;
                case ParseFileType.UnSupport :
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }

        /// <summary>
        /// 解析前检查内容合法性
        /// </summary>
        /// <returns>true合法, false不合法</returns>
        public bool Check() =>
            FileType switch
            {
                ParseFileType.Text => TextItem.Check(),
                ParseFileType.Pic => PicItem.Check(),
                ParseFileType.UnSupport => false,
                _ => false
            };
    }
}