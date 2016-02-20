namespace TradingApp2.TradeLibrary
{
    using System.Collections.Generic;

    using TradingApp2.DataModel.DataModels;

    public class DataManager
    {
        #region Static Fields

        private static readonly Dictionary<int, AccountData> accounts = new Dictionary<int, AccountData>();

        #endregion

        #region Public Methods and Operators

        public static AccountData GetAccountData(int id)
        {
            if (!accounts.ContainsKey(id))
            {
                accounts.Add(id, new AccountData(id));
            }
            return accounts[id];
        }

        #endregion
    }
}