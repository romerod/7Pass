﻿using System;
using System.Xml;
using KeePass.Data;
using KeePass.Sources.WebDav.Api;
using KeePass.Utils;

namespace KeePass.Sources.WebDav
{
    internal class MetaListItemInfo : ListItemInfo
    {
        private readonly ItemInfo _item;
        private readonly string _path;

        public bool IsDir
        {
            get { return _item.IsDir; }
        }

        public string Modified
        {
            get { return _item.Modified; }
        }

        public string Path
        {
            get { return _path; }
        }

        public MetaListItemInfo(string basePath, ItemInfo item)
        {
            if (basePath == null) throw new ArgumentNullException("basePath");
            if (item == null) throw new ArgumentNullException("item");

            _item = item;
            _path = new Uri(new Uri(basePath),
                item.Path).ToString();

            Title = GetTitle(item);
            Notes = GetRelativeTime(item);
            Icon = ThemeData.GetImage(item.IsDir
                ? "folder" : "entry");
        }

        private static string GetRelativeTime(ItemInfo item)
        {
            try
            {
                return XmlConvert.ToDateTime(item.Modified,
                    XmlDateTimeSerializationMode.RoundtripKind)
                    .ToRelative();
            }
            catch (XmlException)
            {
                return string.Empty;
            }
        }

        private static string GetTitle(ItemInfo item)
        {
            try
            {
                return System.IO.Path
                    .GetFileName(item.Path);
            }
            catch (ArgumentException)
            {
                return item.Path;
            }
        }
    }
}