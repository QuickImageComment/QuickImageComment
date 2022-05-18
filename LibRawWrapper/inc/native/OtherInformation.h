#pragma once

#include <libraw.h>
#include "LibRawWrapper.h"
#include "GpsInfo.h"

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Data structure for information purposes.
			/// </summary>
			public ref class OtherInformation
			{
			public:
				/// <summary>
				/// Gets ISO sensitivity.
				/// </summary>
				property float IsoSpeed
				{
					float get() { return m_imgother->iso_speed; }
				}

				/// <summary>
				/// Gets shutter speed.
				/// </summary>
				property TimeSpan Shutter
				{
					TimeSpan get() { return TimeSpan::FromSeconds(m_imgother->shutter); }
				}

				/// <summary>
				/// Gets aperture.
				/// </summary>
				property float Aperture
				{
					float get() { return m_imgother->aperture; }
				}

				/// <summary>
				/// Gets focal length.
				/// </summary>
				property float FocalLength
				{
					float get() { return m_imgother->focal_len; }
				}

				/// <summary>
				/// Gets date of shooting.
				/// </summary>
				property DateTime Timestamp
				{
					DateTime get() { return DateTimeOffset::FromUnixTimeSeconds(m_imgother->timestamp).DateTime; }
				}

				/// <summary>
				/// Gets serial number of image.
				/// </summary>
				property int ShotOrder
				{
					int get() { return m_imgother->shot_order; }
				}

				/// <summary>
				/// Gets GPS data (unparsed block, to write to output as is).
				/// </summary>
				/// <returns></returns>
				array<byte>^ GetGpsData()
				{
					return FixedToArray((byte*)&m_imgother->gpsdata, sizeof(m_imgother->gpsdata));
				}

				/// <summary>
				/// Gets GPS data buffer.
				/// </summary>
				property IntPtr GpsDataBuffer
				{
					IntPtr get() { return (IntPtr)m_imgother->gpsdata; }
				}

				/// <summary>
				/// Gets the length (in bytes) of GPS data buffer.
				/// </summary>
				property int GpsDataBufferLength
				{
					int get() { return sizeof(m_imgother->gpsdata); }
				}

				/// <summary>
				/// Gets the parsed GPS-data: longitude/latitude/altitude and time stamp.
				/// </summary>
				property GpsInfo^ ParsedGps
				{
					GpsInfo^ get()
					{
						if (!m_imgother->parsed_gps.gpsparsed)
							return nullptr;

						return gcnew GpsInfo(&m_imgother->parsed_gps);
					}
				}

				/// <summary>
				/// Gets image description.
				/// </summary>
				property String^ Description
				{
					String^ get() { return FixedCharToString(m_imgother->desc); }
				}

				/// <summary>
				/// Gets author of image.
				/// </summary>
				property String^ Artist
				{
					String^ get() { return FixedCharToString(m_imgother->artist); }
				}

				property array<float>^ AnalogBalance
				{
					array<float>^ get() { return FixedToArray(m_imgother->analogbalance, sizeof(m_imgother->analogbalance)); }
				}

			internal:
				OtherInformation(libraw_imgother_t* imgother) : m_imgother(imgother) { }

			private:
				libraw_imgother_t* m_imgother;
			};
		}
	}
}