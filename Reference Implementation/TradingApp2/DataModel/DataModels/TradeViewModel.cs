namespace TradeLibrary.DataModels
{
    using System;

    using OANDARestLibrary.TradeLibrary.DataTypes;

    using TradingApp2.Data;

    public class TradeViewModel : DataItem
    {
        #region Fields

        protected TradeData _model;

        #endregion

        #region Constructors and Destructors

        public TradeViewModel(TradeData data, DataGroup group)
            : base(group)
        {
            this._model = data;
        }

        #endregion

        #region Public Properties

        public long Id
        {
            get { return this._model.id; }
        }
        public string Instrument
        {
            get { return this._model.instrument; }
        }
        public double Price
        {
            get { return this._model.price; }
        }
        public string Side
        {
            get { return this._model.side; }
        }
        public double StopLoss
        {
            get { return this._model.stopLoss; }
        }
        public override string Subtitle
        {
            get { return this.Price + " : " + this.TakeProfit + "/" + this.StopLoss; }
            set { throw new NotSupportedException(); }
        }
        public double TakeProfit
        {
            get { return this._model.takeProfit; }
        }
        public string Time
        {
            get { return this._model.time; }
        }
        public override string Title
        {
            get { return this.Side + " " + this.Instrument + " " + this.Units; }
            set { throw new NotSupportedException(); }
        }
        public double TrailingAmount
        {
            get { return this._model.trailingAmount; }
        }
        public int TrailingStop
        {
            get { return this._model.trailingStop; }
        }
        public override string UniqueId
        {
            get { return this.Id.ToString(); }
            set { throw new NotSupportedException(); }
        }
        public int Units
        {
            get { return this._model.units; }
        }

        #endregion
    }
}