namespace TradingApp2.TradeLibrary.DataModels
{
    using System;
    using System.Text;

    using OANDARestLibrary.TradeLibrary.DataTypes;

    using TradingApp2.Data;

    public class OrderViewModel : DataItem
    {
        #region Fields

        protected Order _model;

        #endregion

        #region Constructors and Destructors

        public OrderViewModel(Order data, DataGroup group)
            : base(group)
        {
            this._model = data;
        }

        #endregion

        #region Public Properties

        public override string Content
        {
            get
            {
                var result = new StringBuilder();
                result.AppendLine("Id : " + this.Id);
                result.AppendLine("Type : " + this.Type);
                result.AppendLine("Side : " + this.Side);
                result.AppendLine("Instrument : " + this.Instrument);
                result.AppendLine("Units : " + this.Units);
                result.AppendLine("Time : " + this.Time);
                result.AppendLine("Price : " + this.Price);
                result.AppendLine("TakeProfit : " + this.TakeProfit);
                result.AppendLine("StopLoss : " + this.StopLoss);
                result.AppendLine("Expiry : " + this.Expiry);
                result.AppendLine("UpperBound : " + this.UpperBound);
                result.AppendLine("LowerBound : " + this.LowerBound);
                result.AppendLine("TrailingStop : " + this.TrailingStop);

                return result.ToString();
            }
            set { base.Content = value; }
        }
        public string Expiry
        {
            get { return this._model.expiry; }
        }

        public long Id
        {
            get { return this._model.id; }
        }
        public string Instrument
        {
            get { return this._model.instrument; }
        }
        public double LowerBound
        {
            get { return this._model.lowerBound; }
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
        public int TrailingStop
        {
            get { return this._model.trailingStop; }
        }
        public string Type
        {
            get { return this._model.type; }
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
        public double UpperBound
        {
            get { return this._model.upperBound; }
        }

        #endregion
    }
}