#pragma once

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Black level statistics.
			/// </summary>
			public value struct ChannelStatistic
			{
			public:
				ChannelStatistic(int sum, int count)
				{
					Sum = sum;
					Count = count;
				}

				/// <summary>
				/// Gets or sets the sum of pixel values.
				/// </summary>
				property int Sum;

				/// <summary>
				/// Gets or sets the pixel count.
				/// </summary>
				property int Count;

				/// <summary>
				/// Gets the average pixel value.
				/// </summary>
				property double Average
				{
					double get() { return Sum / (double)Count; }
				}
			};
		}
	}
}