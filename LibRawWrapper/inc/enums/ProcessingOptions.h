#pragma once

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			[Flags]
			public enum class ProcessingOptions : unsigned int
			{
				/// <summary>
				/// No options specified.
				/// </summary>
				None = 0,

				/// <summary>
				/// Merge all frames for Pentax 4-shot files.
				/// </summary>
				PentaxPsAllFrames = 1,

				/// <summary>
				/// Convert FP data to 16-bit integer.
				/// </summary>
				ConvertFloatToInt = 1 << 1,

				ArqSkipChannelSwap = 1 << 2,
				NoRotateForKodakThumbnails = 1 << 3,

				/// <summary>
				/// use DNG DefaultCrop* tags for cropping
				/// </summary>
				UseDngDefaultCrop = 1 << 4,

				/// <summary>
				/// Enable 16-bit PPM thumbnails
				/// </summary>
				UsePpm16Thumbnails = 1 << 5,

				/// <summary>
				/// Skip DNG illuminant check when parsing DNG color data (use for compatibility w/older LibRaw versions).
				/// </summary>
				DontCheckDngIlluminant = 1 << 6,

				/// <summary>
				/// Do not copy data extracted by Adobe DNG SDK into separate buffer, but use DNG SDK buffer as is.
				/// </summary>
				DngSdkZeroCopy = 1 << 7,

				/// <summary>
				/// By default, LibRaw assigns bayer pattern for Monochrome TIFF files (e.g. images from Kodak 760).
				/// This does not work as expected if input file is really monochrome (e.g. scan from Imacon X1 in monochrome mode).
				/// This flag will force monochrome mode for TIFF RAWs w/o bayer filter tags (so, it will break old Kodak processing). 
				/// It is better to make it settable via user interaction.
				/// </summary>
				ZeroFiltersForMonochromeTiffs = 1 << 8,

				/// <summary>
				/// If set, LibRaw will add Enhanced DNG frame (NewSubfileType == 16) to the list of available frames.
				/// </summary>
				DngAddEnhanced = 1 << 9,

				/// <summary>
				/// If set, LibRaw will add previews (NewSubfileType == 1) to the frames list.
				/// </summary>
				DngAddPreviews = 1 << 10,

				/// <summary>
				/// By default, DNG frames are not reordered and are available in same order as in DNG (LibRaw traverses IFD/Sub-IFD trees in deep-first order).
				/// This bit will prioritize the largest image.
				/// </summary>
				DngPreferLargestImage = 1 << 11,

				/// <summary>
				/// Request DNG Stage2 processing (by DNG SDK).
				/// </summary>
				DngStage2 = 1 << 12,

				/// <summary>
				/// Request DNG Stage3 processing.
				/// </summary>
				DngStage3 = 1 << 13,

				/// <summary>
				/// By default, if image size parsed by DNG SDK does not match image dimensions parsedby LibRaw, processing will stop with LIBRAW_DATA_ERROR code. 
				/// This flags allows size change in LibRaw::unpack() stage.
				/// </summary>
				DngAllowSizeChange = 1 << 14,

				/// <summary>
				/// By default, for DNG images with different per-channel maximums WB adjustment procedure is performed.
				/// This flag disables such adjustment.
				/// </summary>
				DngDisableWhiteBalanceAdjust = 1 << 15,

				ProvideNonstandardWhiteBalance = 1 << 16,
				CameraWhiteBalanceFallbackToDaylight = 1 << 17,
				CheckThumbnailsKnownVendors = 1 << 18,
				CheckThumbnailsAllVendors = 1 << 19
			};
		}
	}
}