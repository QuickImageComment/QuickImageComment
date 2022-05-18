#pragma once

using namespace System::Diagnostics;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			generic<typename T>
				[DebuggerDisplay(L"R={R}, G1={G1}, B={B}, G2={G2}")]
				public value struct Pixel4
				{
				public:
					property T R;
					property T G1;
					property T B;
					property T G2;

					Pixel4(T r, T g1, T b, T g2)
					{
						R = r;
						G1 = g1;
						B = b;
						G2 = g2;
					}
				};
		}
	}
}