using System;
using Stylet;

namespace OCRProj.Models
{
    public class AnalysisResult : PropertyChangedBase
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ParseAlgorithm { get; set; }
        private ParseFileType FileType { get; }

        public string FileIcon =>
            FileType switch
            {
                ParseFileType.Pic => "ImageOutline",
                ParseFileType.Text => "TextBoxOutline",
                ParseFileType.UnSupport => "FileQuestionOutline",
                _ => throw new ArgumentOutOfRangeException()
            };

        public string ResultContent { get; set; } = "请添加支持的格式处理";

        public AnalysisResult(FileContent fileContent)
        {
            FileType = fileContent.FileType;
            FileName = fileContent.FileName;
            FilePath = fileContent.FilePath;

            ParseAlgorithm = fileContent.FileType switch
            {
                ParseFileType.Text => GetAlgorithmType(fileContent.TextItem),
                ParseFileType.Pic => GetAlgorithmType(fileContent.PicItem),
                ParseFileType.UnSupport => "算法不支持的文件格式",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static string GetAlgorithmType(AnalysisPic analysisPic)
        {
            var algorithmType = "";
            if (analysisPic.IsFilter)
            {
                algorithmType += "文字敏感内容判断";
                if (analysisPic.IsOcr) algorithmType += " | OCR图像识别";
                if (analysisPic.IsJudge) algorithmType += " | 图像敏感内容判断";
            }
            else
            {
                if (analysisPic.IsOcr)
                {
                    algorithmType += " OCR图像识别";
                    if (analysisPic.IsJudge) algorithmType += " | 图像敏感内容判断";
                }
                else
                {
                    if (analysisPic.IsJudge) algorithmType += "图像敏感内容判断";
                }
            }

            return algorithmType;
        }

        private static string GetAlgorithmType(AnalysisObject analysisText)
        {
            return analysisText.IsFilter ? "文字敏感内容判断": "";
        }
    }
}