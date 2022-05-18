#pragma once

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Flip image.
			/// </summary>
			public enum class Interpolation : int
			{
				/// <summary>
				/// High-speed, low-quality bilinear interpolation.
				/// </summary>
				Linear = 0,

				/// <summary>
				/// Variable Number of Gradients (VNG) interpolation.
				/// </summary>
				VNG = 1,

				/// <summary>
				/// Patterned Pixel Grouping (PPG) interpolation.
				/// </summary>
				PPG = 2,

				/// <summary>
				/// Adaptive Homogeneity-Directed Demosaicing.
				/// </summary>
				AHD = 3,

				/// <summary>
				/// DCB demosaicing by Jacek Gozdz.
				/// </summary>
				DCB = 4,

				/// <summary>
				/// DHT Demosaic by Anton Petrusevich. 
				/// </summary>
				DHT = 11,

				/// <summary>
				/// Modified AHD Demosaic by Anton Petrusevich.
				/// </summary>
				ModifiedAHD = 12
			};
		}
	}
}