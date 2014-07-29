namespace AvsAnLib.Internals {
    /// <summary>
    /// The ratio of a's vs. an's for a given prefix
    /// </summary>
    public struct Ratio {
        public int aCount, anCount;
        public bool isSet { get { return (aCount | anCount) != 0; } }
        public int Quality() {
            long diff = anCount - aCount;
            if (diff == 0)
                return 0;
            long occurence = anCount + aCount;
            return (int)(diff * diff / occurence);

        }
    }
}