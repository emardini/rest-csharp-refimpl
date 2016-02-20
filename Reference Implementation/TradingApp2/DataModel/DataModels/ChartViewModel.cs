namespace TradingApp2.TradeLibrary.DataModels
{
    using System;

    using TradingApp2.Common;
    using TradingApp2.DataModel;

    public class ChartViewModel : BindableBase
    {
        #region Constructors and Destructors

        public ChartViewModel(String instrument)
        {
            this.Instrument = instrument;
            // Note: we currently don't support changing the period
            this.Period = "H1";
        }

        #endregion

        #region Public Properties

        public CandleData Candles
        {
            get { return RatesDataSource.GetCandles(this.Instrument, this.Period); }
        }

        public string Instrument { get; private set; }

        public string Period { get; private set; }

        #endregion
    }
}