#pragma once

#include <libraw.h>

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Stages of file processing.
			/// </summary>
			[System::Flags]
			public enum class Progress : int
			{
				/// <summary>
				/// Object just created, no processing carried out.
				/// </summary>
				Start = LIBRAW_PROGRESS_START,

				/// <summary>
				/// File to be processed has been opened.
				/// </summary>
				Open = LIBRAW_PROGRESS_OPEN,

				/// <summary>
				/// Data identification performed, format recognized, metadata extracted.
				/// </summary>
				Identify = LIBRAW_PROGRESS_IDENTIFY,

				/// <summary>
				/// Data sizes adjusted (for files that require such adjustment, namely, certain files from Kodak cameras).
				/// </summary>
				SizeAdjust = LIBRAW_PROGRESS_SIZE_ADJUST,

				// The following flags are set during usage of image processingthat has been taken from dcraw. 

				/// <summary>
				/// RAW data loaded.
				/// </summary>
				LoadRaw = LIBRAW_PROGRESS_LOAD_RAW,

				// The following flags are set during usage of image processing that has been taken from dcraw.

				/// <summary>
				/// imgdata.image array allocated and filled with data.
				/// </summary>
				Raw2Image = LIBRAW_PROGRESS_RAW2_IMAGE,

				/// <summary>
				/// Zero values removed for cameras that require such removal (Panasonic cameras).
				/// </summary>
				RemoveZeroes = LIBRAW_PROGRESS_REMOVE_ZEROES,

				/// <summary>
				/// Bad (dead) pixels removed.
				/// </summary>
				BadPixels = LIBRAW_PROGRESS_BAD_PIXELS,

				/// <summary>
				/// Dark frame subtracted from RAW data.
				/// </summary>
				DarkFrame = LIBRAW_PROGRESS_DARK_FRAME,

				/// <summary>
				/// Interpolation for cameras with a Foveon sensor performed.
				/// </summary>
				FoveonInterpolate = LIBRAW_PROGRESS_FOVEON_INTERPOLATE,

				/// <summary>
				/// White balance performed.
				/// </summary>
				ScaleColors = LIBRAW_PROGRESS_SCALE_COLORS,

				/// <summary>
				/// Image size reduction (for the half_size mode) performed, as well ascopying of 2nd green channel to the 1st one in points where the second channel is present and the first one is absent.
				/// </summary>
				PreInterpolate = LIBRAW_PROGRESS_PRE_INTERPOLATE,

				/// <summary>
				/// Interpolation (debayer) performed.
				/// </summary>
				Interpolate = LIBRAW_PROGRESS_INTERPOLATE,

				/// <summary>
				/// Averaging of green channels performed.
				/// </summary>
				MixGreen = LIBRAW_PROGRESS_MIX_GREEN,

				/// <summary>
				/// Median filtration performed.
				/// </summary>
				MedianFilter = LIBRAW_PROGRESS_MEDIAN_FILTER,

				/// <summary>
				/// Work with highlights performed.
				/// </summary>
				Highlights = LIBRAW_PROGRESS_HIGHLIGHTS,

				/// <summary>
				/// For images from Fuji cameras, rotation performed (or adjust_sizes_info_only() called).
				/// </summary>
				FujiRotate = LIBRAW_PROGRESS_FUJI_ROTATE,

				/// <summary>
				/// Dimensions recalculated for images shot with a rotated camera (sizes.iwidth/sizes.iheight swapped).
				/// </summary>
				Flip = LIBRAW_PROGRESS_FLIP,

				ApplyProfile = LIBRAW_PROGRESS_APPLY_PROFILE,

				/// <summary>
				/// Conversion into output RGB space performed.
				/// </summary>
				ConvertRgb = LIBRAW_PROGRESS_CONVERT_RGB,

				/// <summary>
				/// Image dimensions changed for cameras with non-square pixels.
				/// </summary>
				Stretch = LIBRAW_PROGRESS_STRETCH,

				// The following flags are set during loading of thumbnails. 

				/// <summary>
				/// Thumbnail data have been loaded (for Kodak cameras, the necessary conversions have also been made).
				/// </summary>
				ThumbLoad = LIBRAW_PROGRESS_THUMB_LOAD
			};

		}
	}
}