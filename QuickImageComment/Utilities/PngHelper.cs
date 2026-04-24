// code suggested from Microsoft Copilot
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public static class PngHelper
{
    public static void SavePngWithoutMetadata(Bitmap bmp, string path)
    {
        // Get PNG encoder
        ImageCodecInfo pngEncoder = GetPngEncoder();
        if (pngEncoder == null)
            throw new System.Exception("PNG encoder not found");

        // Encoder parameter: CompressionLevel = 0..9 (0 = none, 9 = max)
        EncoderParameters encParams = new EncoderParameters(1);
        encParams.Param[0] = new EncoderParameter(Encoder.Compression, 0L);

        // Save to memory first
        using (MemoryStream ms = new MemoryStream())
        {
            bmp.Save(ms, pngEncoder, encParams);

            // Re-load the PNG but drop all metadata
            using (Bitmap clean = new Bitmap(ms))
            {
                // Create a new bitmap with same pixel data but no metadata
                using (Bitmap clone = new Bitmap(clean.Width, clean.Height, clean.PixelFormat))
                using (Graphics g = Graphics.FromImage(clone))
                {
                    g.DrawImage(clean, 0, 0, clean.Width, clean.Height);

                    // Save final PNG without metadata
                    clone.Save(path, pngEncoder, encParams);
                }
            }
        }
    }

    private static ImageCodecInfo GetPngEncoder()
    {
        foreach (var c in ImageCodecInfo.GetImageEncoders())
        {
            if (c.FormatID == ImageFormat.Png.Guid)
                return c;
        }
        return null;
    }
}
