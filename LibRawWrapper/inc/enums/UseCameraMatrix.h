#pragma once

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Embedded color profile usage.
			/// </summary>
			public enum class UseCameraMatrix
			{
				/// <summary>
				/// Do not use embedded color profile.
				/// </summary>
				Never = 0,

				/// <summary>
				/// Use embedded color profile (if present) for DNG files (always); for other files only if <see href="OutputParameters::UseCameraWhiteBalance">UseCameraWhiteBalance</see> is set (default).
				/// </summary>
				WhiteBalance = 1,

				/// <summary>
				/// Use embedded color data (if present) regardless of whitebalance setting.
				/// </summary>
				Always = 3
			};
		}
	}
}