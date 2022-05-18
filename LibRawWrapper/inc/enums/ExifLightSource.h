#pragma once

#include <libraw.h>

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// The kind of light source.
			/// </summary>
			public enum class ExifLightSource : byte
			{
				/// <summary>
				/// Unknown.
				/// </summary>
				Unknown = LIBRAW_WBI_Unknown,

				/// <summary>
				/// Daylight.
				/// </summary>
				Daylight = LIBRAW_WBI_Daylight,

				/// <summary>
				/// Fluorescent.
				/// </summary>
				Fluorescent = LIBRAW_WBI_Fluorescent,

				/// <summary>
				/// Tungsten (incandescent light);
				/// </summary>
				Tungsten = LIBRAW_WBI_Tungsten,

				/// <summary>
				/// Flash.
				/// </summary>
				Flash = LIBRAW_WBI_Flash,

				/// <summary>
				/// Fine weather.
				/// </summary>
				FineWeather = LIBRAW_WBI_FineWeather,

				/// <summary>
				/// Cloudy weather.
				/// </summary>
				CloudyWeather = LIBRAW_WBI_Cloudy,

				/// <summary>
				/// Shade.
				/// </summary>
				Shade = LIBRAW_WBI_Shade,

				/// <summary>
				/// Daylight fluorescent (D 5700 - 7100K)
				/// </summary>
				DaylightFluorescent = LIBRAW_WBI_FL_D,

				/// <summary>
				/// Day white fluorescent (N 4600 - 5500K)
				/// </summary>
				DayWhiteFluorescent = LIBRAW_WBI_FL_N,

				/// <summary>
				/// Cool white fluorescent (W 3800 - 4500K)
				/// </summary>
				CoolWhiteFluorescent = LIBRAW_WBI_FL_W,

				/// <summary>
				/// White fluorescent (WW 3250 - 3800K)
				/// </summary>
				WhiteFluorescent = LIBRAW_WBI_FL_WW,

				/// <summary>
				/// Warm white fluorescent (L 2600 - 3250K)
				/// </summary>
				WarmWhiteFluorescent = LIBRAW_WBI_FL_L,

				/// <summary>
				/// Standard light A.
				/// </summary>
				StandardLightA = LIBRAW_WBI_Ill_A,

				/// <summary>
				/// Standard light B.
				/// </summary>
				StandardLightB = LIBRAW_WBI_Ill_B,

				/// <summary>
				/// Standard Light C.
				/// </summary>
				StandardLightC = LIBRAW_WBI_Ill_C,

				/// <summary>
				/// D55.
				/// </summary>
				D55 = LIBRAW_WBI_D55,

				/// <summary>
				/// D65.
				/// </summary>
				D65 = LIBRAW_WBI_D65,

				/// <summary>
				/// D75.
				/// </summary>
				D75 = LIBRAW_WBI_D75,

				/// <summary>
				/// D50.
				/// </summary>
				D50 = LIBRAW_WBI_D50,

				/// <summary>
				/// ISO studio tungsten.
				/// </summary>
				StudioTungsten = LIBRAW_WBI_StudioTungsten,

				// unverified

				Sunset = LIBRAW_WBI_Sunset,
				Underwater = LIBRAW_WBI_Underwater,
				FluorescentHigh = LIBRAW_WBI_FluorescentHigh,
				HT_Mercury = LIBRAW_WBI_HT_Mercury,
				AsShot = LIBRAW_WBI_AsShot,
				Auto = LIBRAW_WBI_Auto,
				Custom = LIBRAW_WBI_Custom,
				Auto1 = LIBRAW_WBI_Auto1,
				Auto2 = LIBRAW_WBI_Auto2,
				Auto3 = LIBRAW_WBI_Auto3,
				Auto4 = LIBRAW_WBI_Auto4,
				Custom1 = LIBRAW_WBI_Custom1,
				Custom2 = LIBRAW_WBI_Custom2,
				Custom3 = LIBRAW_WBI_Custom3,
				Custom4 = LIBRAW_WBI_Custom4,
				Custom5 = LIBRAW_WBI_Custom5,
				Custom6 = LIBRAW_WBI_Custom6,
				PC_Set1 = LIBRAW_WBI_PC_Set1,
				PC_Set2 = LIBRAW_WBI_PC_Set2,
				PC_Set3 = LIBRAW_WBI_PC_Set3,
				PC_Set4 = LIBRAW_WBI_PC_Set4,
				PC_Set5 = LIBRAW_WBI_PC_Set5,
				Measured = LIBRAW_WBI_Measured,
				BW = LIBRAW_WBI_BW,
				Kelvin = LIBRAW_WBI_Kelvin,

				/// <summary>
				/// Other light source.
				/// </summary>
				Other = LIBRAW_WBI_Other,
			};
		}
	}
}