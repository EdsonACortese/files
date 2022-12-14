using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Files.Dialogs
{
    public sealed partial class ExtractFilesDialog : ContentDialog
    {
        public ExtractFilesDialog(string currentDirectory)
        {
            this.InitializeComponent();
            DestPathText.Text = currentDirectory;
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["Extract_Destination_Path"] = currentDirectory;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["Extract_Destination_Cancelled"] = false;
        }

        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.Pickers.FolderPicker folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.CommitButtonText = "Select Folder";
            folderPicker.FileTypeFilter.Add("*");
            var selectedFolder = await folderPicker.PickSingleFolderAsync();
            if (selectedFolder != null)
            {
                DestPathText.Text = selectedFolder.Path;
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["Extract_Destination_Path"] = selectedFolder.Path;
            }
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["Extract_Destination_Cancelled"] = true;
        }
    }
}