#pragma once

#include "LibRawProcessor.h"
#include "LibRawCodecInfo.h"
#include "LibRawFrameCollection.h"

using namespace System;
using namespace System::IO;
using namespace System::Windows::Media::Imaging;
using namespace System::Collections::Generic;

namespace HurlbertVisionLab {
	namespace LibRawWrapper
	{
		/// <summary>
		/// Defines a decoder for RAW images using LibRaw.
		/// </summary>
		public ref class LibRawBitmapDecoder : System::Windows::Threading::DispatcherObject
		{
		public:
			/// <summary>
			/// Initializes a new instance of the <see cref="LibRawBitmapDecoder" /> class from the specified <see cref="Uri" />, with the specified <paramref name="createOptions" /> and <paramref name="cacheOption" />.
			/// </summary>
			/// <param name="bitmapUri">The <see cref="Uri" /> that identifies the bitmap to decode.</param>
			/// <param name="createOptions">Initialization options for the bitmap image.</param>
			/// <param name="cacheOption">The caching method for the bitmap image.</param>
			LibRawBitmapDecoder(Uri^ bitmapUri, BitmapCreateOptions createOptions, BitmapCacheOption cacheOptions)
			{
				if (bitmapUri == nullptr)
					throw gcnew ArgumentNullException("bitmapUri");

				if (!bitmapUri->IsFile)
					throw gcnew NotSupportedException(L"Only file based Uris are currently supported.");

				_libraw = gcnew Native::LibRawProcessor();
				_libraw->Open(bitmapUri->LocalPath);

				Initialize(createOptions, cacheOptions);
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="LibRawBitmapDecoder" /> class from the specified file stream, with the specified <paramref name="createOptions" /> and <paramref name="cacheOption" />.
			/// </summary>
			/// <param name="bitmapStream">The bitmap stream to decode.</param>
			/// <param name="createOptions">Initialization options for the bitmap image.</param>
			/// <param name="cacheOption">The caching method for the bitmap image.</param>
			LibRawBitmapDecoder(Stream^ stream, BitmapCreateOptions createOptions, BitmapCacheOption cacheOptions)
			{
				if (stream == nullptr)
					throw gcnew ArgumentNullException("stream");

				_libraw = gcnew Native::LibRawProcessor();
				_libraw->Open(stream);

				Initialize(createOptions, cacheOptions);
			}
		private:
			void Initialize(BitmapCreateOptions createOptions, BitmapCacheOption cacheOptions)
			{
				if (cacheOptions != BitmapCacheOption::Default && cacheOptions != BitmapCacheOption::None)
					throw gcnew NotSupportedException(L"Caching is not supported.");

				_codecInfo = gcnew LibRawCodecInfo(_libraw->ImageParameters->Make, _libraw->ImageParameters->Model, _libraw->Version);

				_libraw->OutputParameters->NoAutoBrightness = 1;
				_libraw->OutputParameters->OutputBitsPerPixel = 32;
				_libraw->OutputParameters->SetGammaTosRGB(); // ignored by 32bpp

				if ((createOptions & BitmapCreateOptions::PreservePixelFormat) == BitmapCreateOptions::PreservePixelFormat)
				{
					_libraw->OutputParameters->OutputBitsPerPixel = 16;
				}

				if ((createOptions & BitmapCreateOptions::IgnoreColorProfile) == BitmapCreateOptions::IgnoreColorProfile)
				{
					_libraw->OutputParameters->SetGammaToLinear();
					_libraw->OutputParameters->NoAutoScale = true;
				}
			}

		public:

			/// <summary>
			/// Gets a <see cref="BitmapSource" /> that represents the thumbnail of the bitmap, if one is defined.
			/// </summary>
			property BitmapSource^ Thumbnail
			{
				BitmapSource^ get()
				{
					VerifyAccess();
					if (_thumbnail == nullptr)
						_thumbnail = _libraw->GetThumbnailBitmap();

					return _thumbnail;
				}
			}

			/// <summary>
			/// Gets information that describes this codec.
			/// </summary>
			property BitmapCodecInfo^ CodecInfo
			{
				BitmapCodecInfo^ get()
				{
					VerifyAccess();
					return _codecInfo;
				}
			}

			/// <summary>
			/// Gets the content of an individual frame within a bitmap.
			/// </summary>
			property LibRawFrameCollection^ Frames
			{
				LibRawFrameCollection^ get() { return gcnew LibRawFrameCollection(_libraw); }
			}

		private:
			Native::LibRawProcessor^ _libraw;
			BitmapSource^ _thumbnail;
			LibRawCodecInfo^ _codecInfo;
		};
	}
}