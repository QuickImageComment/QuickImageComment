#pragma once

#include <libraw.h>
#include "ExifLightSource.h"
#include "ExifColorSpace.h"
#include "Pixel4.h"
#include "PerChannelBlackCorrection.h"
#include "ChannelStatistic.h" 

using namespace System;
using namespace System::IO;
using namespace System::Collections::Generic;
using namespace System::Windows::Media;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Color data retrieved from the file.
			/// </summary>
			public ref class ColorData
			{
			public:
				/// <summary>
				/// Gets the camera tone curve. May be read from file as is, or calculated, depending on file format.
				/// </summary>
				array<ushort>^ GetCurve()
				{
					return FixedToArray(&m_colordata->curve[0], sizeof(m_colordata->curve));
				}

				/// <summary>
				/// Gets the pointer to the camera tone curve. The buffer contains 65535 ushort values.
				/// </summary>
				property IntPtr CurveBuffer
				{
					IntPtr get() { return (IntPtr)&m_colordata->curve[0]; }
				}

				/// <summary>
				/// Gets the length (in bytes) of the curve buffer.
				/// </summary>
				property int CurveBufferLength
				{
					int get() { return sizeof(m_colordata->curve); }
				}

				/// <summary>
				/// Gets the black level, calculated at the unpacking stage, read from the RAW file, or hardcoded.
				/// </summary>
				/// <remarks>
				/// Depending on the camera, it may be zero (this means that black has been subtracted at the unpacking stage or by the camera itself).
				/// </remarks>
				property int Black
				{
					int get() { return m_colordata->black; }
				}

				/// <summary>
				/// Gets the per-channel black level correction.
				/// </summary>
				/// <returns></returns>
				PerChannelBlackCorrection^ GetPerChannelBlackCorrection()
				{
					return gcnew PerChannelBlackCorrection(&m_colordata->cblack[0]);
				}

				/// <summary>
				/// Gets the pointer to the per-channel black level correction. The buffer contains up to 4102 int values.
				/// </summary>
				/// <remarks>
				/// First 4 values are per-channel correction, next two are black level pattern block size, than cblack[4]*cblack[5] correction values (for indexes[6....6+cblack[4]*cblack[5]).
				/// </remarks>
				property IntPtr PerChannelBlackCorrectionBuffer
				{
					IntPtr get() { return (IntPtr)&m_colordata->cblack[0]; }
				}

				/// <summary>
				/// Gets the length (in bytes) of the per-channel black level correction buffer.
				/// </summary>
				property int PerchannelBlackCorrectionBufferLength
				{
					int get() { return sizeof(m_colordata->cblack); }
				}

				/// <summary>
				/// Gets the maximum pixel value in current file. Calculated at raw2image or dcraw_process() calls.
				/// </summary>
				property int DataMaximum
				{
					int get() { return m_colordata->data_maximum; }
				}

				/// <summary>
				/// Gets the maximum pixel value. Calculated from the data for most cameras, hardcoded for others.
				/// </summary>
				/// <remarks>
				/// This value may be changed on postprocessing stage (when black subtraction performed) and by automated maximum adjustment (this adjustment performed if params.adjust_maximum_thr is set to nonzero).
				/// </remarks>
				property int Maximum
				{
					int get() { return m_colordata->maximum; }
				}

				/// <summary>
				/// Gets per-channel linear data maximum read from file metadata. 
				/// </summary>
				/// <remarks>
				/// If RAW file does not contains this data, linear_max[] is set to zero.
				/// Black value is not subtracted.
				/// </remarks>
				property array<long>^ PerChannelLinearMaximum
				{
					// in CLL+/CLI, long = int = Int32 (https://docs.microsoft.com/en-us/cpp/dotnet/managed-types-cpp-cli)
					array<long>^ get() { return FixedToArray(m_colordata->linear_max, sizeof(m_colordata->linear_max), true); }
				}

				/// <summary>
				/// Gets the maximum pixel value in real image for floating data files.
				/// </summary>
				property float FloatingMaximum
				{
					float get() { return m_colordata->fmaximum; }
				}

				/// <summary>
				/// Gets the normalization coefficient used while converting floating point raw data to integer.
				/// </summary>
				property float FloatingNormalizationCoefficient
				{
					float get() { return m_colordata->fnorm; }
				}

				/// <summary>
				/// Gets the block of white pixels extracted from files CIFF/CRW.
				/// </summary>
				/// <remarks>
				/// Not extracted for other formats.
				/// Used to calculate white balance coefficients.
				/// </remarks>
				property array<ushort, 2>^ WhiteBlock
				{
					array<ushort, 2>^ get() { return FixedToArray(m_colordata->white[0, 0], 8, 8); }
				}

				/// <summary>
				/// Gets the camera RGB - XYZ conversion matrix.
				/// </summary>
				/// <remarks>
				/// This matrix is constant (different for different models).
				/// Last row is zero for RGB cameras and non-zero for different color models (CMYG and so on).
				/// </remarks>
				property array<float, 2>^ CameraRgbToXyz
				{
					array<float, 2>^ get() { return FixedToArray(m_colordata->cam_xyz[0, 0], 4, 3); }
				}

				/// <summary>
				/// White balance coefficients (as shot). Either read from file or calculated.
				/// </summary>
				property array<float>^ CameraWhiteBalance
				{
					array<float>^ get() { return FixedToArray(m_colordata->cam_mul, sizeof(m_colordata->cam_mul)); }
				}

				/// <summary>
				/// White balance coefficients for daylight (daylight balance).
				/// Either read from file, or calculated on the basis of file data, or taken from hardcoded constants.
				/// </summary>
				property array<float>^ DaylightWhiteBalance
				{
					array<float>^ get() { return FixedToArray(m_colordata->pre_mul, sizeof(m_colordata->pre_mul)); }
				}

				/// <summary>
				/// Gets the camera color data read from RAW file (if any).
				/// </summary>
				property array<float, 2>^ CameraMatrix
				{
					array<float, 2>^ get() { return FixedToArray(m_colordata->cmatrix[0, 0], 3, 4); }
				}

				/// <summary>
				/// Gets the camera to sRGB conversion matrix.
				/// </summary>
				property array<float, 2>^ CameraTosRgb
				{
					array<float, 2>^ get() { return FixedToArray(m_colordata->rgb_cam[0, 0], 3, 4); }
				}

				/// <summary>
				/// Gets the camera color correction matrix readed from file metadata (uniform matrix if no such data in file)
				/// </summary>
				property array<float, 2>^ CameraColorCorrectionMatrix
				{
					array<float, 2>^ get() { return FixedToArray(m_colordata->ccm[0, 0], 3, 4); }
				}

				// TODO: phase_one_data;

				/// <remarks>
				/// Used for white balance calculations (for some P&amp;S Canoncameras).
				/// </remarks>
				property float FlashUsed
				{
					float get() { return m_colordata->flash_used; }
				}

				/// <remarks>
				/// Used for white balance calculations (for some P&amp;S Canoncameras).
				/// </remarks>
				property float CanonEV
				{
					float get() { return m_colordata->canon_ev; }
				}

				/// <summary>
				/// Gets firmware revision (for some cameras).
				/// </summary>
				property String^ FirmwareRevision
				{
					String^ get() { return FixedCharToString(m_colordata->model2); }
				}

				/// <summary>
				/// Gets the UniqueCameraModel DNG tag value.
				/// </summary>
				property String^ UniqueCameraModel
				{
					String^ get() { return FixedCharToString(m_colordata->UniqueCameraModel); }
				}

				/// <summary>
				/// Gets the LocalizedCameraModel DNG tag value.
				/// </summary>
				property String^ LocalizedCameraModel
				{
					String^ get() { return FixedCharToString(m_colordata->LocalizedCameraModel); }
				}

				/// <summary>
				/// Gets the pointer to the retrieved ICC profile (if it is present in the RAW file).
				/// </summary>
				property IntPtr ProfileBuffer
				{
					IntPtr get() { return (IntPtr)m_colordata->profile; }
				}

				/// <summary>
				/// Gets the length (in bytes) of the retrieved ICC profile.
				/// </summary>
				property int ProfileBufferLength
				{
					int get() { return (int)m_colordata->profile_length; }
				}

				/// <summary>
				/// Gets whether the RAW file contains an ICC profile.
				/// </summary>
				property bool HasProfile
				{
					bool get() { return m_colordata->profile && m_colordata->profile_length; }
				}

				/// <summary>
				/// Gets the retrieved ICC profile (if it is present in the RAW file).
				/// </summary>
				/// <returns>the contents of the ICC profile as a byte array.</returns>
				array<byte>^ GetProfile()
				{
					if (!HasProfile)
						return nullptr;

					return FixedToArray((byte*)m_colordata->profile, m_colordata->profile_length);
				}

				/// <summary>
				/// Gets the retrieved ICC profile (if it is present in the RAW file).
				/// </summary>
				/// <returns>the ICC profile in the form of <see href="System::Windows::Media::ColorContext">ColorContext</see>.</returns>
				ColorContext^ GetColorContext()
				{
					// There is no overload taking bytes, apart form internal ColorContext.FromRawBytes.

					if (!HasProfile)
						return nullptr;

					String^ tempFileName = Path::GetTempFileName();
					pin_ptr<const WCHAR> pTempFileName = PtrToStringChars(tempFileName);

					std::ofstream tempFile;
					tempFile.open(pTempFileName, std::ios::out | std::ios::binary);
					tempFile.write((char*)m_colordata->profile, m_colordata->profile_length);
					tempFile.close();

					Uri^ tempFileUri = gcnew Uri(tempFileName);

					return gcnew ColorContext(tempFileUri);
				}

				array<ChannelStatistic>^ GetBlackStatistics()
				{
					int len = sizeof(m_colordata->black_stat) / sizeof(m_colordata->black_stat[0]) / 2;
					array<ChannelStatistic>^ black_stat = gcnew array<ChannelStatistic>(len);
					for (size_t i = 0; i < len; i++)
						black_stat[i] = ChannelStatistic((int)m_colordata->black_stat[i], (int)m_colordata->black_stat[len + i]);

					return black_stat;
				}

				// TODO: dng_color
				// TODO: dng_levels

			private:
				inline Pixel4<int> GetWhiteBalanceCoefficient(size_t index)
				{
					return Pixel4<int>
						(
							m_colordata->WB_Coeffs[index][0],
							m_colordata->WB_Coeffs[index][1],
							m_colordata->WB_Coeffs[index][2],
							m_colordata->WB_Coeffs[index][3]
							);
				}

				inline Pixel4<float> GetColorTemperatureCoefficient(size_t index)
				{
					return Pixel4<float>
						(
							m_colordata->WBCT_Coeffs[index][1],
							m_colordata->WBCT_Coeffs[index][2],
							m_colordata->WBCT_Coeffs[index][3],
							m_colordata->WBCT_Coeffs[index][4]
							);
				}

			public:
				/// <summary>
				/// Gets white balance coefficients for a specific light source.
				/// </summary>
				/// <param name="source">The light source for which coefficients should be retreived.</param>
				/// <returns>white balance coefficients if defined; otherwise zero coefficients.</returns>
				Pixel4<int> GetWhiteBalanceCoefficient(ExifLightSource source)
				{
					int len = sizeof(m_colordata->WB_Coeffs) / sizeof(m_colordata->WB_Coeffs[0]);

					size_t index = (size_t)source;
					if (index < 0 || index >= len)
						throw gcnew System::IndexOutOfRangeException(L"source");

					return GetWhiteBalanceCoefficient(index);
				}

				/// <summary>
				/// Gets white balance coefficients	per light source.
				/// </summary>
				/// <returns>a dictionary with all defined light sources and their white balance coefficients.</returns>
				IDictionary<ExifLightSource, Pixel4<int>>^ GetWhiteBalanceCoefficients()
				{
					int len = sizeof(m_colordata->WB_Coeffs) / sizeof(m_colordata->WB_Coeffs[0]);
					IDictionary<ExifLightSource, Pixel4<int>>^ WB_Coeffs = gcnew Dictionary<ExifLightSource, Pixel4<int>>(len);
					for (size_t i = 0; i < len; i++)
					{
						Pixel4<int>* coeff = &GetWhiteBalanceCoefficient(i);
						if (coeff->R || coeff->G1 || coeff->G2 || coeff->R)
							WB_Coeffs->Add((ExifLightSource)i, *coeff);
					}

					return WB_Coeffs;
				}

				/// <summary>
				/// Gets white balance coefficients per color temperature.
				/// </summary>
				/// <returns>a dictionary with all defined color temperatures and their white balance coefficients.</returns>
				IDictionary<int, Pixel4<float>>^ GetColorTemperatureCoefficients()
				{
					int len = sizeof(m_colordata->WBCT_Coeffs) / sizeof(m_colordata->WBCT_Coeffs[0]);
					IDictionary<int, Pixel4<float>>^ WBCT_Coeffs = gcnew Dictionary<int, Pixel4<float>>(len);
					for (size_t i = 0; i < len; i++)
					{
						int cct = (int)m_colordata->WBCT_Coeffs[i][0];
						if (cct)
						{
							Pixel4<float>* coeff = &GetColorTemperatureCoefficient(i);
							WBCT_Coeffs->Add(cct, *coeff);
						}
					}

					return WBCT_Coeffs;
				}

				/// <summary>
				/// Gets whether WB already applied in camera (multishot modes; small raw)
				/// </summary>
				property bool AsShowWhiteBalanceApplied
				{
					bool get() { return m_colordata->as_shot_wb_applied; }
				}

				// TODO: P1_color[2]

				/// <summary>
				/// Gets RAW bits per pixel (PhaseOne: Raw format used).
				/// </summary>
				property int RawBitsPerPixel
				{
					int get() { return m_colordata->raw_bps; }
				}

				property ExifColorSpace ColorSpace
				{
					ExifColorSpace get() { return (ExifColorSpace)m_colordata->ExifColorSpace; }
				}


			internal:
				ColorData(libraw_colordata_t* colordata) : m_colordata(colordata) { }

			private:
				libraw_colordata_t* m_colordata;
			};
		}
	}
}