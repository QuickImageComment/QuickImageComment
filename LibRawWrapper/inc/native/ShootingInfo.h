#pragma once

#include <libraw.h>
#include "LibRawWrapper.h"

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			public ref class ShootingInfo
			{
			public:
				property short DriveMode { short get() { return m_info->DriveMode; } }
				property short FocusMode { short get() { return m_info->FocusMode; } }
				property short MeteringMode { short get() { return m_info->MeteringMode; } }
				property short AFPoint { short get() { return m_info->AFPoint; } }
				property short ExposureMode { short get() { return m_info->ExposureMode; } }
				property short ExposureProgram { short get() { return m_info->ExposureProgram; } }
				property short ImageStabilization { short get() { return m_info->ImageStabilization; } }

				property String^ BodySerial { String^ get() { return FixedCharToString(m_info->BodySerial); } }
				property String^ InternalBodySerial { String^ get() { return FixedCharToString(m_info->InternalBodySerial); } }

			internal:
				ShootingInfo(libraw_shootinginfo_t* info) : m_info(info) { }

			private:
				libraw_shootinginfo_t* m_info;
			};
		}
	}
}