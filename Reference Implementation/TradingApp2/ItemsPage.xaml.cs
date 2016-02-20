 // The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace TradingApp2
{
    using System;
    using System.Collections.Generic;

    using Windows.UI.Xaml.Controls;

    using TradingApp2.Common;
    using TradingApp2.Data;
    using TradingApp2.DataModel.DataModels;

    /// <summary>
    ///     A page that displays a collection of item previews.  In the Split Application this page
    ///     is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class ItemsPage : LayoutAwarePage
    {
        #region Constructors and Destructors

        public ItemsPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Populates the page with content passed during navigation.  Any saved state is also
        ///     provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">
        ///     The parameter value passed to
        ///     <see cref="Frame.Navigate(Type, Object)" /> when this page was initially requested.
        /// </param>
        /// <param name="pageState">
        ///     A dictionary of state preserved by this page during an earlier
        ///     session.  This will be null the first time a page is visited.
        /// </param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // Acquire the current data model
            var sampleDataGroups = AccountDataSource.GetGroups((String)navigationParameter);
            this.DefaultViewModel["Items"] = sampleDataGroups;
        }

        /// <summary>
        ///     Invoked when an item is clicked.
        /// </summary>
        /// <param name="sender">
        ///     The GridView (or ListView when the application is snapped)
        ///     displaying the item clicked.
        /// </param>
        /// <param name="e">Event data that describes the item clicked.</param>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var group = ((DataGroup)e.ClickedItem);
            if (group.Items != null && group.Items.Count > 0 && group.Items[0] as RequestViewModel != null)
            {
                this.Frame.Navigate(typeof(InputSplitPage), group.UniqueId);
            }
            else
            {
                this.Frame.Navigate(typeof(SplitPage), group.UniqueId);
            }
        }

        #endregion
    }
}