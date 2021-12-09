//Copyright (C) 2019 Norbert Wagner

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

#include "image.hpp"
#include "exif.hpp"
#include "exiv2app.hpp"
#include <iostream>
#include <iomanip>
#include <cassert>
#include "exiv2.hpp"
#include "makernote_int.hpp"
#include "makernote_int_add.hpp"
#include "properties.hpp"

//#define TRACING 200

#define VERSION "0.27.5.0"

// definitions for Exif Easy Access
typedef Exiv2::ExifData::const_iterator(*EasyAccessFct)(const Exiv2::ExifData& ed);
struct EasyAccess {
    EasyAccessFct findFct_;
    const char* tagName;
    const char* label_;
};

// status and option codes
static const int exiv2StatusException = 100;
static const int exiv2WriteOptionDefault = 0;
static const int exiv2WriteOptionXmpText = 1;
static const int exiv2WriteOptionXaBag = 2;
static const int exiv2WriteOptionXsStruct = 3;

// used to separate entries in ouptut stream with tag descriptions
char const separator[] = "\n";
static char* token;
static char* next_token;

// used to iterator through meta data
static Exiv2::Image::AutoPtr image;
static Exiv2::ExifData::const_iterator exifEndItem;
static Exiv2::ExifData::const_iterator exifMetaDataItem;
static Exiv2::IptcData::const_iterator iptcEndItem;
static Exiv2::IptcData::const_iterator iptcMetaDataItem;
static Exiv2::XmpData::const_iterator xmpEndItem;
static Exiv2::XmpData::const_iterator xmpMetaDataItem;
static int xmpBagSeqCount;
static bool xmpLangLoop;
static Exiv2::LangAltValue::ValueType::const_iterator xmpLangIterator;
static Exiv2::LangAltValue::ValueType::const_iterator xmpLangEnd;

// used to write meta data
static int    writeMetaDatumCountMax;
static int    writeMetaDatumCountAct;
static char** writeTags;
static char** writeValues;
static int* writeOptions;

// for tracing
static int    tracingCount = 0;
static char** tracingLog;

static const EasyAccess easyAccess[] = {
    { Exiv2::orientation      ,"ExifEasy.Orientation"         ,"Orientation (selected from different tags - especially from maker specific tags)"},
    { Exiv2::isoSpeed         ,"ExifEasy.ISOspeed"            ,"ISO speed (selected from different tags - especially from maker specific tags)"},
    { Exiv2::flashBias        ,"ExifEasy.FlashBias"           ,"Flash bias (selected from different tags - especially from maker specific tags)"},
    { Exiv2::exposureMode     ,"ExifEasy.ExposureMode"        ,"Exposure mode (selected from different tags - especially from maker specific tags)"},
    { Exiv2::sceneMode        ,"ExifEasy.SceneMode"           ,"Scene mode (selected from different tags - especially from maker specific tags)"},
    { Exiv2::macroMode        ,"ExifEasy.MacroMode"           ,"Macro mode (selected from different tags - especially from maker specific tags)"},
    { Exiv2::imageQuality     ,"ExifEasy.ImageQuality"        ,"Image quality (selected from different tags - especially from maker specific tags)"},
    { Exiv2::whiteBalance     ,"ExifEasy.WhiteBalance"        ,"White balance (selected from different tags - especially from maker specific tags)"},
    { Exiv2::lensName         ,"ExifEasy.LensName"            ,"Lens name (selected from different tags - especially from maker specific tags)"},
    { Exiv2::saturation       ,"ExifEasy.Saturation"          ,"Saturation (selected from different tags - especially from maker specific tags)"},
    { Exiv2::sharpness        ,"ExifEasy.Sharpness"           ,"Sharpness (selected from different tags - especially from maker specific tags)"},
    { Exiv2::contrast         ,"ExifEasy.Contrast"            ,"Contrast (selected from different tags - especially from maker specific tags)"},
    { Exiv2::sceneCaptureType ,"ExifEasy.SceneCaptureType"    ,"Scene Capture Type (selected from different tags - especially from maker specific tags)"},
    { Exiv2::meteringMode     ,"ExifEasy.MeteringMode"        ,"Metering Mode (selected from different tags - especially from maker specific tags)"},
    { Exiv2::make             ,"ExifEasy.CameraMake"          ,"Camera make (selected from different tags - especially from maker specific tags)"},
    { Exiv2::model            ,"ExifEasy.CameraModel"         ,"Camera model (selected from different tags - especially from maker specific tags)"},
    { Exiv2::exposureTime     ,"ExifEasy.ExposureTime"        ,"Exposure time (selected from different tags - especially from maker specific tags)"},
    { Exiv2::fNumber          ,"ExifEasy.FNumber"             ,"FNumber (selected from different tags - especially from maker specific tags)"},
    { Exiv2::subjectDistance  ,"ExifEasy.SubjectDistance"     ,"Subject distance (selected from different tags - especially from maker specific tags)"},
    { Exiv2::serialNumber     ,"ExifEasy.CameraSerialNumber"  ,"Camera serial number (selected from different tags - especially from maker specific tags)"},
    { Exiv2::focalLength      ,"ExifEasy.FocalLength"         ,"Focal length (selected from different tags - especially from maker specific tags)"},
    { Exiv2::afPoint          ,"ExifEasy.AFPoint"             , "AF point (selected from different tags - especially from maker specific tags)"}
};

//-------------------------------------------------------------------------
// get list of Exif tags
//-------------------------------------------------------------------------
// prepare and get first description
extern "C" __declspec(dllexport) int __cdecl exiv2getFirstExifTagDescription(LPSTR * retStr) {
    std::ostringstream oss;
    // get standard tags
    Exiv2::ExifTags::taglist(oss);

    // get also mpf tags
    const Exiv2::TagInfo* mpf = Exiv2::Internal::mpfTagList();
    for (int i = 0; mpf[i].tag_ != 0xffff; ++i) {
        oss << mpf[i] << "\n";
    }

    // get makernotes, based on http://dev.exiv2.org/boards/3/topics/583
    const Exiv2::GroupInfo* gi = Exiv2::ExifTags::groupList();
    while (gi->tagList_ != 0) {
        if (strcmp(gi->ifdName_, "Makernote") == 0) {
            Exiv2::ExifTags::taglist(oss, gi->groupName_);
        }
        ++gi;
    }

    char* tagListString = strdup(oss.str().c_str());
    token = NULL;
    next_token = NULL;

    token = strtok_s(tagListString, separator, &next_token);
    *retStr = strdup(token);
    return 0;
}

// get next description (also used for IPTC and XMP)
extern "C" __declspec(dllexport) int __cdecl exiv2getNextTagDescription(LPSTR * retStr) {
    token = strtok_s(NULL, separator, &next_token);
    if (token == NULL) {
        return 1;
    }
    else {
        *retStr = strdup(token);
        return 0;
    }
}

//-------------------------------------------------------------------------
// get Exif Easy description by index
//-------------------------------------------------------------------------
extern "C" __declspec(dllexport) int __cdecl exiv2getExifEasyTagDescription(int index, LPSTR * retStr) {
    if (index < EXV_COUNTOF(easyAccess)) {
        char* tagDescription;
        tagDescription = (char*)malloc(strlen(easyAccess[index].tagName) + 12 + strlen(easyAccess[index].label_) + 1);
        strcpy(tagDescription, easyAccess[index].tagName);
        strcat(tagDescription, ",\tReadonly,\t");
        strcat(tagDescription, easyAccess[index].label_);
        *retStr = strdup(tagDescription);
        return 0;
    }
    else {
        return 1;
    }
}

//-------------------------------------------------------------------------
// get list of IPTC tags
//-------------------------------------------------------------------------
// prepare and get first description
extern "C" __declspec(dllexport) int __cdecl exiv2getFirstIptcTagDescription(LPSTR * retStr) {
    std::ostringstream oss;
    Exiv2::IptcDataSets::dataSetList(oss);

    char* tagListString = strdup(oss.str().c_str());
    token = NULL;
    next_token = NULL;

    token = strtok_s(tagListString, separator, &next_token);
    *retStr = strdup(token);
    return 0;
}

//-------------------------------------------------------------------------
// get list of XMP tags
//-------------------------------------------------------------------------
// print properties of one XMP group
void printProperties(std::ostream& oss, const std::string& prefix)
{
    const Exiv2::XmpPropertyInfo* pl = Exiv2::XmpProperties::propertyList(prefix);
    if (pl) {
        for (int i = 0; pl[i].name_ != 0; ++i) {
            oss << "Xmp." << prefix << "." << pl[i];
        }
    }
}

// prepare and get first description
extern "C" __declspec(dllexport) int __cdecl exiv2getFirstXmpTagDescription(LPSTR * retStr) {
    std::ostringstream oss;

    printProperties(oss, "dc");
    printProperties(oss, "digiKam");
    printProperties(oss, "kipi");
    printProperties(oss, "xmp");
    printProperties(oss, "xmpRights");
    printProperties(oss, "xmpMM");
    printProperties(oss, "xmpBJ");
    printProperties(oss, "xmpTPg");
    printProperties(oss, "xmpDM");
    printProperties(oss, "MicrosoftPhoto");
    printProperties(oss, "lr");
    printProperties(oss, "pdf");
    printProperties(oss, "photoshop");
    printProperties(oss, "crs");
    printProperties(oss, "crss");
    printProperties(oss, "tiff");
    printProperties(oss, "exif");
    printProperties(oss, "exifEX");
    printProperties(oss, "aux");
    printProperties(oss, "iptc");
    printProperties(oss, "Iptc4xmpCore");
    printProperties(oss, "iptcExt");
    printProperties(oss, "Iptc4xmpExt");
    printProperties(oss, "plus");
    printProperties(oss, "mediapro");
    printProperties(oss, "expressionmedia");
    printProperties(oss, "MP");
    printProperties(oss, "MPRI");
    printProperties(oss, "MPReg");
    printProperties(oss, "mwg-rs");
    printProperties(oss, "mwg-kw");
    printProperties(oss, "video");
    printProperties(oss, "audio");
    printProperties(oss, "dwc");
    printProperties(oss, "dcterms");
    printProperties(oss, "acdsee");
    printProperties(oss, "GPano");

    char* tagListString = strdup(oss.str().c_str());
    token = NULL;
    next_token = NULL;

    // Copy tokens
    // Establish string and get the first token:
    token = strtok_s(tagListString, separator, &next_token);
    *retStr = strdup(token);
    return 0;
}

//-------------------------------------------------------------------------
// Read data from image using file name
//-------------------------------------------------------------------------
extern "C" __declspec(dllexport) int __cdecl exiv2readImageByFileName(LPSTR fileName, LPSTR givenIniPath,
    LPSTR * comment, bool* IptcUTF8, LPSTR * errorText)
{
    xmpBagSeqCount = -1;
    xmpLangLoop = false;
    *errorText = strdup("");
    Exiv2::Internal::setIniPath(givenIniPath);

    try
    {
        std::string sFileName = fileName;
        image = Exiv2::ImageFactory::open(sFileName);
        assert(image.get() != 0);
        // redirect error output to stringstream, so that it can be returned as String
        std::stringstream buffer;
        std::streambuf* old = std::cerr.rdbuf(buffer.rdbuf());

        image->readMetadata();
        *errorText = strdup(buffer.str().c_str());
        // if errortext is filled, this does not necessarily mean that no data are available, so continue
        std::cerr.rdbuf(old);

        // JPEG comment
        *comment = strdup(image->comment().c_str());

        // get flag IptcUTF8
        * IptcUTF8 = false;
        Exiv2::IptcData& iptcData = image->iptcData();
        Exiv2::IptcData::const_iterator metaDataItem = iptcData.findKey(Exiv2::IptcKey("Iptc.Envelope.CharacterSet"));
        if (metaDataItem != iptcData.end()) {
            char* value = strdup(metaDataItem->toString().c_str());
            if (!strcmp(value, "\x1B%G")) { // "<ESC>%G"
                *IptcUTF8 = true;
            }
        }

        return 0;
    }
    catch (Exiv2::AnyError& e) {
        *errorText = strdup(e.what());
        return exiv2StatusException;
    }
}

//-------------------------------------------------------------------------
// get Exif meta data item
//-------------------------------------------------------------------------

// get Exif buffer and first meta data item
extern "C" __declspec(dllexport) void __cdecl exiv2getExifDataIteratorAll(bool* exifAvail) {
    *exifAvail = false;
    Exiv2::ExifData& exifData = image->exifData();
    if (!exifData.empty()) {
        exifEndItem = exifData.end();
        exifMetaDataItem = exifData.begin();
        if (exifMetaDataItem != exifEndItem) {
            *exifAvail = true;
        }
    }
}

// get Exif buffer and first meta data item for key
extern "C" __declspec(dllexport) void __cdecl exiv2getExifDataIteratorKey(LPSTR keyString, bool* exifAvail) {
    *exifAvail = false;
    Exiv2::ExifData& exifData = image->exifData();
    if (!exifData.empty()) {
        exifEndItem = exifData.end();
        exifMetaDataItem = exifData.findKey(Exiv2::ExifKey(keyString));
        if (exifMetaDataItem != exifEndItem) {
            *exifAvail = true;
        }
    }
}

// return one Exif meta data item and get next
extern "C" __declspec(dllexport) int __cdecl exiv2getExifDataItem(LPSTR * keyString, long* tag, LPSTR * typeName, long* count,
    long* size, LPSTR * valueString, LPSTR * interpretedString, float* valueFloat, bool* exifAvail, LPSTR * errorText) {

    *errorText = strdup("");
    try {
        *keyString = strdup(exifMetaDataItem->key().c_str());
        *tag = exifMetaDataItem->tag();
        // get type name from tagInfo, because for some special tags (e.g. Exif.Photo.UserComment)
        // the original Exif type "Undefined" is overwritten with "Comment"
        // logic partially copied from Exifdatum::write in exif.cpp
        const Exiv2::TagInfo* ti = Exiv2::Internal::tagInfo(*tag, static_cast<Exiv2::Internal::IfdId>(exifMetaDataItem->ifdId()));
        if (ti) {
            *typeName = strdup(Exiv2::TypeInfo::typeName(ti->typeId_));
        }
        else {
            // get type from exifMetaDataItem itself
            // Sometimes typeName is not set (sample picture from Nikon D90)
            const char* tempTypeName = exifMetaDataItem->typeName();
            if (tempTypeName) //exifMetaDataItem->typeName())
            {
                *typeName = strdup(exifMetaDataItem->typeName());
            }
            else
            {
                *typeName = strdup("Unknown");
            }
        }
        *count = exifMetaDataItem->count();
        *size = exifMetaDataItem->size();
        if (!strcmp(*keyString, "Exif.Photo.MakerNote"))
        {
            // no sense to copy MakerNote, as it is container and not copying saves time (about 150 - 200 ms)
            *valueString = strdup("");
            *interpretedString = strdup("");
            *valueFloat = (float)-999.999;
        }
        else
        {
            if (!strcmp(*typeName, "SByte"))
            {
                long longValue = exifMetaDataItem->toLong(0);
                if (longValue > 127)
                {
                    longValue = longValue - 256;
                }
                char StringValue[4];
                sprintf(StringValue, "%d", longValue);
                *valueString = strdup(StringValue);
                *interpretedString = strdup(StringValue);
            }
            else
            {
                *valueString = strdup(exifMetaDataItem->toString().c_str());
                *interpretedString = strdup(exifMetaDataItem->print().c_str());
            }
            if (!strcmp(*typeName, "Rational") || !strcmp(*typeName, "SRational"))
            {
                // special check: this situation occured in images which 
                // were down-sized by MAGIX Foto Designer 7
                if (exifMetaDataItem->count() > 0)
                {
                    *valueFloat = exifMetaDataItem->toFloat();
                }
                else
                {
                    *valueFloat = (float)0.0;
                }
            }
            else
            {
                *valueFloat = (float)-999.999;
            }
        }

        ++exifMetaDataItem;
        *exifAvail = exifMetaDataItem != exifEndItem;
        return 0;
    }
    catch (Exiv2::AnyError& e) {
        *errorText = strdup(e.what());
        *exifAvail = false;
        return exiv2StatusException;
    }
}

//-------------------------------------------------------------------------
// get one Exif Easy Access meta data item
//-------------------------------------------------------------------------
extern "C" __declspec(dllexport) int __cdecl exiv2getExifEasyDataItem(int* index, LPSTR * keyString, long* tag, LPSTR * typeName, long* count,
    long* size, LPSTR * valueString, LPSTR * interpretedString, float* valueFloat, LPSTR * errorText) {

    *errorText = strdup("");
    try {
        Exiv2::ExifData& exifData = image->exifData();
        while (*index < EXV_COUNTOF(easyAccess)) {
            Exiv2::ExifData::const_iterator metaDataItem = easyAccess[*index].findFct_(exifData);
            if (metaDataItem != exifData.end()) {
                *keyString = strdup(easyAccess[*index].tagName);
                *tag = metaDataItem->tag();
                *typeName = strdup(metaDataItem->key().c_str());
                *count = metaDataItem->count();
                *size = metaDataItem->size();
                *valueString = strdup(metaDataItem->print(&exifData).c_str());
                *interpretedString = strdup(metaDataItem->print(&exifData).c_str());
                if (!strcmp(metaDataItem->typeName(), "Rational") || !strcmp(metaDataItem->typeName(), "SRational"))
                {
                    *valueFloat = metaDataItem->toFloat();
                }
                else
                {
                    *valueFloat = (float)-999.999;
                }
                // increase index for next call
                *index = *index + 1;
                return 0;
            }
            else {
                *index = *index + 1;
            }
        }
        // loop finished, no data found 
        return 1;
    }
    catch (Exiv2::AnyError& e) {
        *errorText = strdup(e.what());
        return exiv2StatusException;
    }
}

//-------------------------------------------------------------------------
// get IPTC meta data item
//-------------------------------------------------------------------------

// get Iptc buffer and first meta data item
extern "C" __declspec(dllexport) void __cdecl exiv2getIptcDataIteratorAll(bool* iptcAvail) {
    *iptcAvail = false;
    Exiv2::IptcData& iptcData = image->iptcData();
    if (!iptcData.empty()) {
        iptcEndItem = iptcData.end();
        iptcMetaDataItem = iptcData.begin();
        if (iptcMetaDataItem != iptcEndItem) {
            *iptcAvail = true;
        }
    }
}

// get Iptc buffer and first meta data item for key
extern "C" __declspec(dllexport) void __cdecl exiv2getIptcDataIteratorKey(LPSTR keyString, bool* iptcAvail) {
    *iptcAvail = false;
    Exiv2::IptcData& iptcData = image->iptcData();
    if (!iptcData.empty()) {
        iptcEndItem = iptcData.end();
        iptcMetaDataItem = iptcData.findKey(Exiv2::IptcKey(keyString));
        if (iptcMetaDataItem != iptcEndItem) {
            *iptcAvail = true;
        }
    }
}

// return one IPTC meta data item and get next
extern "C" __declspec(dllexport) int __cdecl exiv2getIptcDataItem(LPSTR * keyString, long* tag, LPSTR * typeName, long* count,
    long* size, LPSTR * valueString, LPSTR * interpretedString, float* valueFloat, bool* iptcAvail, LPSTR * errorText) {

    *errorText = strdup("");
    try {
        *keyString = strdup(iptcMetaDataItem->key().c_str());
        *tag = iptcMetaDataItem->tag();
        *typeName = strdup(iptcMetaDataItem->typeName());
        *count = iptcMetaDataItem->count();
        *size = iptcMetaDataItem->size();
        *valueString = strdup(iptcMetaDataItem->toString().c_str());
        *interpretedString = strdup(iptcMetaDataItem->print().c_str());
        if (!strcmp(iptcMetaDataItem->typeName(), "Rational") || !strcmp(iptcMetaDataItem->typeName(), "SRational"))
        {
            *valueFloat = iptcMetaDataItem->toFloat();
        }
        else
        {
            *valueFloat = (float)-999.999;
        }
        ++iptcMetaDataItem;
        *iptcAvail = iptcMetaDataItem != iptcEndItem;
        return 0;
    }
    catch (Exiv2::AnyError& e) {
        *errorText = strdup(e.what());
        *iptcAvail = false;
        return exiv2StatusException;
    }
}

//-------------------------------------------------------------------------
// get XMP meta data item
//-------------------------------------------------------------------------

// get Xmp buffer and first meta data item
extern "C" __declspec(dllexport) void __cdecl exiv2getXmpDataIteratorAll(bool* xmpAvail) {
    *xmpAvail = false;
    Exiv2::XmpData& xmpData = image->xmpData();
    if (!xmpData.empty()) {
        xmpEndItem = xmpData.end();
        xmpMetaDataItem = xmpData.begin();
        if (xmpMetaDataItem != xmpEndItem) {
            *xmpAvail = true;
        }
    }
}

// get Xmp buffer and first meta data item for key
extern "C" __declspec(dllexport) void __cdecl exiv2getXmpDataIteratorKey(LPSTR keyString, bool* xmpAvail) {
    *xmpAvail = false;
    Exiv2::XmpData& xmpData = image->xmpData();
    if (!xmpData.empty()) {
        xmpEndItem = xmpData.end();
        xmpMetaDataItem = xmpData.findKey(Exiv2::XmpKey(keyString));
        if (xmpMetaDataItem != xmpEndItem) {
            *xmpAvail = true;
        }
    }
}

// get general XMP meta data
static void getGeneralXmpMetaDatum(LPSTR* keyString, long* tag, LPSTR* typeName, LPSTR* language, long* count,
    long* size, float* valueFloat, Exiv2::XmpData::const_iterator metaDataItem)
{
    *keyString = strdup(metaDataItem->key().c_str());
    *tag = metaDataItem->tag();
    *typeName = strdup(metaDataItem->typeName());
    *language = strdup("");
    *count = metaDataItem->count();
    *size = metaDataItem->size();
    *valueFloat = (float)-999.999;
}

// return one XMP meta data item and get next
extern "C" __declspec(dllexport) int __cdecl exiv2getXmpDataItem(LPSTR * keyString, long* tag, LPSTR * typeName, LPSTR * language, long* count,
    long* size, LPSTR * valueString, LPSTR * interpretedString, float* valueFloat, bool* xmpAvail, LPSTR * errorText) {

    *errorText = strdup("");
    try {
        if (!strcmp(xmpMetaDataItem->typeName(), "XmpBag") ||
            !strcmp(xmpMetaDataItem->typeName(), "XmpSeq")) {
            if (xmpBagSeqCount < 0) {
                // init value for loop
                xmpBagSeqCount = 0;
            }
            // get one entry of XmpBag or XmpSeq
            getGeneralXmpMetaDatum(keyString, tag, typeName, language, count, size, valueFloat, xmpMetaDataItem);
            if (*size == 0)
            {
                *valueString = strdup("");
                *interpretedString = strdup("");
            }
            else
            {
                // assumption: original and interpreted value do not differ
                *valueString = strdup(xmpMetaDataItem->toString(xmpBagSeqCount).c_str());
                *interpretedString = strdup(xmpMetaDataItem->toString(xmpBagSeqCount).c_str());
                xmpBagSeqCount++;
                if (xmpBagSeqCount < xmpMetaDataItem->count()) {
                    // next call will continue with next entry of XmpBag or XmpSeq
                    return 0;
                }
                else {
                    // stop loop with language
                    xmpBagSeqCount = -1;
                    // continue getting next xmpMetaDataItem
                }
            }
        }
        else if (!strcmp(xmpMetaDataItem->typeName(), "LangAlt")) {
            if (!xmpLangLoop) {
                // init values to loop over languages
                const Exiv2::LangAltValue& value = static_cast<const Exiv2::LangAltValue&>(xmpMetaDataItem->value());
                xmpLangIterator = value.value_.begin();
                xmpLangEnd = value.value_.end();
                xmpLangLoop = true;
            }
            // get language entry
            getGeneralXmpMetaDatum(keyString, tag, typeName, language, count, size, valueFloat, xmpMetaDataItem);
            *language = strdup(xmpLangIterator->first.c_str());
            *valueString = strdup(xmpLangIterator->second.c_str());
            *interpretedString = strdup(xmpLangIterator->second.c_str());
            ++xmpLangIterator;
            if (xmpLangIterator != xmpLangEnd) {
                // next call will continue with next language
                return 0;
            }
            else {
                // stop loop with language
                xmpLangLoop = false;
                // continue getting next xmpMetaDataItem
            }
        }
        else {
            getGeneralXmpMetaDatum(keyString, tag, typeName, language, count, size, valueFloat, xmpMetaDataItem);
            *valueString = strdup(xmpMetaDataItem->toString().c_str());
            *interpretedString = strdup(xmpMetaDataItem->print().c_str());
        }
        ++xmpMetaDataItem;
        *xmpAvail = xmpMetaDataItem != xmpEndItem;
        return 0;
    }
    catch (Exiv2::AnyError& e) {
        *errorText = strdup(e.what());
        *xmpAvail = false;
        return exiv2StatusException;
    }
}

//-------------------------------------------------------------------------
// initialise buffer to write image
//-------------------------------------------------------------------------
extern "C" __declspec(dllexport) void __cdecl exiv2initWriteBuffer() {

#ifdef TRACING
    tracingLog = (char**)malloc(TRACING * sizeof(char*));
    tracingCount = 0;
#endif

    writeMetaDatumCountAct = 0;
    if (writeMetaDatumCountMax == 0) {
        writeMetaDatumCountMax = 5;
        writeTags = (char**)malloc(writeMetaDatumCountMax * sizeof(char*));
        writeValues = (char**)malloc(writeMetaDatumCountMax * sizeof(char*));
        writeOptions = (int*)malloc(writeMetaDatumCountMax * sizeof(int));
    }
}

//-------------------------------------------------------------------------
// add one item to write buffer
//-------------------------------------------------------------------------

// add item to buffer (not UTF8)
extern "C" __declspec(dllexport) void __cdecl exiv2addItemToBuffer(LPSTR tag, LPSTR value, int option) {
    if (writeMetaDatumCountAct < writeMetaDatumCountMax) {
        writeMetaDatumCountMax++;
        writeTags = (char**)realloc(writeTags, writeMetaDatumCountMax * sizeof(char*));
        writeValues = (char**)realloc(writeValues, writeMetaDatumCountMax * sizeof(char*));
        writeOptions = (int*)realloc(writeOptions, writeMetaDatumCountMax * sizeof(int));
    }
    writeTags[writeMetaDatumCountAct] = strdup(tag);
    writeValues[writeMetaDatumCountAct] = strdup(value);
    writeOptions[writeMetaDatumCountAct] = option;
    writeMetaDatumCountAct++;
}

// return UTF8 encoded string
std::string utf8EncodedString(const std::string& str)
{
    // convert to wide string
    int size_needed = MultiByteToWideChar(CP_ACP, 0, &str[0], (int)str.size(), NULL, 0);
    std::wstring wstr(size_needed, 0);
    MultiByteToWideChar(CP_ACP, 0, &str[0], (int)str.size(), &wstr[0], size_needed);

    // convert to UTF8 string
    size_needed = WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), NULL, 0, NULL, NULL);
    std::string utf8str(size_needed, 0);
    WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), &utf8str[0], size_needed, NULL, NULL);
    return utf8str;
}

// add UTF8 item to buffer
extern "C" __declspec(dllexport) void __cdecl exiv2addUtf8ItemToBuffer(LPSTR tag, LPSTR value, int option) {
    if (writeMetaDatumCountAct < writeMetaDatumCountMax) {
        writeMetaDatumCountMax++;
        writeTags = (char**)realloc(writeTags, writeMetaDatumCountMax * sizeof(char*));
        writeValues = (char**)realloc(writeValues, writeMetaDatumCountMax * sizeof(char*));
        writeOptions = (int*)realloc(writeOptions, writeMetaDatumCountMax * sizeof(int));
    }

    writeTags[writeMetaDatumCountAct] = strdup(tag);
    std::string utf8string = utf8EncodedString(value);
    writeValues[writeMetaDatumCountAct] = strdup(utf8string.c_str());
    writeOptions[writeMetaDatumCountAct] = option;
    writeMetaDatumCountAct++;
}

//-------------------------------------------------------------------------
// write metadata to image
//-------------------------------------------------------------------------
extern "C" __declspec(dllexport) int __cdecl exiv2writeImage(LPSTR fileName, LPSTR comment, LPSTR * errorText) {

#ifdef TRACING
    // not elegant because temporary string could be longer, but this size should fit all needs - and anyhow only for tracing during development
    char   tracingTemp[256];
    tracingLog = (char**)malloc(TRACING * sizeof(char*));
    tracingCount = 0;
#endif

    * errorText = strdup("");
    try {
        Exiv2::Image::AutoPtr image = Exiv2::ImageFactory::open(fileName);
        assert(image.get() != 0);
        image->readMetadata();
        Exiv2::ExifData& exifData = image->exifData();
        Exiv2::IptcData& iptcData = image->iptcData();
        Exiv2::XmpData& xmpData = image->xmpData();

        // first delete all existing entries
        // starting at the end so that XMP-struct values are deleted from nodes to root
        for (int ii = writeMetaDatumCountAct - 1; ii >= 0; ii--) {
            if (!strncmp(writeTags[ii], "Exif.", 5)) {
                Exiv2::ExifKey theExifKey = Exiv2::ExifKey(writeTags[ii]);
                Exiv2::ExifData::iterator pos = exifData.findKey(theExifKey);
                while (pos != exifData.end()) {
                    exifData.erase(pos);
                    pos = exifData.findKey(theExifKey);
                }
            }
            else if (!strncmp(writeTags[ii], "Iptc.", 5)) {
                Exiv2::IptcKey theIptcKey = Exiv2::IptcKey(writeTags[ii]);
                Exiv2::IptcData::iterator pos = iptcData.findKey(theIptcKey);
                while (pos != iptcData.end()) {
#ifdef TRACING
                    strcpy(tracingTemp, "Erase:");
                    strcat(tracingTemp, writeTags[ii]);
                    if (tracingCount < TRACING) tracingLog[tracingCount++] = strdup(tracingTemp);
#endif
                    iptcData.erase(pos);
                    pos = iptcData.findKey(theIptcKey);
                }
            }
            else if (!strncmp(writeTags[ii], "Xmp.", 4)) {
                size_t taglen = strlen(writeTags[ii]);
                Exiv2::XmpData::iterator metaDataItem = xmpData.end();
                while (metaDataItem > xmpData.begin())
                {
                    --metaDataItem;
                    char* metaDataItemKey = strdup(metaDataItem->key().c_str());
                    // same key found or key with same start followed by [ or /
                    if (!strcmp(writeTags[ii], metaDataItemKey) ||
                        (!strncmp(writeTags[ii], metaDataItemKey, taglen) &&
                            strlen(metaDataItemKey) > taglen &&
                            (metaDataItemKey[taglen] == '[' || metaDataItemKey[taglen] == '/')))
                    {
#ifdef TRACING
                        strcpy(tracingTemp, "Erase: ");
                        strcat(tracingTemp, metaDataItemKey);
                        if (tracingCount < TRACING) tracingLog[tracingCount++] = strdup(tracingTemp);
#endif
                        xmpData.erase(metaDataItem);
                    }
                }
            }
            else {
                *errorText = strdup("Tag name does not start with Exif, Iptc or Xmp");
                return exiv2StatusException;
            }
        }

        for (int ii = 0; ii < writeMetaDatumCountAct; ii++) {
#ifdef TRACING
            strcpy(tracingTemp, "Tag:");
            strcat(tracingTemp, writeTags[ii]);
            strcat(tracingTemp, "=");
            strcat(tracingTemp, writeValues[ii]);
            if (tracingCount < TRACING) tracingLog[tracingCount++] = strdup(tracingTemp);
#endif
            // only write non empty values
            if (strlen(writeValues[ii]) > 0 || writeOptions[ii] > exiv2WriteOptionDefault)
            {
                if (writeOptions[ii] == exiv2WriteOptionDefault)
                {
#ifdef TRACING
                    if (tracingCount < TRACING) tracingLog[tracingCount++] = strdup("WriteDefault");
#endif
                    if (!strncmp(writeTags[ii], "Exif.", 5)) {
                        Exiv2::ExifKey exifKey(writeTags[ii]);
                        Exiv2::Value::AutoPtr value = Exiv2::Value::create(exifKey.defaultTypeId());
                        value->read(writeValues[ii]);
                        exifData.add(exifKey, value.get());
                    }
                    else if (!strncmp(writeTags[ii], "Iptc.", 5)) {
                        Exiv2::IptcKey iptcKey(writeTags[ii]);
                        Exiv2::Value::AutoPtr value = Exiv2::Value::create(Exiv2::IptcDataSets::dataSetType(iptcKey.tag(),
                            iptcKey.record()));
                        value->read(writeValues[ii]);
                        iptcData.add(iptcKey, value.get());
                    }
                    else if (!strncmp(writeTags[ii], "Xmp.", 4)) {
                        Exiv2::XmpKey xmpKey(writeTags[ii]);
                        {
                            Exiv2::Value::AutoPtr value = Exiv2::Value::create(Exiv2::XmpProperties::propertyType(xmpKey));
                            value->read(writeValues[ii]);
                            while (ii < writeMetaDatumCountAct - 1 && !strcmp(writeTags[ii], writeTags[ii + 1])) {
                                // as long as it is the same tag
                                ii++;
                                value->read(writeValues[ii]);
                            }
                            xmpData.add(xmpKey, value.get());
                        }
                    }
                    else {
                        *errorText = strdup("Tag name does not start with Exif, Iptc or Xmp");
                        return exiv2StatusException;
                    }
                }
                else if (writeOptions[ii] == exiv2WriteOptionXaBag)
                {
#ifdef TRACING
                    if (tracingCount < TRACING) tracingLog[tracingCount++] = strdup("create Bag");
#endif
                    Exiv2::XmpTextValue tv;
                    tv.read("");
                    tv.setXmpArrayType(Exiv2::XmpValue::xaBag);
                    xmpData.add(Exiv2::XmpKey(writeTags[ii]), &tv); // Set the array type.
                }
                else if (writeOptions[ii] == exiv2WriteOptionXsStruct)
                {
#ifdef TRACING
                    if (tracingCount < TRACING) tracingLog[tracingCount++] = strdup("create Struct");
#endif
                    Exiv2::XmpTextValue tv;
                    tv.read("");
                    tv.setXmpStruct(Exiv2::XmpValue::xsStruct);
                    xmpData.add(Exiv2::XmpKey(writeTags[ii]), &tv); // Set the Struct type.
                }
                else if (writeOptions[ii] == exiv2WriteOptionXmpText)
                {
#ifdef TRACING
                    strcpy(tracingTemp, "exiv2WriteOptionXmpText add XmpText Value: ");
                    strcat(tracingTemp, writeTags[ii]);
                    if (tracingCount < TRACING) tracingLog[tracingCount++] = strdup(tracingTemp);
#endif
                    Exiv2::XmpKey xmpKey(writeTags[ii]);
                    Exiv2::Value::AutoPtr value = Exiv2::Value::create(Exiv2::XmpProperties::propertyType(xmpKey));
                    value->read(writeValues[ii]);
                    xmpData.add(xmpKey, value.get());
                }
                else
                {
                    *errorText = strdup("Invalid WriteOption");
                    return exiv2StatusException;
                }
            }
        }

        // if defined: set comment 
        if (comment != NULL) {
            image->setComment(comment);
        }

        // write image
        image->setExifData(exifData);
        image->writeMetadata();
#ifdef TRACING
        if (tracingCount < TRACING) tracingLog[tracingCount++] = strdup("exiv2writeImage finished successful");
#endif
        return 0;
    }
    catch (Exiv2::AnyError& e) {
        *errorText = strdup(e.what());
#ifdef TRACING
        if (tracingCount < TRACING) tracingLog[tracingCount++] = strdup("exiv2writeImage finished with exception");
#endif
        return exiv2StatusException;
    }
}

//-------------------------------------------------------------------------
// get log string by index
//-------------------------------------------------------------------------
extern "C" __declspec(dllexport) int __cdecl exiv2getLogString(int index, LPSTR * logString) {
    if (index < tracingCount) {
        *logString = strdup(tracingLog[index]);
        return 0;
    }
    else {
        return 1;
    }
}

//-------------------------------------------------------------------------
// check if tag is repeatable
//-------------------------------------------------------------------------
extern "C" __declspec(dllexport) bool __cdecl exiv2tagRepeatable(LPSTR tagName) {
    if (!strncmp(tagName, "Exif.", 5)) {
        return false;
    }
    else if (!strncmp(tagName, "Iptc.", 5)) {
        Exiv2::IptcKey iptcKey(tagName);
        return Exiv2::IptcDataSets::dataSetRepeatable(iptcKey.tag(), iptcKey.record());
    }
    else if (!strncmp(tagName, "Xmp.", 4)) {
        Exiv2::XmpKey xmpKey(tagName);
        if (Exiv2::XmpProperties::propertyType(xmpKey) == Exiv2::xmpBag ||
            Exiv2::XmpProperties::propertyType(xmpKey) == Exiv2::xmpSeq ||
            Exiv2::XmpProperties::propertyType(xmpKey) == Exiv2::xmpText) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        return true;
    }
}

//-------------------------------------------------------------------------
// return interpreted value of tag
//-------------------------------------------------------------------------
extern "C" __declspec(dllexport) void __cdecl exiv2getInterpretedValue(LPSTR tagName, LPSTR valueString, LPSTR * interpretedValue) {
    if (!strncmp(tagName, "Exif.", 5)) {
        Exiv2::ExifKey key(tagName);
        Exiv2::Value::AutoPtr value = Exiv2::Value::create(key.defaultTypeId());
        value->read(valueString);
        Exiv2::Exifdatum datum = Exiv2::Exifdatum(key, value.get());
        *interpretedValue = strdup(datum.print().c_str());
    }
    else if (!strncmp(tagName, "Iptc.", 5)) {
        Exiv2::IptcKey key(tagName);
        Exiv2::Value::AutoPtr value = Exiv2::Value::create(Exiv2::IptcDataSets::dataSetType(key.tag(), key.record()));
        value->read(valueString);
        Exiv2::Iptcdatum datum = Exiv2::Iptcdatum(key, value.get());
        *interpretedValue = strdup(datum.print().c_str());
    }
    else if (!strncmp(tagName, "Xmp.", 4)) {
        Exiv2::XmpKey key(tagName);
        Exiv2::Value::AutoPtr value = Exiv2::Value::create(Exiv2::XmpProperties::propertyType(key));
        value->read(valueString);
        Exiv2::Xmpdatum datum = Exiv2::Xmpdatum(key, value.get());
        *interpretedValue = strdup(datum.print().c_str());
    }
    else {
        // any other not yet known type, assumption: original and interpreted value do not differ
        *interpretedValue = strdup(valueString);
    }
}

//-------------------------------------------------------------------------
// return float value of tag
// in principal only for Exif types Rational and SRational, but implemented more general
//-------------------------------------------------------------------------
extern "C" __declspec(dllexport) float __cdecl exiv2floatValue(LPSTR tagName, LPSTR valueString) {
    float returnvalue;
    if (!strncmp(tagName, "Exif.", 5)) {
        Exiv2::ExifKey key(tagName);
        Exiv2::Value::AutoPtr value = Exiv2::Value::create(key.defaultTypeId());
        value->read(valueString);
        Exiv2::Exifdatum datum = Exiv2::Exifdatum(key, value.get());
        returnvalue = datum.toFloat();
    }
    else if (!strncmp(tagName, "Iptc.", 5)) {
        Exiv2::IptcKey key(tagName);
        Exiv2::Value::AutoPtr value = Exiv2::Value::create(Exiv2::IptcDataSets::dataSetType(key.tag(), key.record()));
        value->read(valueString);
        Exiv2::Iptcdatum datum = Exiv2::Iptcdatum(key, value.get());
        returnvalue = datum.toFloat();
    }
    else if (!strncmp(tagName, "Xmp.", 4)) {
        Exiv2::XmpKey key(tagName);
        Exiv2::Value::AutoPtr value = Exiv2::Value::create(Exiv2::XmpProperties::propertyType(key));
        value->read(valueString);
        Exiv2::Xmpdatum datum = Exiv2::Xmpdatum(key, value.get());
        returnvalue = datum.toFloat();
    }
    else {
        // any other not yet known type, set a fix value
        returnvalue = (float)-999.999;
    }
    return returnvalue;
}

//-------------------------------------------------------------------------
// get version
//-------------------------------------------------------------------------
extern "C" __declspec(dllexport) void __cdecl exiv2getVersion(LPSTR * version) {
    char tempVersion[32];
    strcpy(tempVersion, VERSION);
#ifdef _WIN64
    strcat(tempVersion, " - 64 Bit");
#else
    strcat(tempVersion, " - 32 Bit");
#endif
    * version = strdup(tempVersion);
}
