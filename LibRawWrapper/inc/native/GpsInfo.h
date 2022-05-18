#pragma once

#include <libraw.h>

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			public ref class GpsInfo
			{
			public:
				property float LatitudeDegrees;
				property float LatitudeMinutes;
				property float LatitudeSeconds;
				property Char LatitudeRef;

				property float LongitudeDegrees;
				property float LongitudeMinutes;
				property float LongitudeSeconds;
				property Char LongitudeRef;

				property float Altitude;
				property bool IsBelowSeaLevel;

				property TimeSpan Timestamp;
				property Char Status;

			internal:
				GpsInfo(libraw_gps_info_t* parsed_info)
				{
					LatitudeDegrees = parsed_info->latitude[0];
					LatitudeMinutes = parsed_info->latitude[1];
					LatitudeSeconds = parsed_info->latitude[2];
					LatitudeRef = parsed_info->latref;

					LongitudeDegrees = parsed_info->longitude[0];
					LongitudeMinutes = parsed_info->longitude[1];
					LongitudeSeconds = parsed_info->longitude[2];
					LongitudeRef = parsed_info->longref;

					Altitude = parsed_info->altitude;
					IsBelowSeaLevel = parsed_info->altref;

					Timestamp = TimeSpan((int)parsed_info->gpstimestamp[0], (int)parsed_info->gpstimestamp[1], (int)parsed_info->gpstimestamp[2]);
					Status = parsed_info->gpsstatus;
				}
			};
		}
	}
}