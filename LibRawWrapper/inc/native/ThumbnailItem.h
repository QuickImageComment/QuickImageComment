#pragma once

#include <libraw.h>
#include "LibRawWrapper.h"
#include "Flip.h"

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Desribes all parameters associated with the preview saved in the RAW file.
			/// </summary>
			public ref class ThumbnailItem
			{
			public:

				///// <summary>
				///// Gets the thumbnail data format.
				///// </summary>
				//property ThumbnailFormat Format
				//{
				//	ThumbnailFormat get() { return (ThumbnailFormat)m_thumbnail->tformat; }
				//}

				/// <summary>
				/// Gets the thumbnail image width.
				/// </summary>
				/// <remarks>
				/// Note: this field may be zero (so, image size is unknown at parse phase)
				/// </remarks>
				property int Width
				{
					int get() { return m_thumbnail_item->twidth; }
				}

				/// <summary>
				/// Gets the thumbnail image height.
				/// </summary>
				/// <remarks>
				/// Note: this field may be zero (so, image size is unknown at parse phase)
				/// </remarks>
				property int Height
				{
					int get() { return m_thumbnail_item->theight; }
				}

				/// <summary>
				/// Gets the thumbnail image rotation, it may differ from main image rotation.
				/// </summary>
				/// <remarks>
				/// This field may be set to 0xffff, this means 'rotation is not known'
				/// </remarks>
				property Flip Flip
				{
					LibRawWrapper::Native::Flip get() { return (LibRawWrapper::Native::Flip)(int)m_thumbnail_item->tflip; }
				}

				/// <summary>
				/// Gets the thumbnail on-disk data size.
				/// </summary>
				property int ThumbnailLength
				{
					int get() { return (int)m_thumbnail_item->tlength; }
				}

				/// <summary>
				/// Gets the number of bits per pixel.
				/// </summary>
				property int BitsPerPixel
				{
					int get() { return m_thumbnail_item->tmisc & 0x1F; }
				}

				/// <summary>
				/// Gets the number of colors.
				/// </summary>
				property int Colors
				{
					int get() { return m_thumbnail_item->tmisc >> 5; }
				}

				/// <summary>
				/// Gets the thumbnail data offset in file.
				/// </summary>
				property long long ThumbnailOffset
				{
					long long get() { return m_thumbnail_item->toffset; }
				}

			internal:
				ThumbnailItem(libraw_thumbnail_item_t* thumbnail_item) : m_thumbnail_item(thumbnail_item) { }

			private:
				libraw_thumbnail_item_t* m_thumbnail_item;
			};
		}
	}
}