namespace OANDARestLibrary.TradeLibrary
{
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading.Tasks;

    using OANDARestLibrary.TradeLibrary.DataTypes;

    public abstract class StreamSession<T> where T : IHeartbeat
    {
        #region Fields

        protected readonly int _accountId;

        private WebResponse _response;

        private bool _shutdown;

        #endregion

        #region Constructors and Destructors

        protected StreamSession(int accountId)
        {
            this._accountId = accountId;
        }

        #endregion

        #region Delegates

        public delegate void DataHandler(T data);

        #endregion

        #region Public Events

        public event DataHandler DataReceived;

        #endregion

        #region Public Methods and Operators

        public void OnDataReceived(T data)
        {
            var handler = this.DataReceived;
            if (handler != null)
            {
                handler(data);
            }
        }

        public async void StartSession()
        {
            this._shutdown = false;
            this._response = await this.GetSession();

            Task.Run(() =>
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                var reader = new StreamReader(this._response.GetResponseStream());
                while (!this._shutdown)
                {
                    var memStream = new MemoryStream();

                    var line = reader.ReadLine();
                    memStream.Write(Encoding.UTF8.GetBytes(line), 0, Encoding.UTF8.GetByteCount(line));
                    memStream.Position = 0;

                    var data = (T)serializer.ReadObject(memStream);

                    // Don't send heartbeats
                    if (!data.IsHeartbeat())
                    {
                        this.OnDataReceived(data);
                    }
                }
            }
                );
        }

        public void StopSession()
        {
            this._shutdown = true;
        }

        #endregion

        #region Methods

        protected abstract Task<WebResponse> GetSession();

        #endregion
    }
}