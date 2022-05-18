#pragma once
#include <libraw.h>
#include <vcclr.h>
#include "Flip.h"

using namespace System;
using namespace System::Windows;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			public ref class ImageSizes
			{
			public:
				/// <summary>
				/// Gets the full height of RAW image (including the frame) in pixels.
				/// </summary>
				property int RawHeight
				{
					int get() { return m_image_sizes->raw_height; }
				}

				/// <summary>
				/// Gets the full width of RAW image (including the frame) in pixels.
				/// </summary>
				property int RawWidth
				{
					int get() { return m_image_sizes->raw_width; }
				}

				/// <summary>
				/// Gets the full size of RAW image (including the frame) in pixels.
				/// </summary>
				property Size RawSize
				{
					System::Windows::Size get() { return System::Windows::Size(m_image_sizes->raw_width, m_image_sizes->raw_height); }
				}

				/// <summary>
				///  Gets the height of visible ("meaningful") part of the image (without the frame).
				/// </summary>
				property int Height
				{
					int get() { return m_image_sizes->height; }
				}

				/// <summary>
				///  Gets the width of visible ("meaningful") part of the image (without the frame).
				/// </summary>
				property int Width
				{
					int get() { return m_image_sizes->width; }
				}

				/// <summary>
				///  Gets the size of visible ("meaningful") part of the image (without the frame).
				/// </summary>
				property Size Size
				{
					System::Windows::Size get() { return System::Windows::Size(m_image_sizes->width, m_image_sizes->height); }
				}

				/// <summary>
				/// Gets the coordinates of the top left corner of the frame.
				/// </summary>
				property int TopMargin { int get() { return m_image_sizes->top_margin; } }

				/// <summary>
				/// Gets the coordinates of the top left corner of the frame.
				/// </summary>
				property int LeftMargin { int get() { return m_image_sizes->left_margin; } }

				/// <summary>
				/// Gets the coordinates of the visible part of the image.
				/// </summary>
				property Thickness Margin
				{
					Thickness get()
					{
						return Thickness(
							m_image_sizes->left_margin,
							m_image_sizes->top_margin,
							m_image_sizes->raw_width - m_image_sizes->width - m_image_sizes->left_margin,
							m_image_sizes->raw_height - m_image_sizes->height - m_image_sizes->top_margin);
					}
				}

				/// <summary>
				/// Gets the coordinates of the visible part of the image.
				/// </summary>
				property Int32Rect Rect
				{
					Int32Rect get()
					{
						return Int32Rect(
							m_image_sizes->left_margin,
							m_image_sizes->top_margin,
							m_image_sizes->raw_width,
							m_image_sizes->raw_height);
					}
				}

				/// <summary>
				/// Gets the height of the output image (may differ from height for cameras that require image rotation or have non-square pixels).
				/// </summary>
				property int OutputHeight
				{
					int get() { return m_image_sizes->iheight; }
				}

				/// <summary>
				/// Gets the width of the output image (may differ from width for cameras that require image rotation or have non-square pixels).
				/// </summary>
				property int OutputWidth
				{
					int get() { return m_image_sizes->iwidth; }
				}

				/// <summary>
				/// Gets the size of the output image (may differ from width/height for cameras that require image rotation or have non-square pixels).
				/// </summary>
				property System::Windows::Size OutputSize
				{
					System::Windows::Size get() { return System::Windows::Size(m_image_sizes->iwidth, m_image_sizes->iheight); }
				}

				/// <summary>
				/// Gets the full size of raw data row in bytes (i.e. stride).
				/// </summary>
				property unsigned int RawPitch
				{
					unsigned int get() { return m_image_sizes->raw_pitch; }
				}

				/// <summary>
				/// Gest the pixel width/height ratio. 
				/// If it is not unity, scaling of the image along one of the axes is required during output.
				/// </summary>
				property double PixelAspect
				{
					double get() { return m_image_sizes->pixel_aspect; }
				}

				/// <summary>
				/// Gets the image orientation.
				/// </summary>
				property Flip Flip
				{
					LibRawWrapper::Native::Flip get() { return (LibRawWrapper::Native::Flip)m_image_sizes->flip; }
				}

			internal:
				ImageSizes(libraw_image_sizes_t* image_sizes) : m_image_sizes(image_sizes) { }

			private:
				libraw_image_sizes_t* m_image_sizes;
			};
		}
	}
}