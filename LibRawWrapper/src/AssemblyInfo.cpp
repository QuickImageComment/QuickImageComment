#include <libraw_version.h>

using namespace System;
using namespace System::Reflection;
using namespace System::Runtime::CompilerServices;
using namespace System::Runtime::InteropServices;
using namespace System::Security::Permissions;

[assembly:AssemblyTitleAttribute(L"LibRaw C++/CLI Wrapper")];
[assembly:AssemblyDescriptionAttribute(L"C++/CLI wrapper around the LibRaw library for reading RAW files")];
[assembly:AssemblyConfigurationAttribute(L"")];
[assembly:AssemblyCompanyAttribute(L"Hurlbert Vision Lab")];
[assembly:AssemblyProductAttribute(L"HurlbertVisionLab.LibRawWrapper")];
[assembly:AssemblyCopyrightAttribute(L"Copyright © 2021-2023 Jan Kučera")];
[assembly:AssemblyTrademarkAttribute(L"")];
[assembly:AssemblyCultureAttribute(L"")];
									
#define _JOIN(a, b, c) #a "." #b "." #c ".0"
#define JOIN(a, b, c) _JOIN(a, b, c)

[assembly:AssemblyVersionAttribute("1.0.2.1")];
[assembly:AssemblyFileVersionAttribute(JOIN(LIBRAW_MAJOR_VERSION, LIBRAW_MINOR_VERSION, LIBRAW_PATCH_VERSION))];

[assembly:ComVisible(false)];
