using System.IO;

namespace OCRProj.Models
{
    public class AnalysisText : AnalysisObject
    {
        public int TextLength { get; set; }
        public string TextContent { get; }

        public override string Parse()
        {
            var result = "";
            if(IsFilter) result = AnalysisMethod.Filter(this);
            return result;
        }

        protected override bool NullAlgorithmCheck() => IsFilter;

        public AnalysisText(){}
        public AnalysisText(string filePath)
        {
            TextContent = File.ReadAllText(filePath);
            TextLength = 0;
        }
    }
}