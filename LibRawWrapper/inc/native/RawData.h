#pragma once

#include <libraw.h>
#include "LibRawWrapper.h"

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			public enum class RawDataFormat
			{
				None,
				Short1,
				Short3,
				Short4,
				Float1,
				Float3,
				Float4,
				Unknown = -1
			};

			public ref class RawData
			{
			public:
				property IntPtr Buffer
				{
					IntPtr get() { return (IntPtr)m_rawdata->raw_alloc; }
				}

				property RawDataFormat BufferFormat
				{
					RawDataFormat get()
					{
						if (!m_rawdata->raw_alloc)
							return RawDataFormat::None;

						if (m_rawdata->raw_image)
							return RawDataFormat::Short1;

						else if (m_rawdata->color3_image)
							return RawDataFormat::Short3;

						else if (m_rawdata->color4_image)
							return RawDataFormat::Short4;

						else if (m_rawdata->float_image)
							return RawDataFormat::Float1;

						else if (m_rawdata->float3_image)
							return RawDataFormat::Float3;

						else if (m_rawdata->float4_image)
							return RawDataFormat::Float4;

						else
							return RawDataFormat::Unknown;
					}
				}

			internal:
				RawData(libraw_rawdata_t* rawdata) : m_rawdata(rawdata) { }

			private:
				libraw_rawdata_t* m_rawdata;
			};
		}
	}
}