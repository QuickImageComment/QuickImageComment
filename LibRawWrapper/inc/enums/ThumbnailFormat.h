#pragma once

#include <libraw.h>

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			public enum class ThumbnailFormat
			{
				/// <summary>
				/// Format unknown or thumbnail not yet read.
				/// </summary>
				Unknown = LIBRAW_THUMBNAIL_UNKNOWN,

				/// <summary>
				/// The thumbnail buffer contains a JPEG file (read from the RAW file "as is," without any manipulations performed on it).
				/// </summary>
				Jpeg = LIBRAW_THUMBNAIL_JPEG,

				/// <summary>
				/// The thumbnail buffer contains the gamma-adjusted RGB bitmap. 
				/// In this format, each pixel of the image is represented by a 8-bit RGB triplet.
				/// </summary>
				/// <remarks>
				/// For Kodak cameras, the gamma correction is performed with allowance for maximum values and the white balance is set in accordance with the camera settings.
				/// </remarks>
				Bitmap = LIBRAW_THUMBNAIL_BITMAP,

				/// <summary>
				/// The thumbnail buffer contains the gamma-adjusted 16-bit RGB bitmap.
				/// </summary>
				/// <remarks>
				/// To get this format instead of <see href="Bitmap">Bitmap</see> you need to set LIBRAW_PROCESSING_USE_PPM16_THUMBS in processing options.
				/// </remarks>
				Bitmap16 = LIBRAW_THUMBNAIL_BITMAP16,

				/// <summary>
				/// Data format is presently recognized upon opening of RAW file but not supported.
				/// </summary>
				Layer = LIBRAW_THUMBNAIL_LAYER,

				/// <summary>
				/// Data format is presently recognized upon opening of RAW file but not supported.
				/// </summary>
				Rollei = LIBRAW_THUMBNAIL_ROLLEI,

				/// <summary>
				/// The thumbnail buffer contains a H.265 data frame (read from RAW file as is, no manipulations performed on it).
				/// </summary>
				H265 = LIBRAW_THUMBNAIL_H265
			};
		}
	}
}