namespace AvsAnLib.Internals {
    /// <summary>
    /// The ratio of a's vs. an's for a given prefix
    /// </summary>
    public struct Ratio {
        public int Occurrence, AminAnDiff;

        public int aCount {
            get => (Occurrence + AminAnDiff) / 2;
            set {
                var old_anCount = anCount;
                Occurrence = value + old_anCount;
                AminAnDiff = value - old_anCount;
            }
        }

        public int anCount {
            get => (Occurrence - AminAnDiff) / 2;
            set {
                var old_aCount = aCount;
                Occurrence = old_aCount + value;
                AminAnDiff = old_aCount - value;
            }
        }

        public void IncA() {
            Occurrence++;
            AminAnDiff++;
        }

        public void IncAn() {
            Occurrence++;
            AminAnDiff--;
        }

        public bool isSet => Occurrence != 0;

        public int Quality() {
            if (AminAnDiff == 0) {
                return 0;
            }

            return (int)(AminAnDiff * (long)AminAnDiff / Occurrence);
        }
    }
}
