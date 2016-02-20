namespace OANDARestLibrary.TradeLibrary.DataTypes
{
    public struct Candle
    {
        #region Public Properties

        public double closeAsk { get; set; }

        public double closeBid { get; set; }

        public double closeMid { get; set; }

        public bool complete { get; set; }

        public double highAsk { get; set; }

        // Bid/Ask candles

        public double highBid { get; set; }

        public double highMid { get; set; }

        public double lowAsk { get; set; }

        public double lowBid { get; set; }

        public double lowMid { get; set; }

        public double openAsk { get; set; }

        public double openBid { get; set; }

        public double openMid { get; set; }

        public string time { get; set; }

        public int volume { get; set; }

        #endregion
    }
}