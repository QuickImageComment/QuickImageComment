#pragma once

#include "LibRawProcessor.h"

using namespace System;
using namespace System::Windows::Media::Imaging;
using namespace System::Collections::Generic;

namespace HurlbertVisionLab {
	namespace LibRawWrapper
	{
		public ref class LibRawFrameCollection : IReadOnlyCollection<BitmapFrame^>
		{
		internal:
			value struct LazyFrameEnumerator : IEnumerator<BitmapFrame^>
			{
			public:
				LazyFrameEnumerator(LibRawFrameCollection^ collection)
				{
					_collection = collection;
					_index = -1;
				}

				property BitmapFrame^ Current
				{
					virtual BitmapFrame^ get()
					{
						if (_index < 0 || _index >= _collection->Count)
							throw gcnew InvalidOperationException();

						return _collection[_index];
					}
				}
				virtual void Reset()
				{
					_index = -1;
				}
				virtual bool MoveNext()
				{
					if (_index >= _collection->Count)
						return false;

					return ++_index < _collection->Count;
				}

			private:
				property Object^ IEnumerator_Current
				{
					virtual Object^ get() sealed = System::Collections::IEnumerator::Current::get
					{
						return Current;
					}
				}

				int _index;
				LibRawFrameCollection^ _collection;
			};

		internal:
			LibRawFrameCollection(Native::LibRawProcessor^ libraw)
			{
				_libraw = libraw;
				_frames = gcnew array<BitmapFrame^>(_libraw->ImageParameters->RawCount);
			}

		public:
			property int Count
			{
				virtual int get() { return _libraw->ImageParameters->RawCount; }
			}

			property BitmapFrame^ default[int]
			{
				BitmapFrame ^ get(int index)
				{
					if (index < 0 || index > Count)
						throw gcnew IndexOutOfRangeException();

					if (_frames[index] == nullptr)
						_frames[index] = GetFrame(index);

					return _frames[index];
				}
			}

				virtual IEnumerator<BitmapFrame^>^ GetEnumerator()
			{
				return gcnew LazyFrameEnumerator(this);
			}

		private:
			BitmapFrame^ GetFrame(int index)
			{
				_libraw->RawParameters->ShotSelect = index;
				BitmapSource^ bitmap = _libraw->GetProcessedBitmap();
				return BitmapFrame::Create(bitmap); // TODO: metadata, color context
			}

			virtual System::Collections::IEnumerator^ IEnumerator_GetEnumerator() sealed = System::Collections::IEnumerable::GetEnumerator
			{
				return GetEnumerator();
			}

			Native::LibRawProcessor^ _libraw;
			array<BitmapFrame^>^ _frames;
		};
	}
}