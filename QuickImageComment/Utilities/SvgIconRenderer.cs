using SkiaSharp;
using Svg.Skia;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace QuickImageComment.Utilities
{
    public static class SvgIconRenderer
    {
        public static Bitmap RenderSvgFromResource(byte[] svgBytes, Size targetSize)
        {
            using (var stream = new MemoryStream(svgBytes))
            {
                var svg = new SKSvg();
                svg.Load(stream);
                return renderSvgToBitmap(svg, targetSize);
            }
        }

        public static Bitmap RenderSvgFromFile(string svgPath, Size targetSize)
        {
            var svg = new SKSvg();
            svg.Load(svgPath);
            return renderSvgToBitmap(svg, targetSize);
        }

        private static Bitmap renderSvgToBitmap(SKSvg svg, Size targetSize)
        {
            // Render at 2× resolution for sharpness
            int renderW = targetSize.Width * 2;
            int renderH = targetSize.Height * 2;

            var skBitmap = new SKBitmap(renderW, renderH);
            using (var canvas = new SKCanvas(skBitmap))
            {
                canvas.Clear(SKColors.Transparent);

                var bounds = svg.Picture.CullRect;

                float scaleX = renderW / bounds.Width;
                float scaleY = renderH / bounds.Height;
                float scale = Math.Min(scaleX, scaleY);

                float dx = (renderW - bounds.Width * scale) / 2f;
                float dy = (renderH - bounds.Height * scale) / 2f;

                canvas.Translate(dx, dy);
                canvas.Scale(scale);
                canvas.DrawPicture(svg.Picture);
                canvas.Flush();
            }

            // Convert Skia bitmap → System.Drawing.Bitmap
            var bmp = new Bitmap(renderW, renderH, PixelFormat.Format32bppArgb);

            var bmpData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            IntPtr srcPtr = skBitmap.GetPixels();
            int byteCount = skBitmap.RowBytes * skBitmap.Height;

            byte[] buffer = new byte[byteCount];
            Marshal.Copy(srcPtr, buffer, 0, byteCount);
            Marshal.Copy(buffer, 0, bmpData.Scan0, byteCount);

            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}