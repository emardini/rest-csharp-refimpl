namespace TradeLibrary.DataModels
{
    using System;

    using OANDARestLibrary.TradeLibrary.DataTypes;

    using TradingApp2.Data;

    public class PositionViewModel : DataItem
    {
        #region Fields

        protected Position _model;

        #endregion

        #region Constructors and Destructors

        public PositionViewModel(Position data, DataGroup group)
            : base(group)
        {
            this._model = data;
        }

        #endregion

        #region Public Properties

        public double AveragePrice
        {
            get { return this._model.avgPrice; }
        }

        public string Instrument
        {
            get { return this._model.instrument; }
        }
        public string Side
        {
            get { return this._model.side; }
        }
        public override string Subtitle
        {
            get { return this.AveragePrice.ToString(); }
            set { throw new NotSupportedException(); }
        }
        public override string Title
        {
            get { return this.Side + " " + this.Instrument + " " + this.Units; }
            set { throw new NotSupportedException(); }
        }
        public override string UniqueId
        {
            get { return this.Instrument; }
            set { throw new NotSupportedException(); }
        }
        public int Units
        {
            get { return this._model.units; }
        }

        #endregion

        //public string Time { get { return _model.time; } }
        //public double Price { get { return _model.price; } }
        //public double TakeProfit { get { return _model.takeProfit; } }
        //public double StopLoss { get { return _model.stopLoss; } }
        //public long Expiry { get { return _model.expiry; } }
        //public double HighLimit { get { return _model.highLimit; } }
        //public double LowLimit { get { return _model.lowLimit; } }
        //public int TrailingStop { get { return _model.trailingStop; } }
        //public int OcaGroupId { get { return _model.ocaGroupId; } }
    }
}