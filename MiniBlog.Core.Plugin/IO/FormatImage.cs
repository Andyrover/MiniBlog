﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace MiniBlog.Core.Plugin.IO
{
    public static class FormatImage
    {
        /// <summary>
        /// 限制宽按比例缩小
        /// </summary>
        /// <param name="inPath">输入路径</param>
        /// <param name="outPath">如果和原图地址相同则覆盖原图</param>
        /// <param name="toWidth">生成图片宽</param>
        /// <param name="quality">压缩质量(1~100)数字越小压缩比率越大(</param>
        public static void AutoToSmall(string inPath, string outPath, int toWidth, int quality=100)
        {
            //原始图片
            var initImage = Image.FromFile(inPath);            
            var rawFormat = initImage.RawFormat;
            var initWidth = initImage.Width;
            var initHeight = initImage.Height;

            //图片太小不用处理了,直接保存
            if (toWidth >= initWidth)
            {
                initImage.Save(outPath);
                initImage.Dispose();
                return;
            }
            var toHeight = toWidth * initHeight / initWidth;
            //缩略图对象
            Image pickedImage = new Bitmap(toWidth, toHeight);
            Graphics graphics = Graphics.FromImage(pickedImage);
            //用指定背景色清空画布
            graphics.Clear(Color.WhiteSmoke);
            //设置质量
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            //绘制缩略图
            graphics.DrawImage(initImage, new Rectangle(-1, -1, toWidth + 1, toHeight + 1), new Rectangle(0, 0, initWidth, initHeight), GraphicsUnit.Pixel);
            graphics.Dispose();
            initImage.Dispose();
            //压缩图片
            Compress(pickedImage, rawFormat, outPath, quality);
        }

        /// <summary>
        /// 图片压缩
        /// </summary>
        /// <param name="inImage">要压缩的图片</param>
        /// <param name="outPath">输入路径</param>
        /// <param name="quality">压缩质量(1~100)数字越小压缩比率越大(</param>
        /// <returns></returns>
        public static bool Compress(Image inImage, ImageFormat rawFormat, string outPath,int quality=100)
        {
            EncoderParameters ep = new EncoderParameters();
            long[] qualitys = new long[1];
            qualitys[0] = quality;//设置压缩的比例1-100    
            EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, qualitys);
            ep.Param[0] = encoderParameter;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    inImage.Save(outPath, jpegICIinfo, ep);
                }
                else
                {
                    inImage.Save(outPath, rawFormat);
                }
                return true;
            }
            catch {
                return false;
            }
            finally
            {
                inImage.Dispose();
            }
        }
    }
}