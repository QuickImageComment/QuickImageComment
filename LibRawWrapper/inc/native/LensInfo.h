#pragma once
#include <libraw.h>
#include <vcclr.h>
#include "LibRawWrapper.h"

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Contains parameters extracted from Makernotes EXIF fields, to help identify which lens was mounted on the camera.
			/// </summary>
			public ref class LensInfo
			{
			public:
				/// <summary>
				/// Gets the minimum focal length for the lens mounted on the camera.
				/// </summary>
				property float MinimumFocalLength { float get() { return m_lensinfo->MinFocal; } }

				/// <summary>
				/// Gets the maximum focal length for the lens mounted on the camera.
				/// </summary>
				property float MaximumFocalLength { float get() { return m_lensinfo->MaxFocal; } }

				/// <summary>
				/// Gets the maximum aperture available at minimal focal length.
				/// </summary>
				property float MaximumApertureAtMinimumFocalLength { float get() { return m_lensinfo->MaxAp4MinFocal; } }

				/// <summary>
				/// Gets the maximum aperture available at maximal focal length.
				/// </summary>
				property float MaximumApertureAtMaximumFocalLength { float get() { return m_lensinfo->MaxAp4MaxFocal; } }

				/// <summary>
				/// Gets the lens manufacturer name.
				/// </summary>
				property String^ LensMake
				{
					String^ get() { return FixedCharToString(m_lensinfo->LensMake); }
				}

				/// <summary>
				/// Gets the Lens name as recorded in EXIF.
				/// </summary>
				property String^ Lens
				{
					String^ get() { return FixedCharToString(m_lensinfo->Lens); }
				}

				property String^ LensSerial
				{
					String^ get() { return FixedCharToString(m_lensinfo->LensSerial); }
				}

				property String^ InternalLensSerial
				{
					String^ get() { return FixedCharToString(m_lensinfo->InternalLensSerial); }
				}

				/// <summary>
				/// Gets the FocalLengthIn35mmFilm in EXIF standard, tag 0xa405.
				/// </summary>
				property ushort FocalLengthIn35mmFormat { ushort get() { return m_lensinfo->FocalLengthIn35mmFormat; } }

				/// <summary>
				/// Gets the value derived from EXIF tag 0x9205.
				/// </summary>
				property float MaximumAperture { float get() { return m_lensinfo->EXIF_MaxAp; } }

				// TODO: libraw_nikonlens_t
				// TODO: libraw_dnglens_t
				// TODO: libraw_makernotes_lens_t

			internal:
				LensInfo(libraw_lensinfo_t* lensinfo) : m_lensinfo(lensinfo) { }

			private:
				libraw_lensinfo_t* m_lensinfo;
			};
		}
	}
}