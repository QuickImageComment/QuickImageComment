#pragma once

#include <libraw.h>
#include "LibRawWrapper.h"
#include "ThumbnailFormat.h"

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Desribes all parameters associated with the preview saved in the RAW file.
			/// </summary>
			public ref class Thumbnail
			{
			public:

				/// <summary>
				/// Gets the thumbnail data format.
				/// </summary>
				property ThumbnailFormat Format
				{
					ThumbnailFormat get() { return (ThumbnailFormat)m_thumbnail->tformat; }
				}

				/// <summary>
				/// Gets the width of the preview image in pixels.
				/// </summary>
				property int Width
				{
					int get() { return m_thumbnail->twidth; }
				}

				/// <summary>
				/// Gets the height of the preview image in pixels.
				/// </summary>
				property int Height
				{
					int get() { return m_thumbnail->theight; }
				}

				/// <summary>
				/// Gets the number of colors in the preview.
				/// </summary>
				property int Colors
				{
					int get() { return m_thumbnail->tcolors; }
				}

				/// <summary>
				/// Gets the thumbnail extracted from the data file.
				/// </summary>
				/// <returns></returns>
				array<byte>^ GetThumbnail()
				{
					return FixedToArray((byte*)&m_thumbnail->thumb[0], (int)m_thumbnail->tlength);
				}

				/// <summary>
				/// Gets the pointer to the thumbnail extracted from the data file.
				/// </summary>
				property IntPtr ThumbnailBuffer
				{
					IntPtr get() { return (IntPtr)m_thumbnail->thumb; }
				}

				/// <summary>
				/// Gets the length (in bytes) of the thumbnail buffer.
				/// </summary>
				property int ThumbnailBufferLength
				{
					int get() { return (int)m_thumbnail->tlength; }
				}


			internal:
				Thumbnail(libraw_thumbnail_t* thumbnail) : m_thumbnail(thumbnail) { }

			private:
				libraw_thumbnail_t* m_thumbnail;
			};
		}
	}
}