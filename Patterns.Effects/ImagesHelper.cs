using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Patterns.Effects
{
    public class ImagesHelper
    {
        public static Bitmap ResizeImage(Bitmap imgToResize, float scale)
        {
            var b = new Bitmap((int)(imgToResize.Width * scale), (int)(imgToResize.Height * scale));
            using (var g = Graphics.FromImage(b))
            {
                using (var wrapMode = new ImageAttributes())
                {
                    g.PixelOffsetMode = PixelOffsetMode.Half;
                    g.InterpolationMode = scale > 1 ? InterpolationMode.NearestNeighbor : InterpolationMode.Bicubic;
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    g.DrawImage(imgToResize, new Rectangle(0, 0, (int)(imgToResize.Width * scale), (int)(imgToResize.Height * scale)), 0, 0, imgToResize.Width, imgToResize.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return b;
        }

        public static Bitmap RepeatToFitSize(Bitmap bitmap, int width, int height)
        {
            var result = new Bitmap(width, height);
            using (var g = Graphics.FromImage(result))
            {
                for (var x = 0; x < width; x += bitmap.Width)
                {
                    for (var y = 0; y < height; y += bitmap.Height)
                    {
                        g.DrawImage(bitmap, x, y);
                    }
                }
            }
            return result;
        }

        public static Bitmap MergeBitmaps(params Bitmap[] bitmaps)
        {
            var mergedBitmap = new Bitmap(bitmaps[0].Width, bitmaps[0].Height);
            using (var graphics = Graphics.FromImage(mergedBitmap))
            {
                graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, mergedBitmap.Width, mergedBitmap.Height);
                for (var i = bitmaps.Length - 1; i >= 0; i--)
                {
                    graphics.DrawImage(bitmaps[i], 0, 0);
                }
            }
            return mergedBitmap;
        }

        public static Bitmap Adjust(Bitmap originalImage, float brightness, float contrast, float gamma)
        {
            Bitmap adjustedImage = new Bitmap(originalImage.Width, originalImage.Height);
            float adjustedBrightness = brightness - 1.0f;
            float[][] ptsArray =
                {
                    new[] {contrast, 0, 0, 0, 0},
                    new[] {0, contrast, 0, 0, 0},
                    new[] {0, 0, contrast, 0, 0},
                    new[] {0, 0, 0, 1.0f, 0},
                    new[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}
                };

            var imageAttributes = new ImageAttributes();
            imageAttributes.ClearColorMatrix();
            imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);
            Graphics g = Graphics.FromImage(adjustedImage);
            g.DrawImage(originalImage, new Rectangle(0, 0, adjustedImage.Width, adjustedImage.Height)
                        , 0, 0, originalImage.Width, originalImage.Height,
                        GraphicsUnit.Pixel, imageAttributes);
            return adjustedImage;
        }

        public static Bitmap ApplyAlphaMask(Bitmap input, Bitmap mask)
        {
            var output = new Bitmap(input.Width, input.Height, PixelFormat.Format32bppArgb);
            var rect = new Rectangle(0, 0, input.Width, input.Height);
            var bitsMask = mask.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var bitsInput = input.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var bitsOutput = output.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                for (int y = 0; y < input.Height; y++)
                {
                    byte* ptrMask = (byte*) bitsMask.Scan0 + y*bitsMask.Stride;
                    byte* ptrInput = (byte*) bitsInput.Scan0 + y*bitsInput.Stride;
                    byte* ptrOutput = (byte*) bitsOutput.Scan0 + y*bitsOutput.Stride;
                    for (int x = 0; x < input.Width; x++)
                    {
                        ptrOutput[4*x] = ptrInput[4*x];
                        ptrOutput[4*x + 1] = ptrInput[4*x + 1];
                        ptrOutput[4*x + 2] = ptrInput[4*x + 2];
                        ptrOutput[4*x + 3] = (byte)((ptrMask[4*x] + ptrMask[4*x + 1] + ptrMask[4*x + 2]) / 3);
                    }
                }
            }
            mask.UnlockBits(bitsMask);
            input.UnlockBits(bitsInput);
            output.UnlockBits(bitsOutput);
            return output;
        }

        public static Bitmap Blur(Bitmap image, Rectangle rectangle, Int32 blurSize)
        {
            Bitmap blurred = new Bitmap(image.Width, image.Height);

            // make an exact copy of the bitmap provided
            using (Graphics graphics = Graphics.FromImage(blurred))
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

            // look at every pixel in the blur rectangle
            for (Int32 xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
            {
                for (Int32 yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
                {
                    Int32 avgR = 0, avgG = 0, avgB = 0;
                    Int32 blurPixelCount = 0;

                    // average the color of the red, green and blue for each pixel in the
                    // blur size while making sure you don't go outside the image bounds
                    for (Int32 x = xx; (x < xx + blurSize && x < image.Width); x++)
                    {
                        for (Int32 y = yy; (y < yy + blurSize && y < image.Height); y++)
                        {
                            Color pixel = blurred.GetPixel(x, y);

                            avgR += pixel.R;
                            avgG += pixel.G;
                            avgB += pixel.B;

                            blurPixelCount++;
                        }
                    }

                    avgR = avgR / blurPixelCount;
                    avgG = avgG / blurPixelCount;
                    avgB = avgB / blurPixelCount;

                    // now that we know the average for the blur size, set each pixel to that color
                    for (Int32 x = xx; x < xx + blurSize && x < image.Width && x < rectangle.Width; x++)
                        for (Int32 y = yy; y < yy + blurSize && y < image.Height && y < rectangle.Height; y++)
                            blurred.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
                }
            }

            return blurred;
        }
    }
}
