#pragma once

#include <libraw.h>
#include "LibRawWrapper.h"
#include "CameraMaker.h"
#include "ColorDescription.h"

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			public ref class ImageParameters
			{
			public:
				/// <summary>
				/// Gets the camera manufacturer.
				/// </summary>
				property String^ Make
				{
					String^ get() { return CharToString(&m_iparams->make[0], sizeof(&m_iparams->make)); }
				}

				/// <summary>
				/// Gets the camera model.
				/// </summary>
				property String^ Model
				{
					String^ get() { return CharToString(&m_iparams->model[0], sizeof(&m_iparams->model)); }
				}

				/// <summary>
				/// Gets the primary camera manufacturer.
				/// </summary>
				/// <remarks>
				/// There is a huge number of identical cameras sold under different names, depending on the market (e.g. multiple Panasonic or Canon models) and even some identical cameras sold under different brands (Panasonic -> Leica, Sony -> Hasselblad).
				/// <see href="NormalizedMake">NormalizedMake</see> contains primary vendor name (e.g. Panasonic for Leica re-branded cameras).
				/// </remarks>
				property String^ NormalizedMake
				{
					String^ get() { return CharToString(&m_iparams->normalized_make[0], sizeof(&m_iparams->normalized_make)); }
				}

				/// <summary>
				/// Gets the primary camera model.
				/// </summary>
				property String^ NormalizedModel
				{
					String^ get() { return CharToString(&m_iparams->normalized_model[0], sizeof(&m_iparams->normalized_model)); }
				}

				/// <summary>
				/// Gets the primary vendor name in indexed form.
				/// </summary>
				property LibRawWrapper::Native::CameraMaker CameraMaker
				{
					LibRawWrapper::Native::CameraMaker get() { return (LibRawWrapper::Native::CameraMaker)m_iparams->maker_index; }
				}

				/// <summary>
				/// Gets the software name/version (mostly for DNG files, to distinguish in-camera DNGs from Adobe DNG Converter produced ones).
				/// </summary>
				property String^ Software
				{
					String^ get() { return CharToString(&m_iparams->software[0], sizeof(&m_iparams->software)); }
				}

				/// <summary>
				/// Gets the number of RAW images in file (0 means that the file has not been recognized).
				/// </summary>
				property int RawCount
				{
					int get() { return m_iparams->raw_count; }
				}

				/// <summary>
				/// Gets whether the image is a Sigma Foveon Image.
				/// </summary>
				property bool IsFoveon
				{
					bool get() { return m_iparams->is_foveon; }
				}

				/// <summary>
				/// Gets the DNG version (for the DNG format)
				/// </summary>
				property Version^ DngVersion
				{
					Version^ get()
					{
						if (!m_iparams->dng_version)
							return nullptr;

						return gcnew Version(
							(byte)(m_iparams->dng_version >> 24),
							(byte)(m_iparams->dng_version >> 16),
							(byte)(m_iparams->dng_version >> 8),
							(byte)(m_iparams->dng_version));
					}
				}

				/// <summary>
				/// Gets the number of colors in the file.
				/// </summary>
				property int Colors
				{
					int get() { return m_iparams->colors; }
				}

				/// <summary>
				/// Gets a bit mask describing the order of color pixels in the matrix (0 for full-color images). 
				/// </summary>
				/// <remarks>
				/// 32 bits of this field describe 16 pixels (8 rows with two pixels in each, from left to right and from top to bottom).
				/// Each two bits have values 0 to 3, which correspond to four possible colors.
				/// Convenient work with this field is ensured by the COLOR(row,column) function, which returns the number of the active color for a given pixel.
				/// Values less than 1000 are reserved as special cases:<ul>
				/// <li>1 - Leaf Catchlight with 16x16 bayer matrix;</li>
				/// <li>9 - Fuji X-Trans (6x6 matrix)</li>
				/// <li>3..8 and 10..999 - are unused.</li></ul>
				/// </remarks>
				property unsigned int Filters
				{
					unsigned int get() { return m_iparams->filters; }
				}

				/// <summary>
				/// Gets Fuji X-Trans row/col to color mapping relative to visible area.
				/// </summary>
				property array<byte, 2>^ FujiXTransMapping
				{
					array<byte, 2>^ get() { return FixedToArray((byte*)&m_iparams->xtrans, 6, 6); }
				}

				/// <summary>
				/// Gets Fuji X-Trans row/col to color mapping relative to sensor edges.
				/// </summary>
				property array<byte, 2>^ FujiXTransMappingAbsolute
				{
					array<byte, 2>^ get() { return FixedToArray((byte*)&m_iparams->xtrans_abs, 6, 6); }
				}

				/// <summary>
				/// Gets the description of colrs numbered from 0 to 3.
				/// </summary>
				property ColorDescription ColorDescription
				{
					LibRawWrapper::Native::ColorDescription get() { return *(LibRawWrapper::Native::ColorDescription*)&m_iparams->cdesc[0]; }
				}

				/// <summary>
				/// Gets the pointer to extracted XMP packet.
				/// </summary>
				property System::IntPtr XmpDataBuffer
				{
					System::IntPtr get() { return (System::IntPtr)m_iparams->xmpdata; }
				}

				/// <summary>
				/// Gets the length (in bytes) of the extracted XMP packet.
				/// </summary>
				property int XmpDataBufferLength
				{
					int get() { return (int)m_iparams->xmplen; }
				}

				/// <summary>
				/// Gets whether the RAW file contains an ICC profile.
				/// </summary>
				property bool HasXmpData
				{
					bool get() { return m_iparams->xmpdata && m_iparams->xmplen; }
				}

				/// <summary>
				/// Gets the extracted XMP packet.
				/// </summary>
				/// <returns>the extracted XMP packet as a byte array.</returns>
				array<byte>^ GetXmpData()
				{
					if (!HasXmpData)
						return nullptr;

					return FixedToArray((byte*)m_iparams->xmpdata, m_iparams->xmplen);
				}

			internal:
				ImageParameters(libraw_iparams_t* iparams) : m_iparams(iparams) { }

			private:
				libraw_iparams_t* m_iparams;
			};
		}
	}
}