// Jacob Klint, 6/2007
using DirectShowLib;
using DirectShowLib.DES;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace User.DirectShow
{
    /// <summary>
    /// Retrieves individual frames from a video file and returns them as bitmaps.
    /// </summary>
    public class FrameGrabber : IEnumerable<FrameGrabber.Frame>
    {
        // underlying variables for properties
        private string fileName;
        private int frameCount;
        private double frameRate;
        private int height;

        // private variables
        private IMediaDet mediaDet;
        private double mediaLength;
        private VideoInfoHeader videoInfo;
        private int width;

        /// <summary>
        /// Creates a FrameGrabber based on the specified video file.
        /// </summary>
        public FrameGrabber(string fileName)
        {
            FileName = fileName;
        }


        /// <summary>
        /// Creates a FrameGrabber with no video file set.  Set FileName before calling other methods.
        /// </summary>
        public FrameGrabber()
            : this("")
        {
            // nothing to do here
        }

        /// <summary>
        ///  Gets or sets the full path of the video file from which to grab frames.
        /// </summary>
        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                mediaDet = null;
                fileName = value;

                if (File.Exists(fileName))
                {
                    AMMediaType mediaType = null;

                    try
                    {
                        mediaDet = (IMediaDet)new MediaDet();
                        DsError.ThrowExceptionForHR(mediaDet.put_Filename(fileName));

                        // find the video stream in the file
                        int index = 0;
                        Guid type = Guid.Empty;
                        while (type != MediaType.Video)
                        {
                            mediaDet.put_CurrentStream(index++);
                            mediaDet.get_StreamType(out type);
                        }

                        // retrieve some measurements from the video
                        mediaDet.get_FrameRate(out frameRate);

                        mediaType = new AMMediaType();
                        mediaDet.get_StreamMediaType(mediaType);
                        videoInfo = (VideoInfoHeader)Marshal.PtrToStructure(mediaType.formatPtr, typeof(VideoInfoHeader));
                        DsUtils.FreeAMMediaType(mediaType);
                        mediaType = null;
                        width = videoInfo.BmiHeader.Width;
                        height = videoInfo.BmiHeader.Height;

                        mediaDet.get_StreamLength(out mediaLength);
                        frameCount = (int)(frameRate * mediaLength);
                    }
                    catch (Exception e)
                    {
                        if (mediaType != null)
                        {
                            DsUtils.FreeAMMediaType(mediaType);
                        }

                        fileName = "";

                        throw new ArgumentException(string.Format("unable to open the file \"{0}\", DirectShow reported the following error: {1}", value, e.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the total number of frames in the video file.
        /// </summary>
        public int FrameCount
        {
            get
            {
                return frameCount;
            }
        }

        /// <summary>
        /// Gets the framerate of the video file.  Some videos always report "0" regardless of their actual value.
        /// </summary>
        public double FrameRate
        {
            get
            {
                return frameRate;
            }
        }

        /// <summary>
        /// Gets the horizontal dimension of the video file in pixels.
        /// </summary>
        public int Width
        {
            get
            {
                return width;
            }
        }

        /// <summary>
        /// Gets the vertical dimension of the video file in pixels.		
        /// </summary>
        public int Height
        {
            get
            {
                return height;
            }
        }

        /// <summary>
        /// Gets the duration of the video file in seconds.
        /// </summary>
        public double MediaLength
        {
            get
            {
                return mediaLength;
            }
        }

        /// <summary>
        /// Gets the bit depth of the captured frame (always 24 bpp RGB).
        /// </summary>
        public PixelFormat PixelFormat
        {
            get
            {
                return PixelFormat.Format24bppRgb;
            }
        }

        /// <summary>
        /// Gets the image at the specified time.  Equivalent to calling "GetImageAtTime()".
        /// </summary>
        public Bitmap this[double seconds]
        {
            get
            {
                return GetImageAtTime(seconds);
            }
        }

        /// <summary>
        /// Gets the specified frame.  Equivalent to calling "GetImage()".
        /// </summary>
        public Bitmap this[int frame]
        {
            get
            {
                return GetImage(frame);
            }
        }

        /// <summary>
        /// Converts the frame number to seconds.
        /// </summary>
        public double ConvertFrameNumberToSeconds(int frameNumber)
        {
            return (frameNumber / frameRate);
        }

        /// <summary>
        /// Converts the seconds to frame number.
        /// </summary>
        public int ConvertSecondsToFrameNumber(double seconds)
        {
            return (int)Math.Floor(seconds * frameRate);
        }

        /// <summary>
        /// Gets the specified frame.  Frames are indexed starting at 0 and go through (FrameCount - 1).
        /// </summary>
        public Frame GetFrame(int frameNumber)
        {
            return new Frame(GetImage(frameNumber), frameNumber, ConvertFrameNumberToSeconds(frameNumber));
        }

        /// <summary>
        /// Gets the image of the specified frame.  Frames are indexed starting at 0 and go through (FrameCount - 1).
        /// </summary>
        public Bitmap GetImage(int frameNumber)
        {
            if (frameNumber < frameCount)
            {
                return GetImageAtTime(ConvertFrameNumberToSeconds(frameNumber));
            }
            else
            {
                throw new ArgumentException(string.Format("frameNumber must be between 0 and {0} inclusive, value was \"{1}\"", frameCount - 1, frameNumber));
            }
        }

        /// <summary>
        /// Gets the frame at the specified time.  Seconds must be less than or equal to MediaLength.
        /// </summary>
        public Frame GetFrameAtTime(double seconds)
        {
            return new Frame(GetImageAtTime(seconds), ConvertSecondsToFrameNumber(seconds), seconds);
        }


        /// <summary>
        /// Gets the image at the specified time.  Seconds must be less than or equal to MediaLength.
        ///	All other image and frame accessors call this method.
        /// </summary>
        public Bitmap GetImageAtTime(double seconds)
        {
            if (seconds <= mediaLength)
            {
                if (mediaDet != null)
                {
                    IntPtr bufferPtr = IntPtr.Zero;
                    Bitmap returnValue = null;

                    try
                    {
                        // create a buffer to hold the image data from the MediaDet
                        int bufferSize;
                        mediaDet.GetBitmapBits(seconds, out bufferSize, IntPtr.Zero, width, height);
                        bufferPtr = Marshal.AllocHGlobal(bufferSize);
                        mediaDet.GetBitmapBits(seconds, out bufferSize, bufferPtr, width, height);

                        // compose a bitmap from the data in the managed buffer	
                        unsafe
                        {
                            returnValue = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                            BitmapData imageData = returnValue.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                            int* imagePtr = (int*)imageData.Scan0;

                            int bitmapHeaderSize = Marshal.SizeOf(videoInfo.BmiHeader);
                            int* sourcePtr = (int*)((byte*)bufferPtr.ToPointer() + bitmapHeaderSize);

                            for (int i = 0; i < (bufferSize - bitmapHeaderSize) / 4; i++)
                            {
                                *imagePtr = *sourcePtr;
                                imagePtr++;
                                sourcePtr++;
                            }

                            returnValue.UnlockBits(imageData);
                            returnValue.RotateFlip(RotateFlipType.Rotate180FlipX); // DirectShow stores pixels in a different order than Bitmaps do
                        }

                        Marshal.FreeHGlobal(bufferPtr);

                        return returnValue;
                    }
                    catch
                    {
                        if (bufferPtr != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(bufferPtr);
                        }

                        if (returnValue != null)
                        {
                            returnValue.Dispose();
                        }

                        throw;
                    }
                }
                else
                {
                    throw new InvalidOperationException("cannot retrieve the frame because the FileName property has not been set yet");
                }
            }
            else
            {
                throw new ArgumentException(string.Format("seconds must be between 0 and {0} inclusive, value was \"{1}\"", mediaLength, seconds));
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<Frame>).GetEnumerator();
        }

        #region IEnumerable<Frame> Members
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<Frame> GetEnumerator()
        {
            int currentFrame = 0;

            while (currentFrame < frameCount)
            {
                yield return GetFrame(currentFrame++);
            }
        }
        #endregion

        /// <summary>
        /// Stores a frame's image, time (in seconds), and frame number.  Can be explicitly converted to a Bitmap.
        /// </summary>
        public class Frame : IDisposable
        {
            private int frameIndex;
            private Bitmap image;
            private double seconds;
            private string timeString;

            /// <summary>
            /// Initializes a new instance of the <see cref="Frame"/> class. 
            /// </summary>
            public Frame(Bitmap image, int frameIndex, double seconds)
            {
                this.image = image;
                this.frameIndex = frameIndex;
                this.seconds = seconds;

                timeString = TimeSpan.FromSeconds(seconds).ToString();
            }

            /// <summary>
            /// Performs an explicit conversion from <see cref="User.DirectShow.FrameGrabber.Frame"/> to <see cref="System.Drawing.Bitmap"/>.
            /// </summary>
            public static explicit operator Bitmap(Frame f)
            {
                return f.Image;
            }

            /// <summary>
            /// Gets the frame number.  Will be between 0 and (FrameCount - 1), inclusive.
            /// </summary>
            public int FrameIndex
            {
                get
                {
                    return frameIndex;
                }
            }

            /// <summary>
            /// Gets the image of the frame.
            /// </summary>
            public Bitmap Image
            {
                get
                {
                    return image;
                }
            }

            /// <summary>
            /// Gets the time position of the frame in seconds.
            /// </summary>
            public double Seconds
            {
                get
                {
                    return seconds;
                }
            }

            /// <summary>
            /// Gets the time position of the frame as a nicely-formatted string.
            /// </summary>
            public string TimeString
            {
                get
                {
                    return timeString;
                }
            }

            #region IDisposable Members

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                image.Dispose();
            }

            #endregion
        }

    }


}
