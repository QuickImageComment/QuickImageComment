// GongSolutions.Shell - A Windows Shell library for .Net.
// Copyright (C) 2007-2009 Steven J. Kirk
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either 
// version 2 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public 
// License along with this program; if not, write to the Free 
// Software Foundation, Inc., 51 Franklin Street, Fifth Floor,  
// Boston, MA 2110-1301, USA.
//

// for performance test
//#define PERFORMANCETEST

using GongSolutions.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace GongSolutions.Shell
{
    /// <summary>
    /// Represents an item in the Windows Shell namespace.
    /// </summary>
    [TypeConverter(typeof(ShellItemConverter))]
    public class ShellItem : IEnumerable<ShellItem>
    {
#if PERFORMANCETEST
        DateTime StartTime;
        DateTime CurrentTime;
        public double firstCall;
        public double lastCall;
        public double sumAllCalls;
        public int countAllCalls;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellItem"/> class.
        /// </summary>
        /// 
        /// <remarks>
        /// Takes a <see cref="Uri"/> containing the location of the ShellItem. 
        /// This constructor accepts URIs using two schemes:
        /// 
        /// - file: A file or folder in the computer's filesystem, e.g.
        ///         file:///D:/Folder
        /// - shell: A virtual folder, or a file or folder referenced from 
        ///          a virtual folder, e.g. shell:///Personal/file.txt
        /// </remarks>
        /// 
        /// <param name="uri">
        /// A <see cref="Uri"/> containing the location of the ShellItem.
        /// </param>
        public ShellItem(Uri uri)
        {
            Initialize(uri);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellItem"/> class.
        /// </summary>
        /// 
        /// <remarks>
        /// Takes a <see cref="string"/> containing the location of the ShellItem. 
        /// This constructor accepts URIs using two schemes:
        /// 
        /// - file: A file or folder in the computer's filesystem, e.g.
        ///         file:///D:/Folder
        /// - shell: A virtual folder, or a file or folder referenced from 
        ///          a virtual folder, e.g. shell:///Personal/file.txt
        /// </remarks>
        /// 
        /// <param name="path">
        /// A string containing a Uri with the location of the ShellItem.
        /// </param>
        public ShellItem(string path)
        {
            Initialize(new Uri(path));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellItem"/> class.
        /// </summary>
        /// 
        /// <remarks>
        /// Takes an <see cref="Environment.SpecialFolder"/> containing the 
        /// location of the folder.
        /// </remarks>
        /// 
        /// <param name="folder">
        /// An <see cref="Environment.SpecialFolder"/> containing the 
        /// location of the folder.
        /// </param>
        public ShellItem(Environment.SpecialFolder folder)
        {
            IntPtr pidl;

            if (Shell32.SHGetSpecialFolderLocation(IntPtr.Zero,
                (CSIDL)folder, out pidl) == HResult.S_OK)
            {
                try
                {
                    m_ComInterface = CreateItemFromIDList(pidl);
                }
                finally
                {
                    Shell32.ILFree(pidl);
                }
            }
            else
            {
                // SHGetSpecialFolderLocation does not support many common
                // CSIDL values on Windows 98, but SHGetFolderPath in 
                // ShFolder.dll does, so fall back to it if necessary. We
                // try SHGetSpecialFolderLocation first because it returns
                // a PIDL which is preferable to a path as it can express
                // virtual folder locations.
                StringBuilder path = new StringBuilder();
                Marshal.ThrowExceptionForHR((int)Shell32.SHGetFolderPath(
                    IntPtr.Zero, (CSIDL)folder, IntPtr.Zero, 0, path));
                m_ComInterface = CreateItemFromParsingName(path.ToString());
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellItem"/> class.
        /// </summary>
        /// 
        /// <remarks>
        /// Creates a ShellItem which is a named child of <paramref name="parent"/>.
        /// </remarks>
        /// 
        /// <param name="parent">
        /// The parent folder of the item.
        /// </param>
        /// 
        /// <param name="name">
        /// The name of the child item.
        /// </param>
        public ShellItem(ShellItem parent, string name)
        {
            if (parent.IsFileSystem)
            {
                // If the parent folder is in the file system, our best 
                // chance of success is to use the FileSystemPath to 
                // create the new item. Folders other than Desktop don't 
                // seem to implement ParseDisplayName properly.
                m_ComInterface = CreateItemFromParsingName(
                    Path.Combine(parent.FileSystemPath, name));
            }
            else
            {
                IShellFolder folder = parent.GetIShellFolder();
                uint eaten;
                IntPtr pidl;
                uint attributes = 0;

                folder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero,
                    name, out eaten, out pidl, ref attributes);

                try
                {
                    m_ComInterface = CreateItemFromIDList(pidl);
                }
                finally
                {
                    Shell32.ILFree(pidl);
                }
            }
        }

        public ShellItem(IntPtr pidl)
        {
            m_ComInterface = CreateItemFromIDList(pidl);
        }

        public ShellItem(ShellItem parent, IntPtr pidl)
        {
            m_ComInterface = CreateItemWithParent(parent, pidl);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ShellItem"/> class.
        /// </summary>
        /// 
        /// <param name="comInterface">
        /// An <see cref="IShellItem"/> representing the folder.
        /// </param>
        public ShellItem(IShellItem comInterface)
        {
            m_ComInterface = comInterface;
        }

        //QIC /// <summary>
        //QIC /// Compares two <see cref="IShellItem"/>s. The comparison is carried
        //QIC /// out by display order.
        //QIC /// </summary>
        //QIC /// 
        //QIC /// <param name="item">
        //QIC /// The item to compare.
        //QIC /// </param>
        //QIC /// 
        //QIC /// <returns>
        //QIC /// 0 if the two items are equal. A negative number if 
        //QIC /// <see langword="this"/> is before <paramref name="item"/> in 
        //QIC /// display order. A positive number if 
        //QIC /// <see langword="this"/> comes after <paramref name="item"/> in 
        //QIC /// display order.
        //QIC /// </returns>
        //QIC public int Compare(ShellItem item)
        //QIC {
        //QIC     int result = m_ComInterface.Compare(item.ComInterface,
        //QIC         SICHINT.DISPLAY);
        //QIC     return result;
        //QIC }

        /// <summary>
        /// Determines whether two <see cref="ShellItem"/>s refer to
        /// the same shell folder.
        /// </summary>
        /// 
        /// <param name="obj">
        /// The item to compare.
        /// </param>
        /// 
        /// <returns>
        /// <see langword="true"/> if the two objects refer to the same
        /// folder, <see langword="false"/> otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is ShellItem)
            {
                ShellItem otherItem = (ShellItem)obj;
                bool result = m_ComInterface.Compare(otherItem.ComInterface,
                    SICHINT.DISPLAY) == 0;

                // Sometimes, folders are reported as being unequal even when
                // they refer to the same folder, so double check by comparing
                // the file system paths. (This was showing up on Windows XP in 
                // the SpecialFolders() test)
                if (!result)
                {
                    result = IsFileSystem && otherItem.IsFileSystem &&
                        (FileSystemPath == otherItem.FileSystemPath);
                }

                return result;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the name of the item in the specified style.
        /// </summary>
        /// 
        /// <param name="sigdn">
        /// The style of display name to return.
        /// </param>
        /// 
        /// <returns>
        /// A string containing the display name of the item.
        /// </returns>
        public string GetDisplayName(SIGDN sigdn)
        {
            IntPtr resultPtr = m_ComInterface.GetDisplayName(sigdn);
            string result = Marshal.PtrToStringUni(resultPtr);
            Marshal.FreeCoTaskMem(resultPtr);
            return result;
        }

        /// <summary>
        /// Returns an enumerator detailing the child items of the
        /// <see cref="ShellItem"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// This method returns all child item including hidden
        /// items.
        /// </remarks>
        /// 
        /// <returns>
        /// An enumerator over all child items.
        /// </returns>
        public IEnumerator<ShellItem> GetEnumerator()
        {
            return GetEnumerator(SHCONTF.FOLDERS | SHCONTF.INCLUDEHIDDEN |
                SHCONTF.NONFOLDERS);
        }

        /// <summary>
        /// Returns an enumerator detailing the child items of the
        /// <see cref="ShellItem"/>.
        /// </summary>
        /// 
        /// <param name="filter">
        /// A filter describing the types of child items to be included.
        /// </param>
        /// 
        /// <returns>
        /// An enumerator over all child items.
        /// </returns>
        public IEnumerator<ShellItem> GetEnumerator(SHCONTF filter)
        {
            IShellFolder folder = GetIShellFolder();
            IEnumIDList enumId = GetIEnumIDList(folder, filter);
            uint count;
            IntPtr pidl;
            HResult result;

            if (enumId == null)
            {
                yield break;
            }

#if PERFORMANCETEST
            StartTime = DateTime.Now;
#endif

            result = enumId.Next(1, out pidl, out count);

#if PERFORMANCETEST
            CurrentTime = DateTime.Now;
            firstCall = CurrentTime.Subtract(StartTime).TotalMilliseconds;
            sumAllCalls = firstCall;
            countAllCalls = 1;
#endif

            while (result == HResult.S_OK)
            {
                yield return new ShellItem(this, pidl);
                Shell32.ILFree(pidl);

#if PERFORMANCETEST
                StartTime = DateTime.Now;
#endif
                result = enumId.Next(1, out pidl, out count);
#if PERFORMANCETEST
                CurrentTime = DateTime.Now;
                lastCall = CurrentTime.Subtract(StartTime).TotalMilliseconds;
                sumAllCalls = sumAllCalls + lastCall;
                countAllCalls++;
#endif
            }

            if (result != HResult.S_FALSE)
            {
                Marshal.ThrowExceptionForHR((int)result);
            }

            yield break;
        }

        /// <summary>
        /// Returns an enumerator detailing the child items of the
        /// <see cref="ShellItem"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// This method returns all child item including hidden
        /// items.
        /// </remarks>
        /// 
        /// <returns>
        /// An enumerator over all child items.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //QIC /// <summary>
        //QIC /// Returns an <see cref="ComTypes.IDataObject"/> representing the
        //QIC /// item. This object is used in drag and drop operations.
        //QIC /// </summary>
        //QIC public ComTypes.IDataObject GetIDataObject()
        //QIC {
        //QIC     IntPtr result = m_ComInterface.BindToHandler(IntPtr.Zero,
        //QIC         BHID.SFUIObject, typeof(ComTypes.IDataObject).GUID);
        //QIC     return (ComTypes.IDataObject)Marshal.GetTypedObjectForIUnknown(result,
        //QIC         typeof(ComTypes.IDataObject));
        //QIC }

        //QIC /// <summary>
        //QIC /// Returns an <see cref="IDropTarget"/> representing the
        //QIC /// item. This object is used in drag and drop operations.
        //QIC /// </summary>
        //QIC public IDropTarget GetIDropTarget(System.Windows.Forms.Control control)
        //QIC {
        //QIC     IntPtr result = GetIShellFolder().CreateViewObject(control.Handle,
        //QIC         typeof(IDropTarget).GUID);
        //QIC     return (IDropTarget)Marshal.GetTypedObjectForIUnknown(result,
        //QIC             typeof(IDropTarget));
        //QIC }

        /// <summary>
        /// Returns an <see cref="IShellFolder"/> representing the
        /// item.
        /// </summary>
        public IShellFolder GetIShellFolder()
        {
            IntPtr result = m_ComInterface.BindToHandler(IntPtr.Zero,
                BHID.SFObject, typeof(IShellFolder).GUID);
            return (IShellFolder)Marshal.GetTypedObjectForIUnknown(result,
                typeof(IShellFolder));
        }

        /// <summary>
        /// Gets the index in the system image list of the icon representing
        /// the item.
        /// </summary>
        /// 
        /// <param name="type">
        /// The type of icon to retrieve.
        /// </param>
        /// 
        /// <param name="flags">
        /// Flags detailing additional information to be conveyed by the icon.
        /// </param>
        /// 
        /// <returns></returns>
        public int GetSystemImageListIndex(ShellIconType type, ShellIconFlags flags)
        {
            SHFILEINFO info = new SHFILEINFO();
            IntPtr result = Shell32.SHGetFileInfo(Pidl, 0, out info,
                Marshal.SizeOf(info),
                SHGFI.ICON | SHGFI.SYSICONINDEX | SHGFI.OVERLAYINDEX | SHGFI.PIDL |
                (SHGFI)type | (SHGFI)flags);

            if (result == IntPtr.Zero)
            {
                throw new Exception("Error retreiving shell folder icon");
            }

            return info.iIcon;
        }

        /// <summary>
        /// Tests whether the <see cref="ShellItem"/> is the immediate parent 
        /// of another item.
        /// </summary>
        /// 
        /// <param name="item">
        /// The potential child item.
        /// </param>
        public bool IsImmediateParentOf(ShellItem item)
        {
            return IsFolder && Shell32.ILIsParent(Pidl, item.Pidl, true);
        }

        /// <summary>
        /// Tests whether the <see cref="ShellItem"/> is the parent of 
        /// another item.
        /// </summary>
        /// 
        /// <param name="item">
        /// The potential child item.
        /// </param>
        public bool IsParentOf(ShellItem item)
        {
            return IsFolder && Shell32.ILIsParent(Pidl, item.Pidl, false);
        }

        /// <summary>
        /// Returns a string representation of the <see cref="ShellItem"/>.
        /// </summary>
        public override string ToString()
        {
            return ToUri().ToString();
        }

        /// <summary>
        /// Returns a URI representation of the <see cref="ShellItem"/>.
        /// </summary>
        public Uri ToUri()
        {
            KnownFolderManager manager = new KnownFolderManager();
            StringBuilder path = new StringBuilder("shell:///");
            KnownFolder knownFolder = manager.FindNearestParent(this);

            if (knownFolder != null)
            {
                List<string> folders = new List<string>();
                ShellItem knownFolderItem = knownFolder.CreateShellItem();
                ShellItem item = this;

                while (item != knownFolderItem)
                {
                    folders.Add(item.GetDisplayName(SIGDN.PARENTRELATIVEPARSING));
                    item = item.Parent;
                }

                folders.Reverse();
                path.Append(knownFolder.Name);
                foreach (string s in folders)
                {
                    path.Append('/');
                    path.Append(s);
                }

                return new Uri(path.ToString());
            }
            else
            {
                return new Uri(FileSystemPath);
            }
        }

        /// <summary>
        /// Gets a child item.
        /// </summary>
        /// 
        /// <param name="name">
        /// The name of the child item.
        /// </param>
        public ShellItem this[string name]
        {
            get
            {
                return new ShellItem(this, name);
            }
        }

        /// <summary>
        /// Tests if two <see cref="ShellItem"/>s refer to the same folder.
        /// </summary>
        /// 
        /// <param name="a">
        /// The first folder.
        /// </param>
        /// 
        /// <param name="b">
        /// The second folder.
        /// </param>
        public static bool operator !=(ShellItem a, ShellItem b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return !object.ReferenceEquals(b, null);
            }
            else
            {
                return !a.Equals(b);
            }
        }

        /// <summary>
        /// Tests if two <see cref="ShellItem"/>s refer to the same folder.
        /// </summary>
        /// 
        /// <param name="a">
        /// The first folder.
        /// </param>
        /// 
        /// <param name="b">
        /// The second folder.
        /// </param>
        public static bool operator ==(ShellItem a, ShellItem b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }
            else
            {
                return a.Equals(b);
            }
        }

        /// <summary>
        /// Gets the underlying <see cref="IShellItem"/> COM interface.
        /// </summary>
        public IShellItem ComInterface
        {
            get { return m_ComInterface; }
        }

        /// <summary>
        /// Gets the normal display name of the item.
        /// </summary>
        public string DisplayName
        {
            get { return GetDisplayName(SIGDN.NORMALDISPLAY); }
        }

        /// <summary>
        /// Gets the file system path of the item.
        /// </summary>
        public string FileSystemPath
        {
            get { return GetDisplayName(SIGDN.FILESYSPATH); }
        }

        /// <summary>
        /// Gets a value indicating whether the item has subfolders.
        /// </summary>
        public bool HasSubFolders
        {
            get { return m_ComInterface.GetAttributes(SFGAO.HASSUBFOLDER) != 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the item is a file system item.
        /// </summary>
        public bool IsFileSystem
        {
            get { return m_ComInterface.GetAttributes(SFGAO.FILESYSTEM) != 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the item is a file system item
        /// or the child of a file system item.
        /// </summary>
        public bool IsFileSystemAncestor
        {
            get { return m_ComInterface.GetAttributes(SFGAO.FILESYSANCESTOR) != 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the item is a folder.
        /// </summary>
        public bool IsFolder
        {
            get { return m_ComInterface.GetAttributes(SFGAO.FOLDER) != 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the item is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return m_ComInterface.GetAttributes(SFGAO.READONLY) != 0; }
        }

        /// <summary>
        /// Gets the item's parent.
        /// </summary>
        public ShellItem Parent
        {
            get
            {
                IShellItem item;
                HResult result = m_ComInterface.GetParent(out item);

                if (result == HResult.S_OK)
                {
                    return new ShellItem(item);
                }
                else if (result == HResult.MK_E_NOOBJECT)
                {
                    return null;
                }
                else
                {
                    Marshal.ThrowExceptionForHR((int)result);
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the item's parsing name.
        /// </summary>
        public string ParsingName
        {
            get { return GetDisplayName(SIGDN.DESKTOPABSOLUTEPARSING); }
        }

        /// <summary>
        /// Gets a PIDL representing the item.
        /// </summary>
        public IntPtr Pidl
        {
            get { return GetIDListFromObject(m_ComInterface); }
        }

        /// <summary>
        /// Gets the item's shell icon.
        /// </summary>
        public Icon ShellIcon
        {
            get
            {
                SHFILEINFO info = new SHFILEINFO();
                IntPtr result = Shell32.SHGetFileInfo(Pidl, 0, out info,
                    Marshal.SizeOf(info),
                    SHGFI.ADDOVERLAYS | SHGFI.ICON |
                    SHGFI.SHELLICONSIZE | SHGFI.PIDL);

                if (result == IntPtr.Zero)
                {
                    throw new Exception("Error retreiving shell folder icon");
                }

                return Icon.FromHandle(info.hIcon);
            }
        }

        //QIC /// <summary>
        //QIC /// Gets the item's tooltip text.
        //QIC /// </summary>
        //QIC public string ToolTipText
        //QIC {
        //QIC     get
        //QIC     {
        //QIC         IntPtr result;
        //QIC         IQueryInfo queryInfo;
        //QIC         IntPtr infoTipPtr;
        //QIC         string infoTip;

        //QIC         try
        //QIC         {
        //QIC             IntPtr relativePidl = Shell32.ILFindLastID(Pidl);
        //QIC             Parent.GetIShellFolder().GetUIObjectOf(IntPtr.Zero, 1,
        //QIC                 new IntPtr[] { relativePidl },
        //QIC                 typeof(IQueryInfo).GUID, 0, out result);
        //QIC         }
        //QIC         catch (Exception)
        //QIC         {
        //QIC             return string.Empty;
        //QIC         }

        //QIC         queryInfo = (IQueryInfo)
        //QIC             Marshal.GetTypedObjectForIUnknown(result,
        //QIC                 typeof(IQueryInfo));
        //QIC         queryInfo.GetInfoTip(0, out infoTipPtr);
        //QIC         infoTip = Marshal.PtrToStringUni(infoTipPtr);
        //QIC         Ole32.CoTaskMemFree(infoTipPtr);
        //QIC         return infoTip;
        //QIC     }
        //QIC }

        /// <summary>
        /// Gets the Desktop folder.
        /// </summary>
        public static ShellItem Desktop
        {
            get
            {
                if (m_Desktop == null)
                {
                    IShellItem item;
                    IntPtr pidl;

                    Shell32.SHGetSpecialFolderLocation(
                         IntPtr.Zero, (CSIDL)Environment.SpecialFolder.Desktop,
                         out pidl);

                    try
                    {
                        item = CreateItemFromIDList(pidl);
                    }
                    finally
                    {
                        Shell32.ILFree(pidl);
                    }

                    m_Desktop = new ShellItem(item);
                }
                return m_Desktop;
            }
        }

        internal static bool RunningVista
        {
            get { return Environment.OSVersion.Version.Major >= 6; }
        }

        void Initialize(Uri uri)
        {
            if (uri.Scheme == "file")
            {
                m_ComInterface = CreateItemFromParsingName(uri.LocalPath);
            }
            else if (uri.Scheme == "shell")
            {
                InitializeFromShellUri(uri);
            }
            else
            {
                throw new InvalidOperationException("Invalid uri scheme");
            }
        }

        void InitializeFromShellUri(Uri uri)
        {
            KnownFolderManager manager = new KnownFolderManager();
            string path = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
            string knownFolder;
            string restOfPath;
            int separatorIndex = path.IndexOf('/');

            if (separatorIndex != -1)
            {
                knownFolder = path.Substring(0, separatorIndex);
                restOfPath = path.Substring(separatorIndex + 1);
            }
            else
            {
                knownFolder = path;
                restOfPath = string.Empty;
            }

            m_ComInterface = manager.GetFolder(knownFolder).CreateShellItem().ComInterface;

            if (restOfPath != string.Empty)
            {
                m_ComInterface = this[restOfPath.Replace('/', '\\')].ComInterface;
            }
        }

        static IShellItem CreateItemFromIDList(IntPtr pidl)
        {
            if (RunningVista)
            {
                return Shell32.SHCreateItemFromIDList(pidl,
                    typeof(IShellItem).GUID);
            }
            else
            {
                return new Interop.VistaBridge.ShellItemImpl(
                    pidl, false);
            }
        }

        static IShellItem CreateItemFromParsingName(string path)
        {
            if (RunningVista)
            {
                return Shell32.SHCreateItemFromParsingName(path, IntPtr.Zero,
                    typeof(IShellItem).GUID);
            }
            else
            {
                IShellFolder desktop = Desktop.GetIShellFolder();
                uint attributes = 0;
                uint eaten;
                IntPtr pidl;

                desktop.ParseDisplayName(IntPtr.Zero, IntPtr.Zero,
                    path, out eaten, out pidl, ref attributes);
                return new Interop.VistaBridge.ShellItemImpl(
                    pidl, true);
            }
        }

        static IShellItem CreateItemWithParent(ShellItem parent, IntPtr pidl)
        {
            if (RunningVista)
            {
                return Shell32.SHCreateItemWithParent(IntPtr.Zero,
                    parent.GetIShellFolder(), pidl, typeof(IShellItem).GUID);
            }
            else
            {
                Interop.VistaBridge.ShellItemImpl impl =
                    (Interop.VistaBridge.ShellItemImpl)parent.ComInterface;
                return new Interop.VistaBridge.ShellItemImpl(
                    Shell32.ILCombine(impl.Pidl, pidl), true);
            }
        }

        static IntPtr GetIDListFromObject(IShellItem item)
        {
            if (RunningVista)
            {
                return Shell32.SHGetIDListFromObject(item);
            }
            else
            {
                return ((Interop.VistaBridge.ShellItemImpl)item).Pidl;
            }
        }

        static IEnumIDList GetIEnumIDList(IShellFolder folder, SHCONTF flags)
        {
            IEnumIDList result;

            if (folder.EnumObjects(IntPtr.Zero, flags, out result) == HResult.S_OK)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        //QIC new to avoid compiler warning 
        public override int GetHashCode()
        {
#if NET5
            return HashCode.Combine(DisplayName, FileSystemPath, Pidl);
#else
            int hashCode = 111570049;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisplayName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FileSystemPath);
            hashCode = hashCode * -1521134295 + Pidl.GetHashCode();
            return hashCode;
#endif
        }

        IShellItem m_ComInterface;
        static ShellItem m_Desktop;
    }

    class ShellItemConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context,
                                            Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertFrom(context, sourceType);
            }
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
                                           CultureInfo culture,
                                           object value)
        {
            if (value is string)
            {
                string s = (string)value;

                if (s.Length == 0)
                {
                    return ShellItem.Desktop;
                }
                else
                {
                    return new ShellItem(s);
                }
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                         CultureInfo culture,
                                         object value,
                                         Type destinationType)
        {
            if (value is ShellItem)
            {
                Uri uri = ((ShellItem)value).ToUri();

                if (destinationType == typeof(string))
                {
                    if (uri.Scheme == "file")
                    {
                        return uri.LocalPath;
                    }
                    else
                    {
                        return uri.ToString();
                    }
                }
                else if (destinationType == typeof(InstanceDescriptor))
                {
                    return new InstanceDescriptor(
                        typeof(ShellItem).GetConstructor(new Type[] { typeof(string) }),
                        new object[] { uri.ToString() });
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    /// <summary>
    /// Enumerates the types of shell icons.
    /// </summary>
    public enum ShellIconType
    {
        /// <summary>The system large icon type</summary>
        LargeIcon = SHGFI.LARGEICON,

        /// <summary>The system shell icon type</summary>
        ShellIcon = SHGFI.SHELLICONSIZE,

        /// <summary>The system small icon type</summary>
        SmallIcon = SHGFI.SMALLICON,
    }

    /// <summary>
    /// Enumerates the optional styles that can be applied to shell icons.
    /// </summary>
    [Flags]
    public enum ShellIconFlags
    {
        /// <summary>The icon is displayed opened.</summary>
        OpenIcon = SHGFI.OPENICON,

        /// <summary>Get the overlay for the icon as well.</summary>
        OverlayIndex = SHGFI.OVERLAYINDEX
    }
}
