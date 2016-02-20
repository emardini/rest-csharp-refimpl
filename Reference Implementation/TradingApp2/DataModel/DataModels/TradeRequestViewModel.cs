namespace TradeLibrary.DataModels
{
    internal class TradeRequestViewModel
    {
        #region Public Properties

        public EDirection Direction { get; set; }

        // value

        public double HighPrice { get; set; }

        public string Instrument { get; set; }

        public double LowPrice { get; set; }

        public double Price { get; set; }

        public double StopLoss { get; set; }

        public double TakeProfit { get; set; }

        // in pippettes
        public int TrailingStop { get; set; }

        public EOrderType Type { get; set; }

        public int Units { get; set; }

        #endregion
    }
}