using Files.Enums;
using GalaSoft.MvvmLight;
using System;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Files.Filesystem
{
    public class ListedItem : ObservableObject
    {
        public StorageItemTypes PrimaryItemAttribute { get; set; }
        public bool ItemPropertiesInitialized { get; set; } = false;
        public string FolderTooltipText { get; set; }
        public string FolderRelativeId { get; set; }
        public bool LoadFolderGlyph { get; set; }
        public bool ContainsFilesOrFolders { get; set; }
        private bool _LoadFileIcon;

        public Uri FolderIconSource
        {
            get
            {
                return ContainsFilesOrFolders ? new Uri("ms-appx:///Assets/FolderIcon2.svg") : new Uri("ms-appx:///Assets/FolderIcon.svg");
            }
        }

        public Uri FolderIconSourceLarge
        {
            get
            {
                return ContainsFilesOrFolders ? new Uri("ms-appx:///Assets/FolderIcon2Large.svg") : new Uri("ms-appx:///Assets/FolderIconLarge.svg");
            }
        }

        public bool LoadFileIcon
        {
            get => _LoadFileIcon;
            set => Set(ref _LoadFileIcon, value);
        }

        private bool _LoadUnknownTypeGlyph;

        public bool LoadUnknownTypeGlyph
        {
            get => _LoadUnknownTypeGlyph;
            set => Set(ref _LoadUnknownTypeGlyph, value);
        }

        private CloudDriveSyncStatusUI _SyncStatusUI;

        public CloudDriveSyncStatusUI SyncStatusUI
        {
            get => _SyncStatusUI;
            set => Set(ref _SyncStatusUI, value);
        }

        private BitmapImage _FileImage;

        public BitmapImage FileImage
        {
            get => _FileImage;
            set
            {
                if (value != null)
                {
                    Set(ref _FileImage, value);
                }
            }
        }

        private string _ItemPath;

        public string ItemPath
        {
            get => _ItemPath;
            set => Set(ref _ItemPath, value);
        }

        private string _ItemName;

        public string ItemName
        {
            get => _ItemName;
            set => Set(ref _ItemName, value);
        }

        private string _ItemType;

        public string ItemType
        {
            get => _ItemType;
            set
            {
                if (value != null)
                {
                    Set(ref _ItemType, value);
                }
            }
        }

        public string FileExtension { get; set; }
        public string FileSize { get; set; }
        public long FileSizeBytes { get; set; }

        public string ItemDateModified { get; private set; }
        public string ItemDateCreated { get; private set; }
        public string ItemDateAccessed { get; private set; }

        public DateTimeOffset ItemDateModifiedReal
        {
            get => _itemDateModifiedReal;
            set
            {
                ItemDateModified = GetFriendlyDate(value);
                _itemDateModifiedReal = value;
            }
        }

        private DateTimeOffset _itemDateModifiedReal;

        public DateTimeOffset ItemDateCreatedReal
        {
            get => _itemDateCreatedReal;
            set
            {
                ItemDateCreated = GetFriendlyDate(value);
                _itemDateCreatedReal = value;
            }
        }

        private DateTimeOffset _itemDateCreatedReal;

        public DateTimeOffset ItemDateAccessedReal
        {
            get => _itemDateAccessedReal;
            set
            {
                ItemDateAccessed = GetFriendlyDate(value);
                _itemDateAccessedReal = value;
            }
        }

        private DateTimeOffset _itemDateAccessedReal;

        public ListedItem(string folderRelativeId)
        {
            FolderRelativeId = folderRelativeId;
        }

        public static string GetFriendlyDate(DateTimeOffset d)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var elapsed = DateTimeOffset.Now - d;

            string returnformat = Enum.Parse<TimeStyle>(localSettings.Values[LocalSettings.DateTimeFormat].ToString()) == TimeStyle.Application ? "D" : "g";

            if (elapsed.TotalDays > 7)
            {
                return d.ToString(returnformat);
            }
            else if (elapsed.TotalDays > 2)
            {
                return string.Format(ResourceController.GetTranslation("DaysAgo"), elapsed.Days);
            }
            else if (elapsed.TotalDays > 1)
            {
                return string.Format(ResourceController.GetTranslation("DayAgo"), elapsed.Days);
            }
            else if (elapsed.TotalHours > 2)
            {
                return string.Format(ResourceController.GetTranslation("HoursAgo"), elapsed.Hours);
            }
            else if (elapsed.TotalHours > 1)
            {
                return string.Format(ResourceController.GetTranslation("HourAgo"), elapsed.Hours);
            }
            else if (elapsed.TotalMinutes > 2)
            {
                return string.Format(ResourceController.GetTranslation("MinutesAgo"), elapsed.Minutes);
            }
            else if (elapsed.TotalMinutes > 1)
            {
                return string.Format(ResourceController.GetTranslation("MinuteAgo"), elapsed.Minutes);
            }
            else
            {
                return string.Format(ResourceController.GetTranslation("SecondsAgo"), elapsed.Seconds);
            }
        }

        public bool IsRecycleBinItem => this is RecycleBinItem;
        public bool IsShortcutItem => this is ShortcutItem;
        public bool IsLinkItem => IsShortcutItem && ((ShortcutItem)this).IsUrl;
    }

    public class RecycleBinItem : ListedItem
    {
        public RecycleBinItem(string folderRelativeId) : base(folderRelativeId)
        {
        }

        // For recycle bin elements (path + name)
        public string ItemOriginalPath { get; set; }
    }

    public class ShortcutItem : ListedItem
    {
        public ShortcutItem(string folderRelativeId) : base(folderRelativeId)
        {
        }

        // For shortcut elements (.lnk and .url)
        public string TargetPath { get; set; }

        public string Arguments { get; set; }
        public string WorkingDirectory { get; set; }
        public bool RunAsAdmin { get; set; }
        public bool IsUrl { get; set; }
    }
}