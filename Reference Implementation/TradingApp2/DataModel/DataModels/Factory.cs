namespace TradingApp2.TradeLibrary.DataModels
{
    using System;
    using System.Collections.Generic;

    using global::TradeLibrary.DataModels;

    using OANDARestLibrary.TradeLibrary.DataTypes;

    using TradingApp2.Data;

    internal class Factory
    {
        #region Public Methods and Operators

        public static List<DataItem> GetDataItems<T>(List<T> items, DataGroup group)
        {
            var list = new List<DataItem>();
            foreach (dynamic item in items)
            {
                list.Add(GetViewModel(item, group));
            }
            return list;
        }

        public static DataItem GetViewModel(TradeData trade, DataGroup group)
        {
            return new TradeViewModel(trade, group);
        }

        public static DataItem GetViewModel(Order order, DataGroup group)
        {
            return new OrderViewModel(order, group);
        }

        public static DataItem GetViewModel(Transaction trans, DataGroup group)
        {
            return new TransactionViewModel(trans, group);
        }

        public static DataItem GetViewModel(Position trans, DataGroup group)
        {
            return new PositionViewModel(trans, group);
        }

        public static DataItem GetViewModel(Price price, DataGroup group)
        {
            return new PriceViewModel(price, group);
        }

        public static DataItem GetViewModel(object obj, DataGroup group)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}