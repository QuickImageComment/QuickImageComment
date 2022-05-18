#pragma once

#include <libraw.h>
#include <vcclr.h>
#include "Warnings.h"
#include "Progress.h"
#include "ImageParameters.h"
#include "ImageSizes.h"
#include "LensInfo.h"
#include "ColorData.h"
#include "OtherInformation.h"
#include "Thumbnail.h"
#include "RawData.h"
#include "OutputParameters.h"
#include "RawParameters.h"
#include "ShootingInfo.h"
#include "DecoderInfo.h"
#include "LibRaw_managed_datastream.h"
#include "LibRawProgressEventArgs.h"

using namespace System;
using namespace System::IO;
using namespace System::Runtime::InteropServices;
using namespace System::Windows::Media;
using namespace System::Windows::Media::Imaging;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// The main LibRaw object (class).
			/// </summary>
			public ref class LibRawProcessor
			{
			public:
				/// <summary>
				/// Initializes a new instance of the <see cref="LibRawProcessor" /> class.
				/// </summary>
				LibRawProcessor() : m_libraw(new ::LibRaw)
				{
					m_sizes = gcnew LibRawWrapper::Native::ImageSizes(&m_libraw->imgdata.sizes);
					m_data = gcnew LibRawWrapper::Native::ImageParameters(&m_libraw->imgdata.idata);
					m_lens = gcnew LibRawWrapper::Native::LensInfo(&m_libraw->imgdata.lens);
					//TODO: makernotes
					m_shootinginfo = gcnew LibRawWrapper::Native::ShootingInfo(&m_libraw->imgdata.shootinginfo);
					m_params = gcnew LibRawWrapper::Native::OutputParameters(&m_libraw->imgdata.params);
					m_rawparams = gcnew LibRawWrapper::Native::RawParameters(&m_libraw->imgdata.rawparams);
					m_color = gcnew LibRawWrapper::Native::ColorData(&m_libraw->imgdata.color);
					m_other = gcnew LibRawWrapper::Native::OtherInformation(&m_libraw->imgdata.other);
					m_thumbnail = gcnew LibRawWrapper::Native::Thumbnail(&m_libraw->imgdata.thumbnail);
					m_rawdata = gcnew LibRawWrapper::Native::RawData(&m_libraw->imgdata.rawdata);
				}

				!LibRawProcessor() // finalizer for unmanaged
				{
					delete m_libraw;
				}

				~LibRawProcessor() // desctructor for managed (implements IDisposable)
				{
					m_libraw->set_progress_handler(NULL, NULL);
					m_progress_callback.Free();

					this->!LibRawProcessor();
				}

				/// <summary>
				/// Opens a file with RAW data, reads metadata (EXIF) from it, and fills <see cref="ImageParameters" />, <see cref="Sizes" />, <see cref="Color" />, <see cref="Other" /> and <see cref="Thumbnail" />.
				/// </summary>
				/// <param name="filename">The file to open.</param>
				void Open(String^ filename)
				{
					if (filename == nullptr)
						throw gcnew ArgumentNullException("filename");

					if (!File::Exists(filename)) // friendlier than generic I/O error
						throw gcnew FileNotFoundException();

					pin_ptr<const WCHAR> str = PtrToStringChars(filename);
					ThrowOnError(m_libraw->open_file(str));
				}

				/// <summary>
				/// Opens a datastream with RAW data, reads metadata (EXIF) from it, and fills <see cref="ImageParameters" />, <see cref="Sizes" />, <see cref="Color" />, <see cref="Other" /> and <see cref="Thumbnail" />.
				/// </summary>
				/// <param name="stream">The stream to open. The stream should be readable and seekable.</param>
				void Open(Stream^ stream)
				{
					if (stream == nullptr)
						throw gcnew ArgumentNullException("stream");

					UnmanagedMemoryStream^ unmanagedStream = dynamic_cast<UnmanagedMemoryStream^>(stream);
					if (unmanagedStream)
					{
						ThrowOnError(m_libraw->open_buffer(unmanagedStream->PositionPointer, unmanagedStream->Length));
						return;
					}

					LibRaw_managed_datastream* datastream = new LibRaw_managed_datastream(stream);
					ThrowOnError(m_libraw->open_datastream(datastream));
				}

				/// <summary>
				/// Unpacks the RAW files of the image, calculates the black level (not for all formats).
				/// The results are placed in imgdata.image.
				/// </summary>
				void Unpack()
				{
					ThrowOnError(m_libraw->unpack());
				}

				/// <summary>
				/// Reads (or unpacks) the image preview (thumbnail), placing the result into the <see href="Thumbnail.ThumbnailBuffer">Thumbnail.ThumbnailBuffer</see>.
				/// </summary>
				/// <remarks>
				/// JPEG previews are placed into this buffer without any changes (with the header etc.). 
				/// Other preview formats are placed into the buffer in the form of the unpacked bitmap image (three components, 8 bits per component).
				/// The thumbnail format is written to the <see href="Thumbnail.ThumbnailFormat">Thumbnail.Format</see> field.
				/// </remarks>
				void UnpackThumbnail()
				{
					ThrowOnError(m_libraw->unpack_thumb());
				}

				/// <summary>
				/// Returns the current raw decoder data.
				/// </summary>
				/// <returns>the current raw decoder data.</returns>
				DecoderInfo^ GetDecoderInfo()
				{
					libraw_decoder_info_t info;
					ThrowOnError(m_libraw->get_decoder_info(&info));
					return gcnew DecoderInfo(&info);
				}

				/// <summary>
				/// This call sets internal fast cancel flags. If set, current Raw decoderwill be terminated ASAP.
				/// </summary>
				/// <remarks>
				/// This call is useful if you need to cancel all LibRaw decoders in multithreaded program (e.g. for fast program termination or just for cancel current processing).
				/// </remarks>
				void SetCancelFlag()
				{
					m_libraw->setCancelFlag();
				}

				/// <summary>
				/// This call clears internal fast cancel flags, so (early) terminated LibRaw decoder may work again.
				/// </summary>
				void ClearCancelFlag()
				{
					m_libraw->clearCancelFlag();
				}

				/// <summary>
				/// This call will subtract black level values from RAW data (for suitable RAW data).
				/// colordata.data_maximum and colordata.maximum and black level data (colordata.black and colordata.cblack) will be adjusted too.
				/// </summary>
				/// <remarks>
				/// This call should be used if you post process RAW data by your own code.
				/// LibRaw postprocessing functions will call subtract_black() by oneself.
				/// </remarks>
				void SubtractBlack()
				{
					ThrowOnError(m_libraw->subtract_black());
				}

				/// <summary>
				/// Gets the count of non-fatal data errors (out of range, etc) occured in unpack() stage.
				/// </summary>
				property int ErrorCount
				{
					int get() { return m_libraw->error_count(); }
				}

				// 	Function calls for floating point support

				/// <summary>
				/// Gets whether file contains floating point data.
				/// </summary>
				property bool IsFloatingPoint
				{
					bool get() { return m_libraw->is_floating_point(); }
				}

				/// <summary>
				/// Gets whether data has read (decoded) into memory and not converted to integer data.
				/// </summary>
				property bool HasFloatingPointData
				{
					bool get() { return m_libraw->have_fpdata(); }
				}

				/// <summary>
				/// Converts floating point data to integer.
				/// </summary>
				/// <param name="dmin">The expected minimum data value. Default is 4096.</param>
				/// <param name="dmax">The expected maximum data value. Default is 32767.</param>
				/// <param name="dtarget">The target value. If data maximum is out of dmin..dmax range, then data scaled to set maximum to dtarget. Default is 16383.</param>
				void ConvertFloatingToInteger(float dmin, float dmax, float dtarget)
				{
					m_libraw->convertFloatToInt(dmin, dmax, dtarget);
				}

				// Support for YCC formats (Canon sRAW/mRAW and Nikon Small NEF)

				/// <summary>
				/// Gets whether current image is YCC-based.
				/// </summary>
				property bool IsSraw
				{
					bool get() { return m_libraw->is_sraw(); }
				}

				/// <summary>
				/// Gets whether current image is a Nikon Small NEF file.
				/// </summary>
				property bool IsNikonSraw
				{
					bool get() { return m_libraw->is_nikon_sraw(); }
				}

				/// <summary>
				/// Gets the neutral (gray) point for color channels.
				/// </summary>
				property int SrawMidpoint
				{
					int get() { return m_libraw->sraw_midpoint(); }
				}

				/// <summary>
				/// If LibRaw is compiled with Adobe DNG SDK support and you wish to use this support:
				/// • you need to create own dng_host object
				/// • and pass it to LibRaw object using this function.
				/// </summary>
				/// <param name="dng_host">A pointer to the dng_host object.</param>
				void SetDngHost(IntPtr dng_host)
				{
					m_libraw->set_dng_host((void*)dng_host);
				}

				/// <summary>
				/// Frees the allocated data of LibRaw instance, enabling one to process the next file using the same processor.
				/// Repeated calls of <see href="Recycle">Recycle()</see> are quite possible and do not conflict with anything.
				/// </summary>
				void Recycle()
				{
					m_libraw->recycle();
				}

				/// <summary>
				/// This call closes input datastream with associated data buffer and unblocks opened file. 
				/// </summary>
				void RecycleStream()
				{
					m_libraw->recycle_datastream();
				}

				// Auxiliary

				static property String^ VersionString
				{
					String^ get() { return gcnew String(LibRaw::version()); }
				}

				//static property int VersionNumber
				//{
				//	int get() { return LibRaw::versionNumber(); }
				//}

				static property Version^ Version
				{
					System::Version^ get()
					{
						int version = LibRaw::versionNumber();
						return gcnew System::Version(version >> 16, (byte)(version >> 8), (byte)version);
					}
				}

				static array<String^>^ GetSupportedCameras()
				{
					int count = LibRaw::cameraCount();
					const char** list = LibRaw::cameraList();

					array<String^>^ arr = gcnew array<String^>(count);

					for (size_t i = 0; i < count && list[i]; i++)
						arr[i] = gcnew String(list[i]);

					return arr;
				}

				void SetRawSpeedCameraFile(String^ pathToCamerasXml)
				{
					char* path = StringToChar(pathToCamerasXml);
					int result = m_libraw->set_rawspeed_camerafile(path);

					if (result)
						throw gcnew InvalidOperationException();
				}


				/// <summary>
				/// Gets the past phases of image processing.
				/// </summary>
				property Progress Progress
				{
					LibRawWrapper::Native::Progress get() { return (LibRawWrapper::Native::Progress)m_libraw->imgdata.progress_flags; }
				}

				/// <summary>
				/// Gets the suspicious situations (warnings) that have emerged during image processing.
				/// </summary>
				property Warnings Warnings
				{
					LibRawWrapper::Native::Warnings get() { return (LibRawWrapper::Native::Warnings)m_libraw->imgdata.process_warnings; }
				}

				/// <summary>
				/// Gets the image parameters retrieved from the RAW file.
				/// </summary>
				property ImageParameters^ ImageParameters
				{
					LibRawWrapper::Native::ImageParameters^ get() { return m_data; }
				}

				/// <summary>
				/// Gets the geometrical parameters of the image.
				/// </summary>
				property ImageSizes^ Sizes
				{
					LibRawWrapper::Native::ImageSizes^ get() { return m_sizes; }
				}

				/// <summary>
				/// Gets the description of lens used for the shot.
				/// </summary>
				property LensInfo^ Lens
				{
					LibRawWrapper::Native::LensInfo^ get() { return m_lens; }
				}

				// TODO: libraw_makernotes_t

				/// <summary>
				/// Gets the color data retrieved from the file.
				/// </summary>
				property ColorData^ Color
				{
					LibRawWrapper::Native::ColorData^ get() { return m_color; }
				}

				/// <summary>
				/// Gets the image parameters that have been extracted from the file but are not needed in further file processing.
				/// </summary>
				property OtherInformation^ Other
				{
					LibRawWrapper::Native::OtherInformation^ get() { return m_other; }
				}

				/// <summary>
				/// Gets the information on the preview and the preview data themselves.
				/// </summary>
				/// <remarks>
				/// All fields of this structure but thumbnail itself are filled when <see href="Open">Open()</see> is called. 
				/// Thumbnail readed by <see href="UnpackThumbnail">UnpackThumbnail()</see> call.
				/// </remarks>
				property Thumbnail^ Thumbnail
				{
					LibRawWrapper::Native::Thumbnail^ get() { return m_thumbnail; }
				}

				/// <summary>
				/// Gets a data structure with pointer to raw-data buffer.
				/// </summary>
				property RawData^ RawData
				{
					LibRawWrapper::Native::RawData^ get() { return m_rawdata; }
				}

				/// <summary>
				/// Gets the data structure intended for management of image postprocessing (using the dcraw emulator).
				/// </summary>
				property OutputParameters^ OutputParameters
				{
					LibRawWrapper::Native::OutputParameters^ get() { return m_params; }
				}

				property ShootingInfo^ ShootingInfo
				{
					LibRawWrapper::Native::ShootingInfo^ get() { return m_shootinginfo; }
				}

				property RawParameters^ RawParameters
				{
					LibRawWrapper::Native::RawParameters^ get() { return m_rawparams; }
				}

				/// <summary>
				/// Gets the thumbnail as a bitmap.
				/// </summary>
				/// <returns>the thumbnail as a bitmap; or null if thumbnail not available.</returns>
				BitmapSource^ GetThumbnailBitmap()
				{
					EnsureAtLeast(LIBRAW_PROGRESS_OPEN);

					if ((m_libraw->imgdata.progress_flags & LIBRAW_PROGRESS_THUMB_LOAD) == 0)
						UnpackThumbnail();

					switch (m_libraw->imgdata.thumbnail.tformat)
					{
					case LIBRAW_THUMBNAIL_UNKNOWN:
						return nullptr;

					case LIBRAW_THUMBNAIL_BITMAP:
						return BitmapSource::Create(
							m_libraw->imgdata.thumbnail.twidth,
							m_libraw->imgdata.thumbnail.theight,
							96.0, 96.0, PixelFormats::Rgb24, nullptr,
							(IntPtr)m_libraw->imgdata.thumbnail.thumb,
							m_libraw->imgdata.thumbnail.tlength,
							m_libraw->imgdata.thumbnail.twidth * 3);

					case LIBRAW_THUMBNAIL_BITMAP16:
						return BitmapSource::Create(
							m_libraw->imgdata.thumbnail.twidth,
							m_libraw->imgdata.thumbnail.theight,
							96.0, 96.0, PixelFormats::Rgb48, nullptr,
							(IntPtr)m_libraw->imgdata.thumbnail.thumb,
							m_libraw->imgdata.thumbnail.tlength,
							m_libraw->imgdata.thumbnail.twidth * 3 * sizeof(ushort));

					case LIBRAW_THUMBNAIL_JPEG:
					{
						Stream^ thumbnailStream = gcnew UnmanagedMemoryStream((unsigned char*)m_libraw->imgdata.thumbnail.thumb, m_libraw->imgdata.thumbnail.tlength);
						JpegBitmapDecoder^ jpegDecoder = gcnew JpegBitmapDecoder(thumbnailStream, BitmapCreateOptions::None, BitmapCacheOption::None);
						return jpegDecoder->Frames[0];
					}

					default:
						throw gcnew NotSupportedException();
					}
				}

				// DCRAW LEGACY CALLS

				[Obsolete] void AdjustSizesInfo() { m_libraw->adjust_sizes_info_only(); }
				[Obsolete] void DcrawProcess() { ThrowOnError(m_libraw->dcraw_process()); }
				[Obsolete] void GetMemoryImageFormat([Out] int% width, [Out] int% height, [Out] int% colors, [Out] int% bpp)
				{
					int w, h, c, b;
					m_libraw->get_mem_image_format(&w, &h, &c, &b);
					width = w; height = h; colors = c; bpp = b;
				}
				[Obsolete] void CopyMemoryImage(void* scan0, int stride, int bgr) { ThrowOnError(m_libraw->copy_mem_image(scan0, stride, bgr)); }

				/// <summary>
				/// Gets the processed image as a bitmap.
				/// </summary>
				/// <remarks>If not called before, this method calls <see cref="Unpack()" /> and <see cref="DcrawProcess()" />.</remarks>
				/// <returns>the processed image as a bitmap; or null if image not available.</returns>
				BitmapSource^ GetProcessedBitmap()
				{
					EnsureAtLeast(LIBRAW_PROGRESS_OPEN);

					if ((m_libraw->imgdata.progress_flags & LIBRAW_PROGRESS_LOAD_RAW) == 0)
						Unpack();

					if ((m_libraw->imgdata.progress_flags & LIBRAW_PROGRESS_CONVERT_RGB) == 0)
					{
						OutputParameters->SetGammaTosRGB();	// does not apply to float output
						m_libraw->dcraw_process();
					}

					int width, height, colors, bpp;
					m_libraw->get_mem_image_format(&width, &height, &colors, &bpp);

					if (bpp == 32 && colors == 3)
					{
						// special case since GUID_WICPixelFormat96bppRGBFloat is not supported
						// we want to allocate once, so we allocate for 128bpp, fill 96bpp and then spread it
						int stride96 = width * sizeof(float) * 3;
						int stride128 = width * sizeof(float) * 4;
						float* pImage = (float*)malloc(height * stride128);
						if (!pImage) ThrowOnError(ENOMEM);
						ThrowOnError(m_libraw->copy_mem_image(pImage, stride96, 0));

						try
						{
							float* pSrc = pImage + width * 3 * height - 3;
							float* pDst = pImage + width * 4 * height - 4;
							while (pSrc < pDst)
							{
								pDst[2] = pSrc[2]; // TODO: due to caching it might be faster to do 0 1 2 when safe
								pDst[1] = pSrc[1];
								pDst[0] = pSrc[0];
								pSrc -= 3;
								pDst -= 4;
							}
							return BitmapSource::Create(width, height, 96.0, 96.0, PixelFormats::Rgb128Float, nullptr, (IntPtr)pImage, height * stride128, stride128);
						}
						finally
						{
							free(pImage);
						}
					}

					int ret = 0;
					libraw_processed_image_t* image = m_libraw->dcraw_make_mem_image(&ret);
					ThrowOnError(ret);

					if (!image || !image->data)
						return nullptr;

					try
					{
						switch (image->bits)
						{
						case 8:
						{
							PixelFormat format = PixelFormats::Rgb24;
							if (image->colors == 1) format = PixelFormats::Gray8;
							else if (image->colors != 3) throw gcnew NotSupportedException(L"Only single or three-color formats are supported.");

							return BitmapSource::Create(
								image->width,
								image->height,
								96.0, 96.0, format, nullptr,
								(IntPtr)image->data,
								image->data_size,
								image->width * sizeof(byte) * image->colors);
						}
						case 16:
						{
							PixelFormat format = PixelFormats::Rgb48;
							if (image->colors == 1) format = PixelFormats::Gray16;
							else if (image->colors != 3) throw gcnew NotSupportedException(L"Only single or three-color formats are supported.");

							return BitmapSource::Create(
								image->width,
								image->height,
								96.0, 96.0, format, nullptr,
								(IntPtr)image->data,
								image->data_size,
								image->width * sizeof(ushort) * image->colors);
						}
						case 32:
						{
							PixelFormat format = PixelFormats::Gray32Float;
							if (image->colors != 1) throw gcnew NotSupportedException(L"Only single or three-color formats are supported.");

							return BitmapSource::Create(
								image->width,
								image->height,
								96.0, 96.0, format, nullptr,
								(IntPtr)image->data,
								image->data_size,
								image->width * sizeof(float) * image->colors);
						}
						default:
							throw gcnew NotSupportedException(L"Only 8-bit or 16-bit formats are supported.");
						}
					}
					finally
					{
						m_libraw->dcraw_clear_mem(image);
					}
				}

				/// <summary>
				/// Occurs during RAW postprocessing.
				/// </summary>
				event LibRawProgressEventHandler^ ProgressChanged
				{
					void add(LibRawProgressEventHandler^ value)
					{
						bool wasNull = m_progress_handler == nullptr;
						m_progress_handler = static_cast<LibRawProgressEventHandler^>(Delegate::Combine(m_progress_handler, value));

						if (m_progress_handler != nullptr & wasNull)
						{
							ProgressCallback^ callbackDelegate = gcnew ProgressCallback(this, &LibRawProcessor::OnProgressCallback);
							m_progress_callback = GCHandle::Alloc(callbackDelegate); // no need to pin, just keep alive

							IntPtr callbackPointer = Marshal::GetFunctionPointerForDelegate(callbackDelegate);
							progress_callback callback = static_cast<progress_callback>(callbackPointer.ToPointer());
							m_libraw->set_progress_handler(callback, NULL);
						}
					}

					void remove(LibRawProgressEventHandler^ value)
					{
						bool wasNull = m_progress_handler == nullptr;
						m_progress_handler = static_cast<LibRawProgressEventHandler^>(Delegate::Remove(m_progress_handler, value));

						if (m_progress_handler == nullptr & !wasNull)
						{
							m_libraw->set_progress_handler(NULL, NULL);
							m_progress_callback.Free();
						}
					}

					void raise(Object^ sender, LibRawProgressEventArgs^ e)
					{
						LibRawProgressEventHandler^ handler = m_progress_handler;
						if (handler != nullptr)
							handler->Invoke(this, e);
					}
				}

			protected:
				void EnsureAtLeast(unsigned int progress)
				{
					if ((m_libraw->imgdata.progress_flags & LIBRAW_PROGRESS_THUMB_MASK) < progress)
						throw gcnew InvalidOperationException();
				}

				bool OnProgress(Native::Progress progress, int iteration, int expected)
				{
					LibRawProgressEventArgs^ e = gcnew LibRawProgressEventArgs(progress, iteration, expected);
					ProgressChanged(this, e);
					return e->Cancel;
				}

				static bool ThrowOnError(int error)
				{
					if (error == LIBRAW_SUCCESS)
						return true;

					if (error > 0)
						throw gcnew System::ComponentModel::Win32Exception(error, gcnew String(strerror(error)));

					const char* pMessage = LibRaw::strerror(error);
					String^ message = gcnew String(pMessage);

					switch (error)
					{
						// non-fatal

					case LIBRAW_UNSPECIFIED_ERROR:
						throw gcnew Exception(message);

					case LIBRAW_FILE_UNSUPPORTED:
						throw gcnew NotSupportedException(message);

					case LIBRAW_REQUEST_FOR_NONEXISTENT_IMAGE:
						throw gcnew IndexOutOfRangeException(message);

					case LIBRAW_OUT_OF_ORDER_CALL:
						throw gcnew InvalidOperationException(message);

					case LIBRAW_NO_THUMBNAIL:
						return false;

					case LIBRAW_UNSUPPORTED_THUMBNAIL:
						throw gcnew NotSupportedException(message);

					case LIBRAW_INPUT_CLOSED:
						throw gcnew InvalidOperationException(message);

					case LIBRAW_NOT_IMPLEMENTED:
						throw gcnew NotImplementedException(message);

						// fatal

					case LIBRAW_UNSUFFICIENT_MEMORY:
						throw gcnew OutOfMemoryException(message);

					case LIBRAW_DATA_ERROR:
						throw gcnew InvalidDataException(message);

					case LIBRAW_IO_ERROR:
						throw gcnew IOException(message);

					case LIBRAW_CANCELLED_BY_CALLBACK:
						throw gcnew OperationCanceledException(message);

					case LIBRAW_BAD_CROP:
						throw gcnew ArgumentOutOfRangeException(message);

					case LIBRAW_TOO_BIG:
						throw gcnew InvalidOperationException(message);

					case LIBRAW_MEMPOOL_OVERFLOW:
						throw gcnew OutOfMemoryException(message);

					default:
						throw gcnew Exception(message);
					}
				}

			private:
				delegate int ProgressCallback(void*, LibRaw_progress, int, int);

				int OnProgressCallback(void* data, LibRaw_progress stage, int iteration, int expected)
				{
					return OnProgress((Native::Progress)stage, iteration, expected);
				}

				::LibRaw* m_libraw;
				LibRawWrapper::Native::ImageParameters^ m_data;
				LibRawWrapper::Native::ImageSizes^ m_sizes;
				LibRawWrapper::Native::LensInfo^ m_lens;
				LibRawWrapper::Native::ColorData^ m_color;
				LibRawWrapper::Native::OtherInformation^ m_other;
				LibRawWrapper::Native::Thumbnail^ m_thumbnail;
				LibRawWrapper::Native::RawData^ m_rawdata;
				LibRawWrapper::Native::OutputParameters^ m_params;
				LibRawWrapper::Native::RawParameters^ m_rawparams;
				LibRawWrapper::Native::ShootingInfo^ m_shootinginfo;

				GCHandle m_progress_callback;
				LibRawProgressEventHandler^ m_progress_handler;

			};
		}
	}
}