namespace OCRProj.Models
{
    public class AnalysisPic: AnalysisObject
    {
        public double PicWidth { get; set; } = 0;
        public double PicHeight { get; set; } = 0;
        public double PicResolution { get; set; }
        
        public bool IsOcr { get; set; } = false;
        public bool IsJudge { get; set; } = false;

        public override bool Parse()
        {
            if (IsOcr)
            {
                AnalysisMethod.Ocr(this);
            }
            if (IsJudge)
            {
                AnalysisMethod.ContentJudge(this);
            }

            return true;
        }

        protected override bool NullAlgorithmCheck() => IsJudge || IsFilter || IsOcr;
    }
}