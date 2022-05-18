#pragma once

#include <libraw.h>

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Nonstandard Situations (Warnings) during RAW Data Processing.
			/// </summary>
			[Flags]
			public enum class Warnings : int
			{
				/// <summary>
				/// None.
				/// </summary>
				None = 0,

				/// <summary>
				/// Postprocessing must use white balance of the camera but this balance is not suitable for use.
				/// </summary>
				BadCameraWhiteBalance = LIBRAW_WARN_BAD_CAMERA_WB,

				/// <summary>
				/// Only for cameras where the metadata are taken from an external JPEG file: metadata extraction has failed.
				/// </summary>
				NoMetadata = LIBRAW_WARN_NO_METADATA,

				/// <summary>
				/// Only for P&amp;S Kodak cameras: data in JPEG format. At the same time, open_file() will return LIBRAW_FILE_UNSUPPORTED.
				/// </summary>
				NoJpegLib = LIBRAW_WARN_NO_JPEGLIB,

				/// <summary>
				/// Caller set embedded input profile use, but no such profile exists in RAW.
				/// </summary>
				NoEmbeddedProfile = LIBRAW_WARN_NO_EMBEDDED_PROFILE,

				/// <summary>
				/// Error when opening input profile ICC file.
				/// </summary>
				NoInputProfile = LIBRAW_WARN_NO_INPUT_PROFILE,

				/// <summary>
				/// Error when opening output profile ICC file.
				/// </summary>
				BadOutputProfile = LIBRAW_WARN_BAD_OUTPUT_PROFILE,

				/// <summary>
				/// 
				/// </summary>
				NoBadPixelMap = LIBRAW_WARN_NO_BADPIXELMAP,

				/// <summary>
				/// Error when opening bad pixels map file
				/// </summary>
				BadDarkFrameFile = LIBRAW_WARN_BAD_DARKFRAME_FILE,

				/// <summary>
				/// Error when opening dark frame file.
				/// </summary>
				BadDarkFrameDimensions = LIBRAW_WARN_BAD_DARKFRAME_DIM,

#ifdef LIBRAW_OLD_VIDEO_SUPPORT
				NoJasper = LIBRAW_WARN_NO_JASPER,
#endif

				/// <summary>
				/// Dark frame file either differs in dimensions from RAW-file processed, or have wrong format.
				/// </summary>
				/// <remarks>
				/// Dark frame should be in 16-bit PGM format (one can generate it using simple_dcraw -4 -D).
				/// </remarks>
				RawSpeedProblem = LIBRAW_WARN_RAWSPEED_PROBLEM,

				/// <summary>
				/// Problems detected in RawSpeed decompressor. The image data processedby LibRaw own decoder.
				/// </summary>
				RawSpeedUnsupported = LIBRAW_WARN_RAWSPEED_UNSUPPORTED,

				/// <summary>
				/// Not warning, but information. The file was decoded by RawSpeed.
				/// </summary>
				RawSpeedProcessed = LIBRAW_WARN_RAWSPEED_PROCESSED,

				/// <summary>
				/// Incorrect/unsupported user_qual was set, AHD demosaic used instead.
				/// </summary>
				FallbackToAhd = LIBRAW_WARN_FALLBACK_TO_AHD,

				/// <summary>
				/// Not really a warning, but flag that fuji parser was used.
				/// </summary>
				ParseFujiProcessed = LIBRAW_WARN_PARSEFUJI_PROCESSED,

				/// <summary>
				/// Not really a warning: image was decoded by DNG SDK.
				/// </summary>
				DngSdkProcessed = LIBRAW_WARN_DNGSDK_PROCESSED,

				/// <summary>
				/// DNG sub0images was reordered.
				/// </summary>
				DngImagesReordered = LIBRAW_WARN_DNG_IMAGES_REORDERED,

				/// <summary>
				/// DNG Stage2 conversion was performed.
				/// </summary>
				DngStage2Applied = LIBRAW_WARN_DNG_STAGE2_APPLIED,

				/// <summary>
				/// DNG Stage3 conversion was performed.
				/// </summary>
				DngStage3Applied = LIBRAW_WARN_DNG_STAGE3_APPLIED
			};
		}
	}
}