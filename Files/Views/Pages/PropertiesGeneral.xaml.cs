using Files.Filesystem;
using Files.View_Models;
using Files.View_Models.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace Files
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class PropertiesGeneral : Page
    {
        public BaseProperties BaseProperties { get; set; }

        public SelectedItemsPropertiesViewModel ViewModel { get; set; }

        public PropertiesGeneral()
        {
            this.InitializeComponent();
        }

        private void Properties_Loaded(object sender, RoutedEventArgs e)
        {
            if (BaseProperties != null)
            {
                BaseProperties.GetSpecialProperties();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = new SelectedItemsPropertiesViewModel();
            var np = e.Parameter as Properties.PropertyNavParam;

            if (np.navParameter is ListedItem)
            {
                var listedItem = np.navParameter as ListedItem;
                if (listedItem.PrimaryItemAttribute == StorageItemTypes.File)
                {
                    BaseProperties = new FileProperties(ViewModel, np.tokenSource, Dispatcher, ItemMD5HashProgress, listedItem);
                }
                else if (listedItem.PrimaryItemAttribute == StorageItemTypes.Folder)
                {
                    BaseProperties = new FolderProperties(ViewModel, np.tokenSource, Dispatcher, listedItem);
                }
            }
            else if (np.navParameter is List<ListedItem>)
            {
                BaseProperties = new CombinedProperties(ViewModel, np.tokenSource, Dispatcher, np.navParameter as List<ListedItem>);
            }
            else if (np.navParameter is DriveItem)
            {
                BaseProperties = new DriveProperties(ViewModel, np.navParameter as DriveItem);
            }

            base.OnNavigatedTo(e);
        }
    }
}