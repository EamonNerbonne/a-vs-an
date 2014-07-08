namespace AvsAnLib.Internals {
    public struct Ratio {
        public int aCount, anCount;
        public bool isSet { get { return (aCount | anCount) != 0; } }
    }
}