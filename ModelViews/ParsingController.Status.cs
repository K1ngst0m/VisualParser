using System;
using OCRProj.Models;
using Stylet;

namespace OCRProj.Views
{
    public partial class ShellViewModel : Screen
    {
        private void ToggleParsingButton() => IsParsing = !IsParsing;

        public void HideSnackBar() => IsActiveSnackBar = false;

        private void ToggleSnackBar(ParsingResult parsingResult)
        {
            IsActiveSnackBar = false;
            UpdateParsingStatus(parsingResult);
            IsActiveSnackBar = true;
        }

        private void UpdateParsingStatus(ParsingResult parsingResult)
        {
            ParsingResultMessage = parsingResult switch
            {
                ParsingResult.Success => "处理成功, 结果在下部的[处理结果]查看",
                ParsingResult.Fail => "处理失败, 请查看日志信息",
                ParsingResult.Cancel => "处理已中断",
                _ => throw new ArgumentOutOfRangeException(nameof(parsingResult), parsingResult, null)
            };
        }
    }
}