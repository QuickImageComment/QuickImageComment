#pragma once

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Color space.
			/// </summary>
			public enum class OutputColorspace
			{
				/// <summary>
				/// Camera-specific space.
				/// </summary>
				Raw,

				/// <summary>
				/// sRGB.
				/// </summary>
				sRGB,

				/// <summary>
				/// Adobe RGB (1998).
				/// </summary>
				AdobeRGB,

				/// <summary>
				/// WideGamut 65.
				/// </summary>
				WideGamut,

				/// <summary>
				/// ProPhoto D65.
				/// </summary>
				ProPhoto,

				/// <summary>
				/// XYZ.
				/// </summary>
				XYZ,

				/// <summary>
				/// ACES.
				/// </summary>
				ACES,

				/// <summary>
				/// DCI-P3 D65.
				/// </summary>
				DciP3,

				/// <summary>
				/// Rec. 2020.
				/// </summary>
				Rec2020
			};
		}
	}
}