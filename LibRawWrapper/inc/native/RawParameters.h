#pragma once

#include <libraw.h>
#include "LibRawWrapper.h"
#include "DngProcessing.h"
#include "ProcessingOptions.h"
#include "ProcessingSpecials.h"

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			public ref class RawParameters
			{
			public:

				/// <summary>
				/// Gets or sets whether to use RawSpeed library for data unpacking (only if RawSpeed support compiled in).
				/// </summary>
				property bool UseRawSpeed
				{
					bool get() { return m_params->use_rawspeed; }
					void set(bool value) { m_params->use_rawspeed = value; }
				}

				/// <summary>
				/// Gets or sets whether to use Adobe DNG SDK (if compiled with it and dng host is set).
				/// </summary>
				property LibRawWrapper::Native::DngProcessing DngProcessing
				{
					LibRawWrapper::Native::DngProcessing get() { return (LibRawWrapper::Native::DngProcessing)m_params->use_dngsdk; }
					void set(LibRawWrapper::Native::DngProcessing value) { m_params->use_dngsdk = (int)value; }
				}

				/// <summary>
				/// Gets or sets the processing options used on unpack() phase for specific image formats.
				/// </summary>
				property ProcessingOptions Options
				{
					ProcessingOptions get() { return (ProcessingOptions)m_params->options; }
					void set(LibRawWrapper::Native::ProcessingOptions value) { m_params->options = (unsigned int)value; }
				}

				property ProcessingSpecials Specials
				{
					ProcessingSpecials get() { return (ProcessingSpecials)m_params->specials; }
					void set(ProcessingSpecials value) { m_params->specials = (unsigned int)value; }
				}

				/// <summary>
				/// Gets or sets when to stop processing if raw buffer size grows larger than that value (in megabytes).
				/// </summary>
				/// <remarks>
				/// Default is LIBRAW_MAX_ALLOC_MB_DEFAULT (2048Mb).
				/// </remarks>
				property unsigned int MaxRawMemoryMB
				{
					unsigned int get() { return m_params->max_raw_memory_mb; }
					void set(unsigned int value) { m_params->max_raw_memory_mb = value; }
				}

				/// <summary>
				/// Gets or sets the level to suppress posterization display in shadows if <see cref="ProcessingSpecials.SonyArw2DeltaToValue" /> used for <see cref="Specials" />.
				/// </summary>
				property int SonyArw2PosterizationThreshold
				{
					int get() { return m_params->sony_arw2_posterization_thr; }
					void set(int value) { m_params->sony_arw2_posterization_thr = value; }
				}

				/// <summary>
				/// Gets or sets the gamma value for Coolscan NEF decoding (no way to get if from file, it should be set by calling application).
				/// </summary>
				property float CoolscanNefGamma
				{
					float get() { return m_params->coolscan_nef_gamma; }
					void set(float value) { m_params->coolscan_nef_gamma = value; }
				}

				/// <summary>
				/// Gets or sets the shot order for Pentax 4shot files. Default is "3102".
				/// </summary>
				property String^ Pentax4shotOrder
				{
					String^ get() { return FixedCharToString(m_params->p4shot_order); }
					void set(String^ value)
					{
						if (value == nullptr)
						{
							memset(m_params->p4shot_order, 0, sizeof(m_params->p4shot_order));
							return;
						}

						if (value->Length != 4)
							throw gcnew System::ArgumentException("The string must consist of 4 digits.", "value");

						pin_ptr<const WCHAR> str = PtrToStringChars(value);
						for (size_t i = 0; i < 4; i++)
							m_params->p4shot_order[i] = (char)str[i];
					}
				}

			internal:
				RawParameters(libraw_raw_unpack_params_t* params) : m_params(params) { }

			private:
				libraw_raw_unpack_params_t* m_params;
			};
		}
	}
}