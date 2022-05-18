#pragma once

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			[Flags]
			public enum class ProcessingSpecials : unsigned int
			{
				/// <summary>
				/// No options specified.
				/// </summary>
				None = 0,

				/// <summary>
				///decode only base pixels,leave delta pixels as zero; 
				/// </summary>
				SonyArw2Baseonly = 1,

				/// <summary>
				/// decode only delta pixelswith base pixels zeroed;
				/// </summary>
				SonyArw2Deltaonly = 1 << 1,

				/// <summary>
				/// decode delta pixels,do not add base value;
				/// </summary>
				SonyArw2DeltaZeroBase = 1 << 2,

				/// <summary>
				/// show possible posterization areas;
				/// </summary>
				SonyArw2DeltaToValue = 1 << 3,

				SonyArw2Allflags = SonyArw2Baseonly | SonyArw2Deltaonly | SonyArw2DeltaZeroBase | SonyArw2DeltaToValue,

				/// <summary>
				/// Turn off R/G channels interpolation.
				/// </summary>
				NoDP2QInterpolateRG = 1 << 4,

				/// <summary>
				/// Turn off data interpolation of low-sensitivity (AF or overexposure control).
				/// </summary>
				NoDP2QInterpolateAF = 1 << 5,

				/// <summary>
				/// Disable YCC to RGB conversion.
				/// </summary>
				SrawNoRgb = 1 << 6,

				/// <summary>
				/// Disable missing color values interpolation.
				/// </summary>
				SrawNoInterpolate = 1 << 7
			};

		}
	}
}