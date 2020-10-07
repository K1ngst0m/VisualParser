
using System.Windows.Media;

namespace OCRProj.Models
{
    public class AnalysisPic: AnalysisObject
    {
        public AnalysisPic(){}
        public AnalysisPic(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; set; }
        public double PicWidth { get; set; } = 0;
        public double PicHeight { get; set; } = 0;
        public double PicResolution { get; set; }
        
        public bool IsOcr { get; set; } = false;
        public bool IsJudge { get; set; } = false;

        public override string Parse()
        {
            var result = "";
            if (IsOcr)
            {
                result = AnalysisMethod.Ocr(this);
            }
            if (IsJudge)
            {
                result = AnalysisMethod.ContentJudge(this);
            }

            return result;
        }

        protected override bool NullAlgorithmCheck() => IsJudge || IsFilter || IsOcr;
    }
}