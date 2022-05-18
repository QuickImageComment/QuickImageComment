#pragma once

#include "LibRawWrapper.h"

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Per-channel black level correction.
			/// </summary>
			public ref class PerChannelBlackCorrection
			{
			public:
				/// <summary>
				/// Gets or sets the per-channel correction.
				/// </summary>
				property array<int>^ Correction;

				/// <summary>
				/// Gets or sets the black level pattern block width.
				/// </summary>
				property int BlockHeight;

				/// <summary>
				/// Gets or sets the black level pattern block width.
				/// </summary>
				property int BlockWidth;

				/// <summary>
				/// Gets or sets the correction values.
				/// </summary>
				property array<int, 2>^ Block;

			internal:
				PerChannelBlackCorrection(unsigned int* cblack)
				{
					Correction = gcnew array<int>(4) { (int)cblack[0], (int)cblack[1], (int)cblack[2], (int)cblack[3] };
					BlockHeight = (int)cblack[4];
					BlockWidth = (int)cblack[5];
					Block = FixedToArray((int*)&cblack[6], BlockHeight, BlockWidth);
				}
			};
		}
	}
}