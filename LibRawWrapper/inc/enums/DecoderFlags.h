#pragma once

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			[Flags]
			public enum class DecoderFlags : unsigned int
			{
				None = 0,

				/// <summary>
				/// This flag is set if decoder uses RAW tone curve and curve data may be modified before call to decoder (i.e. curve values are not read or calculated within decoder).
				/// </summary>
				HasCurve = 1 << 4,

				/// <summary>
				/// This flag is set if file format is Sony ARW2.3, so sony_arw2_options is applicable.
				/// </summary>
				SonyArw2 = 1 << 5,

				/// <summary>
				/// This flag is set if file format is (possibly) supported by RawSpeed library, so unpack() will try to use it.
				/// </summary>
				TryRawSpeed = 1 << 6,

				/// <summary>
				/// Decoder allocates data, no need to pass allocated memory to decoder.
				/// </summary>
				OwnAllocations = 1 << 7,

				/// <summary>
				/// Do not use automated maximum calculation for this data format.
				/// </summary>
				FixedMaxC = 1 << 8,

				/// <summary>
				/// Internal flag, special to adobe DNG decoder.
				/// </summary>
				AdobeCopyPixel = 1 << 9,

				/// <summary>
				/// Special flag uset for 4-channel (legacy) decoders with black/masked areas.
				/// </summary>
				LegacyWithMargins = 1 << 10,

				/// <summary>
				/// 3-component full-color data (not usual 4-component).
				/// </summary>
				ThreeChannels = 1 << 11,

				Sinar4shot = 1 << 11,
				FlatData = 1 << 12,
				FlatBg2Swapped = 1 << 13,
				NotSet = 1 << 15
			};
		}
	}
}