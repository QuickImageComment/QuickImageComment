#pragma once

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Highlight mode.
			/// </summary>
			public enum class HighlightMode
			{
				/// <summary>
				/// Clip all highlights to solid white (default). 
				/// </summary>
				Clip = 0,

				/// <summary>
				/// Leave highlights unclipped in various shades of pink.
				/// </summary>		
				Unclip = 1,

				/// <summary>
				/// Blend clipped and unclipped values together for a gradual fade to white.
				/// </summary>
				Blend = 2,

				/// <summary>
				/// Reconstruct highlights.
				/// </summary>
				Rebuild = 3,
			};
		}
	}
}