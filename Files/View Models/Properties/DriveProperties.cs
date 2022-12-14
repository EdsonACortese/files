using Files.Filesystem;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Files.View_Models.Properties
{
    internal class DriveProperties : BaseProperties
    {
        public DriveItem Drive { get; }

        public DriveProperties(SelectedItemsPropertiesViewModel viewModel, DriveItem driveItem)
        {
            ViewModel = viewModel;
            Drive = driveItem;

            GetBaseProperties();
        }

        public override void GetBaseProperties()
        {
            if (Drive != null)
            {
                ViewModel.DriveItemGlyphSource = Drive.Glyph;
                ViewModel.LoadDriveItemGlyph = true;
                ViewModel.ItemName = Drive.Text;
                ViewModel.ItemType = Drive.Type.ToString();
            }
        }

        public override void GetSpecialProperties()
        {
            ViewModel.ItemAttributesVisibility = Visibility.Collapsed;
            StorageFolder diskRoot = Task.Run(async () => await ItemViewModel.GetFolderFromPathAsync(Drive.Path)).Result;

            string freeSpace = "System.FreeSpace";
            string capacity = "System.Capacity";
            string fileSystem = "System.Volume.FileSystem";

            try
            {
                var properties = Task.Run(async () =>
                {
                    return await diskRoot.Properties.RetrievePropertiesAsync(new[] {
                    freeSpace,
                    capacity,
                    fileSystem });
                }).Result;

                ViewModel.DriveCapacityValue = (ulong)properties[capacity];
                ViewModel.DriveFreeSpaceValue = (ulong)properties[freeSpace];
                ViewModel.DriveUsedSpaceValue = ViewModel.DriveCapacityValue - ViewModel.DriveFreeSpaceValue;
                ViewModel.DriveFileSystem = (string)properties[fileSystem];
            }
            catch (Exception e)
            {
                ViewModel.LastSeparatorVisibility = Visibility.Collapsed;
                NLog.LogManager.GetCurrentClassLogger().Error(e, e.Message);
            }
        }
    }
}