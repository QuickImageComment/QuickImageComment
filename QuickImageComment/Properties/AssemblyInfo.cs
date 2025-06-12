//Copyright (C) 2009 Norbert Wagner

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either version 2
//of the License, or (at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program; if not, write to the Free Software
//Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("QuickImageComment")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Norbert Wagner")]
[assembly: AssemblyProduct("QuickImageComment")]
[assembly: AssemblyCopyright("Copyright © Norbert Wagner 2007-2025")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("daf752d6-d3be-4bdf-a73d-a10e85cf94f2")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("4.67.*")]
[assembly: AssemblyInformationalVersion("4.67" + AssemblyInfo.VersionSuffix + AssemblyInfo.Platform + AssemblyInfo.Framework)]
[assembly: AssemblyFileVersion("4.67.0.0")]

class AssemblyInfo
{
#if DEBUG
    public const string VersionSuffix = "-DBG";
#else
    public const string VersionSuffix = "-Beta-4";
#endif
    // Version to Check is the last published version
    // for beta versions it is one before AssemblyVersion
    // for released versions it is same as AssemblyVersion
    public const string VersionToCheck = "4.66";

#if PLATFORMTARGET_X64
    public const string Platform = " - 64 Bit";
#else
    public const string Platform = " - 32 Bit";
#endif

#if NET5
    public const string Framework = " .Net 5";
#elif NET461
    public const string Framework = " .Net 4.6.1";
#elif NET4
    public const string Framework = " .Net 4";
#else
    public const string Framework = " .Net ??";
#endif
}
