namespace TradingApp2.DataModel.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Windows.UI.Xaml;

    using Framework;

    using OANDARestLibrary;
    using OANDARestLibrary.Framework;
    using OANDARestLibrary.TradeLibrary.DataTypes;

    public class AccountData : ObservableObject
    {
        #region Fields

        private long _currentTrans;

        private ObservableCollection<Order> _orders;

        private ObservableCollection<Position> _positions;

        private ObservableCollection<TradeData> _trades;

        private DispatcherTimer _transTimer;

        private ObservableCollection<Transaction> _transactions;

        #endregion

        #region Constructors and Destructors

        public AccountData(int id)
        {
            this.Id = id;
        }

        #endregion

        #region Public Events

        public event EventHandler<CustomEventArgs<Transaction>> NewTransaction;

        #endregion

        #region Public Properties

        public AccountDetails Details { get; set; }

        public int Id { get; private set; }

        public ObservableCollection<Order> Orders
        {
            get { return this._orders; }
            set
            {
                this._orders = value;
                this.RaisePropertyChanged("Orders");
            }
        }
        public ObservableCollection<Position> Positions
        {
            get { return this._positions; }
            set
            {
                this._positions = value;
                this.RaisePropertyChanged("Positions");
            }
        }
        public ObservableCollection<TradeData> Trades
        {
            get { return this._trades; }
            set
            {
                this._trades = value;
                this.RaisePropertyChanged("Trades");
            }
        }
        public ObservableCollection<Transaction> Transactions
        {
            get { return this._transactions; }
            set
            {
                this._transactions = value;
                this.RaisePropertyChanged("Transactions");
            }
        }

        #endregion

        #region Public Methods and Operators

        public void EnableUpdates()
        {
            if (this._transTimer == null)
            {
                this._transTimer = new DispatcherTimer();
                this._transTimer.Tick += this.Refresh;
                this._transTimer.Interval = new TimeSpan(0, 0, 0, 1);
                this._transTimer.Start();
            }
        }

        public ObservableCollection<T> GetObservable<T>(List<T> list)
        {
            var collection = new ObservableCollection<T>();
            foreach (var item in list)
            {
                collection.Add(item);
            }
            return collection;
        }

        public void OnNewTransaction(Transaction e)
        {
            var handler = this.NewTransaction;
            if (handler != null)
            {
                handler(this, new CustomEventArgs<Transaction>(e));
            }
        }

        public async void Refresh()
        {
            var transParams = new Dictionary<string, string> { { "minId", "" + (this._currentTrans + 1) } };
            var newTransactions = await Rest.GetTransactionListAsync(this.Id, transParams);
            if (newTransactions.Count > 0)
            {
                this._currentTrans = newTransactions[0].id;
                foreach (var newTransaction in newTransactions)
                {
                    this.OnNewTransaction(newTransaction);
                }

                // these can't change unless there's been a transaction...
                this.Trades = this.GetObservable(await Rest.GetTradeListAsync(this.Id));
                this.Orders = this.GetObservable(await Rest.GetOrderListAsync(this.Id));
                this.Transactions = this.GetObservable(await Rest.GetTransactionListAsync(this.Id));
                this.Positions = this.GetObservable(await Rest.GetPositionsAsync(this.Id));
            }
        }

        #endregion

        #region Methods

        private void Refresh(object sender, object e)
        {
            this.Refresh();
        }

        #endregion
    }
}