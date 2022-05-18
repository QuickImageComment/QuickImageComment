#pragma once

#include <libraw.h>
#include "Progress.h"

using namespace System;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Provides information for the <see cref="LibRawProcessor.ProgressChanged" /> event.
			/// </summary>
			public ref class LibRawProgressEventArgs : EventArgs
			{
			public:
				/// <summary>
				/// Initializes a new instance of the LibRawProgressEventArgs class.
				/// </summary>
				/// <param name="stage">Current processing stage.</param>
				/// <param name="iteration">Iteration number within current stage.</param>
				/// <param name="expected">Expected number of iterations on current stage.</param>
				LibRawProgressEventArgs(Progress stage, int iteration, int expected)
				{
					_stage = stage;
					_iteration = iteration;
					_expected = expected;
				}

				/// <summary>
				/// Gets or sets whether to terminate the current image processing.
				/// </summary>
				/// <remarks>
				/// If set to true, all processing will be cancelled immediately and all resources will be returned to system by recycle() call.
				/// Current call will throw <see cref="OperationCanceledException" />.
				/// </remarks>
				property bool Cancel
				{
					bool get() { return _cancel; }
					void set(bool value) { _cancel = value; }
				}

				/// <summary>
				/// Gets the iteration number within current stage (from 0 to <see cref="Expected" />-1).
				/// </summary>
				property int Iteration
				{
					int get() { return _iteration; }
				}

				/// <summary>
				/// Gets the expected number of iterations on current stage.
				/// </summary>
				property int Expected
				{
					int get() { return _expected; }
				}

				/// <summary>
				/// Gets the progress of current stage in percentage.
				/// </summary>
				property double Percent
				{
					double get() { return (_iteration + 1) / (double)_expected; }
				}

				/// <summary>
				/// Gets the current processing stage.
				/// </summary>
				/// <remarks>
				/// Not all processing stages are covered by callback calls.
				/// </remarks>
				property Progress Stage
				{
					Progress get() { return _stage; }
				}

				/// <summary>
				/// Gets the current processing stage description.
				/// </summary>
				property String^ Description
				{
					String^ get() { return gcnew String(LibRaw::strprogress((LibRaw_progress)_stage)); }
				}

			private:
				bool _cancel;
				int _iteration;
				int _expected;
				Progress _stage;
			};

			/// <summary>
			/// Represents the method that handles the <see cref="LibRawProcessor.ProgressChanged" /> event.
			/// </summary>
			/// <param name="sender">The object that raised the event.</param>
			/// <param name="e">Information about the event.</param>
			public delegate void LibRawProgressEventHandler(Object^ sender, LibRawProgressEventArgs^ e);
		}
	}
}