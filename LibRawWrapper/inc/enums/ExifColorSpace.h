#pragma once

#include <libraw.h>

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// EXIF color space.
			/// </summary>
			public enum class ExifColorSpace : int
			{
				NotFound = LIBRAW_COLORSPACE_NotFound,

				/// <summary>
				/// sRGB.
				/// </summary>
				sRgb = LIBRAW_COLORSPACE_sRGB,

				/// <summary>
				/// Adobe RGB.
				/// </summary>
				AdobeRgb = LIBRAW_COLORSPACE_AdobeRGB,

				WideGamutRgb = LIBRAW_COLORSPACE_WideGamutRGB,
				ProPhotoRgb = LIBRAW_COLORSPACE_ProPhotoRGB,
				Icc = LIBRAW_COLORSPACE_ICC,
				Uncalibrated = LIBRAW_COLORSPACE_Uncalibrated,
				CameraLinearUniWB = LIBRAW_COLORSPACE_CameraLinearUniWB,
				CameraLinear = LIBRAW_COLORSPACE_CameraLinear,
				CameraGammaUniWB = LIBRAW_COLORSPACE_CameraGammaUniWB,
				CameraGamma = LIBRAW_COLORSPACE_CameraGamma,
				MonochromeLinear = LIBRAW_COLORSPACE_MonochromeLinear,
				MonochromeGamma = LIBRAW_COLORSPACE_MonochromeGamma,
				Unknown = LIBRAW_COLORSPACE_Unknown
			};
		}
	}
}