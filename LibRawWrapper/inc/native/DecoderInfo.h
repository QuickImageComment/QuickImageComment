#pragma once

#include <libraw.h>
#include "DecoderFlags.h"

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			public ref class DecoderInfo
			{
			public:
				/// <summary>
				/// Gets the decoder function name.
				/// </summary>
				property String^ Name
				{
					String^ get() { return _name; }
				}

				/// <summary>
				/// Gets the decoder data format.
				/// </summary>
				property DecoderFlags Flags
				{
					DecoderFlags get() { return _flags; }
				}

			internal:
				DecoderInfo(libraw_decoder_info_t* info)
				{
					_name = gcnew System::String(info->decoder_name);
					_flags = (DecoderFlags)info->decoder_flags;
				}

			private:
				System::String^ _name;
				DecoderFlags _flags;
			};
		}
	}
}