namespace TradeLibrary.DataModels
{
    using System.Collections.Generic;

    using OANDARestLibrary.TradeLibrary.DataTypes;

    public class UserData
    {
        #region Public Properties

        public List<Account> Accounts { get; private set; }

        #endregion

        //public AccountData SelectedAccount { get; }
    }
}