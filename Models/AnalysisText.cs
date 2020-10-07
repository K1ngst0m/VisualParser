namespace OCRProj.Models
{
    public class AnalysisText : AnalysisObject
    {
        public int TextLenght { get; set; }

        public override bool Parse()
        {
            if(IsFilter) AnalysisMethod.Filter(this);
            return true;
        }

        protected override bool NullAlgorithmCheck() => IsFilter;
    }
}