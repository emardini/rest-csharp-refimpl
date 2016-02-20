 // The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace TradingApp2.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    using Windows.Foundation.Metadata;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;

    using TradingApp2.Common;

    /// <summary>
    ///     Base class for <see cref="SampleDataItem" /> and <see cref="SampleDataGroup" /> that
    ///     defines properties common to both.
    /// </summary>
    [WebHostHidden]
    public abstract class SampleDataCommon : BindableBase
    {
        #region Static Fields

        private static readonly Uri _baseUri = new Uri("ms-appx:///");

        #endregion

        #region Fields

        private string _description = string.Empty;

        private ImageSource _image;

        private String _imagePath;

        private string _subtitle = string.Empty;

        private string _title = string.Empty;

        private string _uniqueId = string.Empty;

        #endregion

        #region Constructors and Destructors

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        #endregion

        #region Public Properties

        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(_baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        #endregion

        #region Public Methods and Operators

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }

        public override string ToString()
        {
            return this.Title;
        }

        #endregion
    }

    /// <summary>
    ///     Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        #region Fields

        private string _content = string.Empty;

        private SampleDataGroup _group;

        #endregion

        #region Constructors and Destructors

        public SampleDataItem(String uniqueId,
            String title,
            String subtitle,
            String imagePath,
            String description,
            String content,
            SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        #endregion

        #region Public Properties

        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }

        #endregion
    }

    /// <summary>
    ///     Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        #region Fields

        private readonly ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();

        private readonly ObservableCollection<SampleDataItem> _topItem = new ObservableCollection<SampleDataItem>();

        #endregion

        #region Constructors and Destructors

        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this.Items.CollectionChanged += this.ItemsCollectionChanged;
        }

        #endregion

        #region Public Properties

        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }

        public ObservableCollection<SampleDataItem> TopItems
        {
            get { return this._topItem; }
        }

        #endregion

        #region Methods

        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        this.TopItems.Insert(e.NewStartingIndex, this.Items[e.NewStartingIndex]);
                        if (this.TopItems.Count > 12)
                        {
                            this.TopItems.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        this.TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        this.TopItems.RemoveAt(e.OldStartingIndex);
                        this.TopItems.Add(this.Items[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        this.TopItems.Insert(e.NewStartingIndex, this.Items[e.NewStartingIndex]);
                        this.TopItems.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        this.TopItems.RemoveAt(e.OldStartingIndex);
                        if (this.Items.Count >= 12)
                        {
                            this.TopItems.Add(this.Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        this.TopItems[e.OldStartingIndex] = this.Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.TopItems.Clear();
                    while (this.TopItems.Count < this.Items.Count && this.TopItems.Count < 12)
                    {
                        this.TopItems.Add(this.Items[this.TopItems.Count]);
                    }
                    break;
            }
        }

        #endregion
    }

    /// <summary>
    ///     Creates a collection of groups and items with hard-coded content.
    ///     SampleDataSource initializes with placeholder data rather than live production
    ///     data so that sample data is provided at both design-time and run-time.
    /// </summary>
    public sealed class SampleDataSource
    {
        #region Static Fields

        private static readonly SampleDataSource _sampleDataSource = new SampleDataSource();

        #endregion

        #region Fields

        private readonly ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();

        #endregion

        #region Constructors and Destructors

        public SampleDataSource()
        {
            var ITEM_CONTENT = String.Format("Item Content: "); //"{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}",
            //"Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat");

            var group1 = new SampleDataGroup("Rates",
                "Rates",
                "Live rates and charts",
                "Assets/icon_rates.png",
                "Group Description: Live rates for tradable pairs.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                "Item Title: 1",
                "Item Subtitle: 1",
                "Assets/LightGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                "Item Title: 2",
                "Item Subtitle: 2",
                "Assets/DarkGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                "Item Title: 3",
                "Item Subtitle: 3",
                "Assets/MediumGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                "Item Title: 4",
                "Item Subtitle: 4",
                "Assets/DarkGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                "Item Title: 5",
                "Item Subtitle: 5",
                "Assets/MediumGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group1));
            this.AllGroups.Add(group1);

            var group2 = new SampleDataGroup("BuySell",
                "Buy/Sell",
                "Place a new Trade",
                "Assets/icon_buysell.png",
                "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group2.Items.Add(new SampleDataItem("Group-2-Item-1",
                "Item Title: 1",
                "Item Subtitle: 1",
                "Assets/DarkGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group2));
            group2.Items.Add(new SampleDataItem("Group-2-Item-2",
                "Item Title: 2",
                "Item Subtitle: 2",
                "Assets/MediumGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group2));
            group2.Items.Add(new SampleDataItem("Group-2-Item-3",
                "Item Title: 3",
                "Item Subtitle: 3",
                "Assets/LightGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group2));
            this.AllGroups.Add(group2);

            var group3 = new SampleDataGroup("New Order",
                "New Order",
                "Place a new Order",
                "Assets/icon_neworder.png",
                "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group3.Items.Add(new SampleDataItem("Group-3-Item-1",
                "Item Title: 1",
                "Item Subtitle: 1",
                "Assets/MediumGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group3));
            group3.Items.Add(new SampleDataItem("Group-3-Item-2",
                "Item Title: 2",
                "Item Subtitle: 2",
                "Assets/LightGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group3));
            group3.Items.Add(new SampleDataItem("Group-3-Item-3",
                "Item Title: 3",
                "Item Subtitle: 3",
                "Assets/DarkGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group3));
            group3.Items.Add(new SampleDataItem("Group-3-Item-4",
                "Item Title: 4",
                "Item Subtitle: 4",
                "Assets/LightGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group3));
            group3.Items.Add(new SampleDataItem("Group-3-Item-5",
                "Item Title: 5",
                "Item Subtitle: 5",
                "Assets/MediumGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group3));
            group3.Items.Add(new SampleDataItem("Group-3-Item-6",
                "Item Title: 6",
                "Item Subtitle: 6",
                "Assets/DarkGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group3));
            group3.Items.Add(new SampleDataItem("Group-3-Item-7",
                "Item Title: 7",
                "Item Subtitle: 7",
                "Assets/MediumGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group3));
            this.AllGroups.Add(group3);

            var group4 = new SampleDataGroup("Trades",
                "Trades",
                "Current Open Trades",
                "Assets/icon_trades.png",
                "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group4.Items.Add(new SampleDataItem("Group-4-Item-1",
                "Item Title: 1",
                "Item Subtitle: 1",
                "Assets/DarkGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group4));
            group4.Items.Add(new SampleDataItem("Group-4-Item-2",
                "Item Title: 2",
                "Item Subtitle: 2",
                "Assets/LightGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group4));
            group4.Items.Add(new SampleDataItem("Group-4-Item-3",
                "Item Title: 3",
                "Item Subtitle: 3",
                "Assets/DarkGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group4));
            group4.Items.Add(new SampleDataItem("Group-4-Item-4",
                "Item Title: 4",
                "Item Subtitle: 4",
                "Assets/LightGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group4));
            group4.Items.Add(new SampleDataItem("Group-4-Item-5",
                "Item Title: 5",
                "Item Subtitle: 5",
                "Assets/MediumGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group4));
            group4.Items.Add(new SampleDataItem("Group-4-Item-6",
                "Item Title: 6",
                "Item Subtitle: 6",
                "Assets/LightGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group4));
            this.AllGroups.Add(group4);

            var group5 = new SampleDataGroup("Orders",
                "Orders",
                "Current Open Orders",
                "Assets/icon_orders.png",
                "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group5.Items.Add(new SampleDataItem("Group-5-Item-1",
                "Item Title: 1",
                "Item Subtitle: 1",
                "Assets/LightGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group5));
            group5.Items.Add(new SampleDataItem("Group-5-Item-2",
                "Item Title: 2",
                "Item Subtitle: 2",
                "Assets/DarkGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group5));
            group5.Items.Add(new SampleDataItem("Group-5-Item-3",
                "Item Title: 3",
                "Item Subtitle: 3",
                "Assets/LightGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group5));
            group5.Items.Add(new SampleDataItem("Group-5-Item-4",
                "Item Title: 4",
                "Item Subtitle: 4",
                "Assets/MediumGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group5));
            this.AllGroups.Add(group5);

            var group6 = new SampleDataGroup("Positions",
                "Positions",
                "Current Open Positions",
                "Assets/icon_positions.png",
                "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group6.Items.Add(new SampleDataItem("Group-6-Item-1",
                "Item Title: 1",
                "Item Subtitle: 1",
                "Assets/LightGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-2",
                "Item Title: 2",
                "Item Subtitle: 2",
                "Assets/DarkGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-3",
                "Item Title: 3",
                "Item Subtitle: 3",
                "Assets/MediumGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-4",
                "Item Title: 4",
                "Item Subtitle: 4",
                "Assets/DarkGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-5",
                "Item Title: 5",
                "Item Subtitle: 5",
                "Assets/LightGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-6",
                "Item Title: 6",
                "Item Subtitle: 6",
                "Assets/MediumGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-7",
                "Item Title: 7",
                "Item Subtitle: 7",
                "Assets/DarkGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group6));
            group6.Items.Add(new SampleDataItem("Group-6-Item-8",
                "Item Title: 8",
                "Item Subtitle: 8",
                "Assets/LightGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group6));
            this.AllGroups.Add(group6);

            var group7 = new SampleDataGroup("Activity",
                "Activity",
                "Recent Account Activity",
                "Assets/icon_activity.png",
                "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group7.Items.Add(new SampleDataItem("Group-7-Item-1",
                "Item Title: 1",
                "Item Subtitle: 1",
                "Assets/LightGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group7));
            group7.Items.Add(new SampleDataItem("Group-7-Item-2",
                "Item Title: 2",
                "Item Subtitle: 2",
                "Assets/DarkGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group7));
            group7.Items.Add(new SampleDataItem("Group-7-Item-3",
                "Item Title: 3",
                "Item Subtitle: 3",
                "Assets/MediumGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group7));
            group7.Items.Add(new SampleDataItem("Group-7-Item-4",
                "Item Title: 4",
                "Item Subtitle: 4",
                "Assets/DarkGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group7));
            group7.Items.Add(new SampleDataItem("Group-7-Item-5",
                "Item Title: 5",
                "Item Subtitle: 5",
                "Assets/LightGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group7));
            group7.Items.Add(new SampleDataItem("Group-7-Item-6",
                "Item Title: 6",
                "Item Subtitle: 6",
                "Assets/MediumGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group7));
            group7.Items.Add(new SampleDataItem("Group-7-Item-7",
                "Item Title: 7",
                "Item Subtitle: 7",
                "Assets/DarkGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group7));
            group7.Items.Add(new SampleDataItem("Group-7-Item-8",
                "Item Title: 8",
                "Item Subtitle: 8",
                "Assets/LightGray.png",
                "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                ITEM_CONTENT,
                group7));
            this.AllGroups.Add(group7);
        }

        #endregion

        #region Public Properties

        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        #endregion

        #region Public Methods and Operators

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where(group => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1)
            {
                return matches.First();
            }
            return null;
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups"))
            {
                throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            }

            return _sampleDataSource.AllGroups;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where(item => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1)
            {
                return matches.First();
            }
            return null;
        }

        #endregion
    }
}