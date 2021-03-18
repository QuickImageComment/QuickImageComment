//Copyright (C) 2020 Norbert Wagner

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

// *****************************************************************************
// this file includes additional and modified methods for makernote_int
// *****************************************************************************

// *****************************************************************************
// included header files

#include "makernote_int.hpp"
#include "makernote_int_add.hpp"

// + standard includes
#include <shlobj.h>

// *****************************************************************************
// class member definitions
namespace Exiv2 {
    namespace Internal {

        const char* iniPath;

        void setIniPath(const char* givenIniPath)
        {
            iniPath = strdup(givenIniPath);
        }

        std::string getExiv2ConfigPath()
        {
            std::string homedir;
            std::string inifile;
            inifile = "exiv2.ini";
            DWORD ftyp = GetFileAttributesA(Exiv2::Internal::iniPath);
            if (ftyp & FILE_ATTRIBUTE_DIRECTORY) {
                homedir = std::string(Exiv2::Internal::iniPath);
            }
            else {
                char path[MAX_PATH];
                if (SUCCEEDED(SHGetFolderPathA(NULL, CSIDL_PROFILE, NULL, 0, path))) {
                    homedir = std::string(path);
                }
            }
            return homedir + EXV_SEPARATOR_CHR + inifile;
        }
    }
}
