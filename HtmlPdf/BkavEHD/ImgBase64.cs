using System;
using System.IO;

namespace BES
{
    public static class ImgBase64
    {
        public static string ImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }
    }

}
