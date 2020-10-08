using System;
using System.Text;
using Tesseract;

namespace OCRProj.Models
{
    public static class AnalysisMethod
    {
        /// <summary>
        /// 图像字符识别算法
        /// </summary>
        /// <param name="parseItem">图片对象</param>
        public static string Ocr(AnalysisPic parseItem)
        {
            using var engine = new TesseractEngine("Resources/Algorithms/ocr/tessdata", "chi_sim", EngineMode.Default);
            using var img = Pix.LoadFromFile(parseItem.FilePath);
            using var page = engine.Process(img);
            return page.GetText();
        }

        /// <summary>
        /// 过滤器算法
        /// </summary>
        /// <param name="parseItem">文字对象</param>
        public static string Filter(AnalysisText parseItem)
        {
            var workDir = System.IO.Directory.GetCurrentDirectory();
            const string clpPath = "/Resources/Algorithms/clp/clean_language_project.exe";
            var clpContent = $" '{parseItem.TextContent}'";
            var command = new StringBuilder(workDir).Append(clpPath).Append(clpContent);
            var result = ExecuteCmd(command.ToString());
            result = result
                .Substring(result.IndexOf("#A#", StringComparison.Ordinal))
                .Replace("#A#", "");
            result = result.Remove(result.IndexOf("#B#", StringComparison.Ordinal) - 1);
            return result;
        }

        private static string ExecuteCmd(string command)
        {
            var pro = new System.Diagnostics.Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            //pro.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            pro.Start();
            pro.StandardInput.WriteLine(command);
            pro.StandardInput.WriteLine("exit");
            pro.StandardInput.AutoFlush = true;
            //获取cmd窗口的输出信息
            var output = pro.StandardOutput.ReadToEnd();
            pro.WaitForExit();//等待程序执行完退出进程
            pro.Close();
            return output;
        }

        public static string ContentJudge(AnalysisObject parseItem)
        {
            return "Content Judge Result";
        }
    }
}