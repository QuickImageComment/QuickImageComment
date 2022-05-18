#pragma once

using namespace System;
using namespace System::Windows::Media::Imaging;

namespace HurlbertVisionLab {
	namespace LibRawWrapper
	{
		private ref class LibRawCodecInfo : BitmapCodecInfo
		{
		public:
			property String^ Author
			{
				virtual String^ get() override { return L"LibRaw LLC (info@libraw.org)"; }
			}
			property Guid ContainerFormat
			{
				virtual Guid get() override { return Guid("{c1fc85cb-d64f-478b-a4ec-69adc9ee1392}"); } // from Microsoft Camera Raw Decoder
			}
			property String^ DeviceManufacturer
			{
				virtual String^ get() override { return _deviceManufacturer; }
			}
			property String^ DeviceModels
			{
				virtual String^ get() override { return _deviceModels; }
			}
			property String^ FileExtensions
			{
				virtual String^ get() override { return L".ARW,.CR2,.CRW,.ERF,.KDC,.MRW,.NEF,.NRW,.ORF,.PEF,.RAF,.RAW,.RW2,.RWL,.SR2,.SRW,.DNG"; } // from Microsoft Camera Raw Decoder
			}
			property String^ FriendlyName
			{
				virtual String^ get() override { return L"LibRaw Decoder"; }
			}
			property String^ MimeTypes
			{
				virtual String^ get() override { return L"image/ARW,image/CR2,image/CRW,image/ERF,image/KDC,image/MRW,image/NEF,image/NRW,image/ORF,image/PEF,image/RAF,image/RAW,image/RW2,image/RWL,image/SR2,image/SRW,image/DNG"; } // from Microsoft Camera Raw Decoder
			}
			property System::Version^ SpecificationVersion
			{
				virtual System::Version^ get() override { return gcnew System::Version(1, 0); }
			}
			property bool SupportsAnimation
			{
				virtual bool get() override { return false; }
			}
			property bool SupportsLossless
			{
				virtual bool get() override { return true; }
			}
			property bool SupportsMultipleFrames
			{
				virtual bool get() override { return true; }
			}
			property System::Version^ Version
			{
				virtual System::Version^ get() override { return _librawVersion; }
			}

		internal:
			LibRawCodecInfo(String^ deviceManufacturer, String^ deviceModels, System::Version^ version)
			{
				_deviceManufacturer = deviceManufacturer;
				_deviceModels = deviceModels;
				_librawVersion = version;
			}

		private:
			String^ _deviceManufacturer;
			String^ _deviceModels;
			System::Version^ _librawVersion;
		};
	}
}