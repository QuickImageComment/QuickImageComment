#pragma once

#include <vcclr.h>
#include <libraw.h>
#include "LibRawWrapper.h"
#include "Pixel4.h"
#include "Flip.h"
#include "Interpolation.h"
#include "HighlightMode.h"
#include "UseCameraMatrix.h"
#include "OutputColorspace.h"
#include "OutputFormat.h"

using namespace System;
using namespace System::Windows;

namespace HurlbertVisionLab {
	namespace LibRawWrapper {
		namespace Native
		{
			/// <summary>
			/// Structure for management of dcraw-compatible calls.
			/// </summary>
			public ref class OutputParameters
			{
			public:
				/// <summary>
				/// Gets or sets the 4 numbers corresponding to the coordinates (in pixels) of the rectangle that is used to calculate the white balance.
				/// </summary>
				property Int32Rect GreyBox
				{
					Int32Rect get()
					{
						return Int32Rect(
							m_params->greybox[0],
							m_params->greybox[1],
							m_params->greybox[2],
							m_params->greybox[3]);
					}
					void set(Int32Rect value)
					{
						m_params->greybox[0] = value.X;
						m_params->greybox[1] = value.Y;
						m_params->greybox[2] = value.Width;
						m_params->greybox[3] = value.Height;
					}
				}

				/// <summary>
				/// Gets or sets the image cropping rectangle.
				/// </summary>
				/// <remarks>
				/// All coordinates are applied before any image rotation.
				/// </remarks>
				property Int32Rect CropBox
				{
					Int32Rect get()
					{
						return Int32Rect(
							m_params->cropbox[0],
							m_params->cropbox[1],
							m_params->cropbox[2],
							m_params->cropbox[3]);
					}
					void set(Int32Rect value)
					{
						m_params->cropbox[0] = value.X;
						m_params->cropbox[1] = value.Y;
						m_params->cropbox[2] = value.Width;
						m_params->cropbox[3] = value.Height;
					}
				}

				/// <summary>
				/// Gets or sets the chromatic aberration multipliers.
				/// </summary>
				/// <remarks>
				/// For some formats, it affects RAW data reading, since correction of aberrations changes the output size.
				/// </remarks>
				property Pixel4<double> ChromaticAberration
				{
					Pixel4<double> get()
					{
						return Pixel4<double>
							(
								m_params->aber[0],
								m_params->aber[1],
								m_params->aber[2],
								m_params->aber[3]
								);
					}
					void set(Pixel4<double> value)
					{
						m_params->aber[0] = value.R;
						m_params->aber[1] = value.G1;
						m_params->aber[2] = value.B;
						m_params->aber[3] = value.G2;
					}
				}

				/// <summary>
				/// Gets the user gamma-curve.
				/// </summary>
				property array<double>^ Gamma
				{
					array<double>^ get() { return FixedToArray(m_params->gamm, sizeof(m_params->gamm)); }
				}

				/// <summary>
				/// Sets the user gamma-curve.
				/// </summary>
				/// <param name="gamma">The gamma value</param>
				/// <param name="slope">The slope for linear part (so called toe slope). Set to zero for sipmle curve.</param>
				void SetGamma(double gamma, double slope)
				{
					m_params->gamm[0] = 1.0 / gamma;
					m_params->gamm[1] = slope;
				}

				/// <summary>
				/// Sets the gamma to linear curve.
				/// </summary>
				void SetGammaToLinear() { SetGamma(1, 1); }
				/// <summary>
				/// Sets the gamma to sRGB curve.
				/// </summary>
				void SetGammaTosRGB() { SetGamma(2.4, 12.92); }
				/// <summary>
				/// Sets the gamma to rec. BT.709 curve (the default).
				/// </summary>
				void SetGammaToBT709() { SetGamma(2.222, 4.5); }

				/// <summary>
				/// Gets or sets the multipliers of the user's white balance.
				/// </summary>
				property Nullable<Pixel4<float>> UserMultipliers
				{
					Nullable<Pixel4<float>> get()
					{
						if (m_params->user_mul[0])
							return Nullable<Pixel4<float>>(Pixel4<float>
								(
									m_params->user_mul[0],
									m_params->user_mul[1],
									m_params->user_mul[2],
									m_params->user_mul[3]
									));

						return Nullable<Pixel4<float>>();
					}
					void set(Nullable<Pixel4<float>> value)
					{
						if (value.HasValue)
						{
							m_params->user_mul[0] = value.Value.R;
							m_params->user_mul[1] = value.Value.G1;
							m_params->user_mul[2] = value.Value.B;
							m_params->user_mul[3] = value.Value.G2;
						}
						else
							memset(m_params->user_mul, 0, sizeof(m_params->user_mul));
					}
				}

				/// <summary>
				/// Gets or sets the brightness (default 1.0).
				/// </summary>
				property float Brightness
				{
					float get() { return m_params->bright; }
					void set(float value) { m_params->bright = value; }
				}

				/// <summary>
				/// Gets or sets the parameter for noise reduction through wavelet denoising.
				/// </summary>
				property float DenoisingThreshold
				{
					float get() { return m_params->threshold; }
					void set(float value) { m_params->threshold = value; }
				}

				/// <summary>
				/// Gets or sets whether to output the image in 50% size. 
				/// </summary>
				/// <remarks>
				/// For some formats, it affects RAW data reading.
				/// </remarks>
				property bool HalfSize
				{
					bool get() { return m_params->half_size; }
					void set(bool value) { m_params->half_size = value; }
				}

				/// <summary>
				/// Gets or sets whether interpolation is separate for two green components.
				/// </summary>
				property bool FourColourRGB
				{
					bool get() { return m_params->four_color_rgb; }
					void set(bool value) { m_params->four_color_rgb = value; }
				}

				/// <summary>
				/// Gets or sets the highlight mode.
				/// </summary>
				property LibRawWrapper::Native::HighlightMode HighlightMode
				{
					LibRawWrapper::Native::HighlightMode get()
					{
						return m_params->highlight >= 3
							? LibRawWrapper::Native::HighlightMode::Rebuild
							: (LibRawWrapper::Native::HighlightMode)m_params->highlight;
					}
					void set(LibRawWrapper::Native::HighlightMode value) { m_params->half_size = (int)value; }
				}

				/// <summary>
				/// Gets or sets the highlight rebuilding grow factor. 
				/// </summary>
				property int HighlightRebuildFactor
				{
					int get() { return m_params->highlight; }
					void set(int value) { m_params->highlight = value; }
				}

				/// <summary>
				/// Gets or sets whether to use automatic white balance obtained after averaging over the entireimage.
				/// </summary>
				property bool UseAutomaticWhiteBalance
				{
					bool get() { return m_params->use_auto_wb; }
					void set(bool value) { m_params->use_auto_wb = value; }
				}

				/// <summary>
				/// Gets or sets whether to use the white balance from the camera, if possible.
				/// </summary>
				/// <remarks>
				/// If camera-recorded WB is not available, dcraw_process() will fallback to: <ul>
				/// <li>Auto - WB if bit LIBRAW_PROCESSING_CAMERAWB_FALLBACK_TO_DAYLIGHT is not set in params.raw_processing_options (or for the rare specific case: no valid WB index was parsed from CRW file)</li>
				/// <li>Daylight - WB if abovementioned bit is not set.</li></ul>
				/// </remarks>
				property bool UseCameraWhiteBalance
				{
					bool get() { return m_params->use_camera_wb; }
					void set(bool value) { m_params->use_camera_wb = value; }
				}

				/// <summary>
				/// Gets or sets whether to use the embedded color profile.
				/// </summary>
				property LibRawWrapper::Native::UseCameraMatrix UseCameraMatrix
				{
					LibRawWrapper::Native::UseCameraMatrix get() { return (LibRawWrapper::Native::UseCameraMatrix)m_params->use_camera_matrix; }
					void set(LibRawWrapper::Native::UseCameraMatrix value) { m_params->use_camera_matrix = (int)value; }
				}

				/// <summary>
				/// gets or sets the output colorspace.
				/// </summary>
				property LibRawWrapper::Native::OutputColorspace OutputColorspace
				{
					LibRawWrapper::Native::OutputColorspace get() { return (LibRawWrapper::Native::OutputColorspace)m_params->output_color; }
					void set(LibRawWrapper::Native::OutputColorspace value) { m_params->output_color = (int)value; }
				}

				/// <summary>
				/// Gets or sets the path to output profile ICC file (used only if LibRaw compiled with LCMS support).
				/// </summary>
				property String^ OutputProfileFilename
				{
					String^ get()
					{
						if (!m_params->output_profile) return nullptr;
						return gcnew String(m_params->output_profile);
					}
					void set(String^ value)
					{
						free(m_params->output_profile);
						m_params->output_profile = StringToChar(value);
					}
				}

				/// <summary>
				/// Gets or sets the path to input (camera) profile ICC file (or 'embed' for embedded profile). 
				/// Used only if LCMS support compiled in.
				/// </summary>
				property String^ CameraProfileFilename
				{
					String^ get()
					{
						if (!m_params->camera_profile) return nullptr;
						return gcnew String(m_params->camera_profile);
					}
					void set(String^ value)
					{
						free(m_params->camera_profile);
						m_params->camera_profile = StringToChar(value);
					}
				}

				/// <summary>
				/// Gets or sets the path to file with bad pixels map (in dcraw format: "column row date-of-pixel-death-in-UNIX-format", one pixel per row).
				/// </summary>
				property String^ BadPixelsFilename
				{
					String^ get()
					{
						if (!m_params->bad_pixels) return nullptr;
						return gcnew String(m_params->bad_pixels);
					}
					void set(String^ value)
					{
						free(m_params->bad_pixels);
						m_params->bad_pixels = StringToChar(value);
					}
				}

				/// <summary>
				/// Gets or sets the path to dark frame file (in 16-bit PGM format)
				/// </summary>
				property String^ DarkFrameFilename
				{
					String^ get()
					{
						if (!m_params->dark_frame) return nullptr;
						return gcnew String(m_params->dark_frame);
					}
					void set(String^ value)
					{
						free(m_params->dark_frame);
						m_params->dark_frame = StringToChar(value);
					}
				}

				/// <summary>
				/// Gets or sets whether the output is 8 bit (default)/16 bit.
				/// </summary>
				property int OutputBitsPerPixel
				{
					int get() { return m_params->output_bps; }
					void set(int value) { m_params->output_bps = value; }
				}

				/// <summary>
				/// Get or sets whether to otuput PPM/TIFF.
				/// </summary>
				property LibRawWrapper::Native::OutputFormat OutputFormat
				{
					LibRawWrapper::Native::OutputFormat get() { return (LibRawWrapper::Native::OutputFormat)m_params->output_tiff; }
					void set(LibRawWrapper::Native::OutputFormat value) { m_params->output_tiff = (int)value; }
				}

				/// <summary>
				/// Gets or sets user flip image.
				/// </summary>
				/// <remarks>
				/// For some formats, affects RAW data reading, e.g., unpacking of thumbnails from Kodak cameras.
				/// </remarks>
				property Nullable<LibRawWrapper::Native::Flip> UserFlip
				{
					Nullable<LibRawWrapper::Native::Flip> get()
					{
						if (m_params->user_flip == -1) return Nullable<LibRawWrapper::Native::Flip>();
						return Nullable<LibRawWrapper::Native::Flip>((LibRawWrapper::Native::Flip)m_params->user_flip);
					}
					void set(Nullable<LibRawWrapper::Native::Flip> value)
					{
						m_params->user_flip = value.HasValue ? (int)value.Value : -1;
					}
				}

				/// <summary>
				/// Gets or sets the interpolation quality.
				/// </summary>
				property Nullable<LibRawWrapper::Native::Interpolation> UserQuality
				{
					Nullable<LibRawWrapper::Native::Interpolation> get()
					{
						if (m_params->user_qual == -1) return Nullable<LibRawWrapper::Native::Interpolation>();
						return Nullable<LibRawWrapper::Native::Interpolation>((LibRawWrapper::Native::Interpolation)m_params->user_qual);
					}
					void set(Nullable<LibRawWrapper::Native::Interpolation> value)
					{
						m_params->user_qual = value.HasValue ? (int)value.Value : -1;
					}
				}

				/// <summary>
				/// Gets or sets user black level.
				/// </summary>
				property Nullable<int> UserBlackLevel
				{
					Nullable<int> get()
					{
						if (m_params->user_black == -1) return Nullable<int>();
						return Nullable<int>(m_params->user_black);
					}
					void set(Nullable<int> value)
					{
						m_params->user_black = value.HasValue ? value.Value : -1;
					}
				}

				/// <summary>
				/// Gets or sets per-channel corrections to <see cref="UserBlackLevel" />.
				/// </summary>
				property Nullable<Pixel4<int>> UserBlackCorrection
				{
					Nullable<Pixel4<int>> get()
					{
						if (m_params->user_cblack[0] <= -1000000 &&
							m_params->user_cblack[1] <= -1000000 &&
							m_params->user_cblack[2] <= -1000000 &&
							m_params->user_cblack[3] <= -1000000) return Nullable<Pixel4<int>>();

						return Nullable<Pixel4<int>>
							(
								Pixel4<int>
								(
									m_params->user_cblack[0],
									m_params->user_cblack[1],
									m_params->user_cblack[2],
									m_params->user_cblack[3]
									)
								);
					}
					void set(Nullable<Pixel4<int>> value)
					{
						if (value.HasValue)
						{
							m_params->user_cblack[0] = value.Value.R;
							m_params->user_cblack[1] = value.Value.G1;
							m_params->user_cblack[2] = value.Value.B;
							m_params->user_cblack[3] = value.Value.G2;
						}
						else
						{
							m_params->user_cblack[0] =
								m_params->user_cblack[1] =
								m_params->user_cblack[2] =
								m_params->user_cblack[3] = -1000001;
						}
					}
				}

				/// <summary>
				/// Gets or sets saturation adjustment.
				/// </summary>
				property Nullable<int> UserSaturation
				{
					Nullable<int> get()
					{
						if (m_params->user_sat == -1) return Nullable<int>();
						return Nullable<int>(m_params->user_sat);
					}
					void set(Nullable<int> value)
					{
						m_params->user_sat = value.HasValue ? value.Value : -1;
					}
				}

				/// <summary>
				/// Gets or sets number of median filter passes.
				/// </summary>
				property int MedianPasses
				{
					int get() { return m_params->med_passes; }
					void set(int value) { m_params->med_passes = value; }
				}

				/// <summary>
				/// Gets or sets whether to use automatic increase of brightness by histogram.
				/// </summary>
				property bool NoAutoBrightness
				{
					bool get() { return m_params->no_auto_bright; }
					void set(bool value) { m_params->no_auto_bright = value; }
				}

				/// <summary>
				/// Gets or sets the portion of clipped pixels when auto brighness increase is used.
				/// </summary>
				/// <remarks>
				/// Default value is 0.01 (1%) for dcraw compatibility.
				/// Recommended value for modern low-noise multimegapixel cameras depends on shooting style.
				/// Values in 0.001-0.00003 range looks reasonable.
				/// </remarks>
				property float AutoBrightnessThreshold
				{
					float get() { return m_params->auto_bright_thr; }
					void set(float value) { m_params->auto_bright_thr = value; }
				}

				/// <summary>
				/// This parameters controls auto-adjusting of maximum value based on channel_maximum[] data, calculated from real frame data.
				/// If calculated maximum is greater than adjust_maximum_thr*maximum, than maximum is set to calculated_maximum.
				/// </summary>
				/// <remarks>
				/// Default: 0.75. If you set this value above 0.99999, than default value will be used. 
				/// If you set this value below 0.00001, than no maximum adjustment will be performed. 
				/// Adjusting maximum should not damage any picture (esp. if you use default value) and is very useful for correcting channel overflow problems (magenta clouds on landscape shots, green-blue highlights for indoor shots).
				/// </remarks>
				property float AdjustMaximumThreshold
				{
					float get() { return m_params->adjust_maximum_thr; }
					void set(float value) { m_params->adjust_maximum_thr = value; }
				}

				/// <summary>
				/// Gets or sets whether to use rotation for cameras on a Fuji sensor.
				/// </summary>
				property bool UseFujiRotate
				{
					bool get() { return m_params->use_fuji_rotate; }
					void set(bool value) { m_params->use_fuji_rotate = value; }
				}

				/// <summary>
				/// Gets or sets whether to fix green channels disbalance.
				/// </summary>
				/// <remarks>
				/// Green matching requires additional memory for image data.
				/// </remarks>
				property bool GreenMatching
				{
					bool get() { return m_params->green_matching; }
					void set(bool value) { m_params->green_matching = value; }
				}

				/// <summary>
				/// Gets or sets the number of DCB correction passes.
				/// </summary>
				/// <remarks>
				/// Default is -1 (no correction).
				/// Useful only for DCB interpolation.
				/// </remarks>
				property int DcbIterations
				{
					int get() { return m_params->dcb_iterations; }
					void set(int value) { m_params->dcb_iterations = value; }
				}

				/// <summary>
				/// Gets or sets whether DCB interpolation will enhance interpolated colors.
				/// </summary>
				property bool DcbEnhance
				{
					bool get() { return m_params->dcb_enhance_fl; }
					void set(bool value) { m_params->dcb_enhance_fl = value; }
				}

				/// <summary>
				/// Gets or sets the level of FBDD noise reduction before demosaic.
				/// </summary>
				property int FbddNoiseReduction
				{
					int get() { return m_params->fbdd_noiserd; }
					void set(int value) { m_params->fbdd_noiserd = value; }
				}

				/// <summary>
				/// Gets or sets whether the exposure correction before demosaic is active.
				/// </summary>
				property bool CorrectExposure
				{
					bool get() { return m_params->exp_correc; }
					void set(bool value) { m_params->exp_correc = value; }
				}

				/// <summary>
				/// Gets or sets the exposure shift in linear scale.
				/// </summary>
				/// <remarks>
				/// Usable range from 0.25 (2-stop darken) to 8.0 (3-stop lighter).
				/// Default: 1.0 (no exposure shift).
				/// </remarks>
				property float ExposureShift
				{
					float get() { return m_params->exp_shift; }
					void set(float value) { m_params->exp_shift = value; }
				}

				/// <summary>
				/// Gets or sets whether to preserve highlights when lighten the image.
				/// </summary>
				/// <remarks>
				/// Usable range from 0.0 (no preservation) to 1.0 (full preservation). 
				/// 0.0 is the default value.
				/// </remarks>
				property float ExposurePreserveHighlights
				{
					float get() { return m_params->exp_preser; }
					void set(float value) { m_params->exp_preser = value; }
				}

				/// <summary>
				/// Gets or sets whether to use pixel values scaling (call to LibRaw::scale_colors()) in LibRaw::dcraw_process().
				/// </summary>
				/// <remarks>
				/// This is special use value because white balance is performed in scale_colors(), so skipping it will result in non-balanced image.
				/// This setting is targeted to use with <see cref="NoInterpolation" />, or with own interpolation callback call.
				/// </remarks>
				property bool NoAutoScale
				{
					bool get() { return m_params->no_auto_scale; }
					void set(bool value) { m_params->no_auto_scale = value; }
				}

				/// <summary>
				/// Gets or sets whether to disable demosaic code in LibRaw::dcraw_process().
				/// </summary>
				property bool NoInterpolation
				{
					bool get() { return m_params->no_interpolation; }
					void set(bool value) { m_params->no_interpolation = value; }
				}

			internal:
				OutputParameters(libraw_output_params_t* params) : m_params(params) { }

			private:
				libraw_output_params_t* m_params;
			};
		}
	}
}
