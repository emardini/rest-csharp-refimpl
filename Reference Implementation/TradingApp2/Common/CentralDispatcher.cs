namespace TradingApp2.Common
{
    using Windows.UI.Core;

    internal class CentralDispatcher
    {
        #region Public Properties

        public static CoreDispatcher Dispatcher { get; set; }

        #endregion
    }
}