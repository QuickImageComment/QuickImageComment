#pragma once

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Flip image.
			/// </summary>
			public enum class Flip : int
			{
				/// <summary>
				/// All flipping disabled.
				/// </summary>
				None = 0,

				/// <summary>
				/// Rotate 180 degrees.
				/// </summary>
				Rotate180 = 3,

				/// <summary>
				/// Rotate 90 degrees counter clockwise.
				/// </summary>
				Rotate90CounterClockwise = 5,

				/// <summary>
				/// Rotate 90 degrees clockwise.
				/// </summary>
				Rotate90Clockwise = 6,
			};
		}
	}
}