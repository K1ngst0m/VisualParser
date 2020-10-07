namespace OCRProj.Models
{
    public abstract class AnalysisObject
    {
        // 索引
        public int Index { get; set; } = 0;
        public int Type = 0;
        public bool IsFilter { get; set; } = false;

        public abstract string Parse();

        public virtual bool Check()
        {
            if (!NullAlgorithmCheck()) return false;
            
            return true;
        }

        protected abstract bool NullAlgorithmCheck();
    }
}