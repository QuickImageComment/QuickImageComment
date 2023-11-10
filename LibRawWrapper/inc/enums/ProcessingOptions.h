#pragma once

#include <libraw.h>

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
				PentaxPsAllFrames = LIBRAW_RAWOPTIONS_PENTAX_PS_ALLFRAMES,

				/// <summary>
				/// Convert FP data to 16-bit integer.
				/// </summary>
				ConvertFloatToInt = LIBRAW_RAWOPTIONS_CONVERTFLOAT_TO_INT,

				ArqSkipChannelSwap = LIBRAW_RAWOPTIONS_ARQ_SKIP_CHANNEL_SWAP,
				NoRotateForKodakThumbnails = LIBRAW_RAWOPTIONS_NO_ROTATE_FOR_KODAK_THUMBNAILS,

				/// <summary>
				/// Enable 16-bit PPM thumbnails
				/// </summary>
				UsePpm16Thumbnails = LIBRAW_RAWOPTIONS_USE_PPM16_THUMBS,

				/// <summary>
				/// Skip DNG illuminant check when parsing DNG color data (use for compatibility w/older LibRaw versions).
				/// </summary>
				DontCheckDngIlluminant = LIBRAW_RAWOPTIONS_DONT_CHECK_DNG_ILLUMINANT,

				/// <summary>
				/// Do not copy data extracted by Adobe DNG SDK into separate buffer, but use DNG SDK buffer as is.
				/// </summary>
				DngSdkZeroCopy = LIBRAW_RAWOPTIONS_DNGSDK_ZEROCOPY,

				/// <summary>
				/// By default, LibRaw assigns bayer pattern for Monochrome TIFF files (e.g. images from Kodak 760).
				/// This does not work as expected if input file is really monochrome (e.g. scan from Imacon X1 in monochrome mode).
				/// This flag will force monochrome mode for TIFF RAWs w/o bayer filter tags (so, it will break old Kodak processing). 
				/// It is better to make it settable via user interaction.
				/// </summary>
				ZeroFiltersForMonochromeTiffs = LIBRAW_RAWOPTIONS_ZEROFILTERS_FOR_MONOCHROMETIFFS,

				/// <summary>
				/// If set, LibRaw will add Enhanced DNG frame (NewSubfileType == 16) to the list of available frames.
				/// </summary>
				DngAddEnhanced = LIBRAW_RAWOPTIONS_DNG_ADD_ENHANCED,

				/// <summary>
				/// If set, LibRaw will add previews (NewSubfileType == 1) to the frames list.
				/// </summary>
				DngAddPreviews = LIBRAW_RAWOPTIONS_DNG_ADD_PREVIEWS,

				/// <summary>
				/// By default, DNG frames are not reordered and are available in same order as in DNG (LibRaw traverses IFD/Sub-IFD trees in deep-first order).
				/// This bit will prioritize the largest image.
				/// </summary>
				DngPreferLargestImage = LIBRAW_RAWOPTIONS_DNG_PREFER_LARGEST_IMAGE,

				/// <summary>
				/// Request DNG Stage2 processing (by DNG SDK).
				/// </summary>
				DngStage2 = LIBRAW_RAWOPTIONS_DNG_STAGE2,

				/// <summary>
				/// Request DNG Stage3 processing.
				/// </summary>
				DngStage3 = LIBRAW_RAWOPTIONS_DNG_STAGE3,

				/// <summary>
				/// By default, if image size parsed by DNG SDK does not match image dimensions parsedby LibRaw, processing will stop with LIBRAW_DATA_ERROR code. 
				/// This flags allows size change in LibRaw::unpack() stage.
				/// </summary>
				DngAllowSizeChange = LIBRAW_RAWOPTIONS_DNG_ALLOWSIZECHANGE,

				/// <summary>
				/// By default, for DNG images with different per-channel maximums WB adjustment procedure is performed.
				/// This flag disables such adjustment.
				/// </summary>
				DngDisableWhiteBalanceAdjust = LIBRAW_RAWOPTIONS_DNG_DISABLEWBADJUST,

				/// <summary>
				/// If set (default is not), and when applicable, color.cam_mul[] and color.WB_Coeffs/WBCT_Coeffs will contain WB settings for a non-standard workflow.
				/// </summary>
				/// <remarks>
				/// Right now only Sony DSC-F828 is affected: camera-recorded white balance can't be directly applied to raw data because WB is for RGB, while raw data is RGBE.
				/// </remarks>
				ProvideNonstandardWhiteBalance = LIBRAW_RAWOPTIONS_PROVIDE_NONSTANDARD_WB,
				
				/// <summary>
				/// If set (default is not), LibRaw::dcraw_process() will fallback to daylight WB (excluding some very specific cases like Canon D30).
				/// This is how LibRaw 0.19 (and older) works.
				/// If not set, LibRaw::dcraw_process() will fallback to calculated auto WB if camera WB is requested, but appropriate white balance was not found in metadata.
				/// </summary>
				CameraWhiteBalanceFallbackToDaylight = LIBRAW_RAWOPTIONS_CAMERAWB_FALLBACK_TO_DAYLIGHT,
				
				CheckThumbnailsKnownVendors = LIBRAW_RAWOPTIONS_CHECK_THUMBNAILS_KNOWN_VENDORS,
				CheckThumbnailsAllVendors = LIBRAW_RAWOPTIONS_CHECK_THUMBNAILS_ALL_VENDORS,

				/// <summary>
				/// Stage2 processing will be performed only if OpcodeList2 tags are present in the input DNG file.
				/// </summary>
				/// <remarks>
				/// <see cref="DngStage2" /> will force Stage2 processing if set (regardless of input file tags).
				/// </remaks>
				DngStage2IfPresent = LIBRAW_RAWOPTIONS_DNG_STAGE2_IFPRESENT,
				
				/// <summary>
				/// Stage3 processing will be performed only if OpcodeList3 tags are present in the input DNG file.
				/// </summary>
				/// <remarks>
				/// <see cref="DngStage2" /> will force Stage3 processing if set (regardless of input file tags).
				/// </remaks>
				DngStage3IfPresent = LIBRAW_RAWOPTIONS_DNG_STAGE3_IFPRESENT,
				
				/// <summary>
				/// DNG Transparency Masks will be extracted (if selected via <see cref="RawParameters.ShotSelect" />).
				/// </summary>
				DngAddMasks = LIBRAW_RAWOPTIONS_DNG_ADD_MASKS,

				/// <summary>
				/// Image orientation is set based on TIFF/IFD0:Orientation tag, makernotes orientation data is ignored.
				/// </summary>
				CanonIgnoreMakerNotesRotation = LIBRAW_RAWOPTIONS_CANON_IGNORE_MAKERNOTES_ROTATION
			};
		}
	}
}