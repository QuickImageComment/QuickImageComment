#pragma once

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			[Flags]
			public enum class DngProcessing
			{
				None = 0,
				Float = 1,
				Linear = 2,
				Deflate = 4,
				XTrans = 8,
				Other = 16,
				Bit8 = 32,
				LargeRange = 64,

				All = Float | Linear | Deflate | XTrans | Bit8 | Other | LargeRange,
				Default = Float | Linear | Deflate | Bit8
			};
		}
	}
}