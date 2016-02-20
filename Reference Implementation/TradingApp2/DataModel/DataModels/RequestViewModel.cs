namespace TradingApp2.DataModel.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.Practices.Prism.Commands;

    using TradingApp2.Data;

    public abstract class RequestViewModel : DataItem
    {
        #region Fields

        protected Dictionary<string, string> requestParams;

        private readonly string _name;

        #endregion

        #region Constructors and Destructors

        protected RequestViewModel(string name, DataGroup group)
            : base(group)
        {
            this.MakeRequestCommand = DelegateCommand.FromAsyncHandler(this.MakeRequest, () => true);
            this._name = name;
        }

        #endregion

        #region Public Properties

        public DelegateCommand MakeRequestCommand { get; protected set; }

        public string RequestDetails
        {
            get
            {
                var details = new StringBuilder();
                foreach (var pair in this.requestParams)
                {
                    details.AppendLine(pair.Key + ": " + pair.Value);
                }
                return details.ToString();
            }
            set { throw new NotImplementedException(); }
        }

        public string ResponseDetails { get; set; }

        public override string Subtitle
        {
            get { return ""; }
            set { throw new NotSupportedException(); }
        }
        public override string Title
        {
            get { return this._name; }
            set { throw new NotSupportedException(); }
        }
        public override string UniqueId
        {
            get { return this._name; }
            set { throw new NotSupportedException(); }
        }

        #endregion

        #region Public Methods and Operators

        public abstract Task MakeRequest();

        #endregion

        #region Methods

        protected async Task InternalMakeRequest<T>(Func<Task<T>> func)
        {
            try
            {
                var response = await func();
                this.ResponseDetails = response.ToString();
            }
            catch (Exception ex)
            {
                this.ResponseDetails = ex.Message;
            }

            this.OnPropertyChanged("ResponseDetails");
        }

        #endregion
    }
}