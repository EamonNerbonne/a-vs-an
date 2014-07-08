namespace AvsAnLib.Internals {
    /// <summary>
    /// The ratio of a's vs. an's for a given prefix
    /// </summary>
    public struct Ratio {
        public int aCount, anCount;
        public bool isSet { get { return (aCount | anCount) != 0; } }
    }
}