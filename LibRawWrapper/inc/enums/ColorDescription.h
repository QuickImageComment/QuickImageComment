#pragma once

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			public enum class ColorDescription : unsigned int
			{
				Invalid,
				RGBG = 'GBGR',
				RGBE = 'EBGR',
				GMCY = 'YCMG',
				GBTG = 'GTBG',
			};
		}
	}
}