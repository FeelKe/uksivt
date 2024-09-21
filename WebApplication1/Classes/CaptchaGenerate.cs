using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace WebApplication1.Classes
{
    public class CaptchaGenerate
    {
        public string Text { get; private set; }

        public CaptchaGenerate()
        {
            Text = GenerateCaptchaText(5);
        }

        public Image<Rgba32> GetImage()
        {
            int imageWidth = 200;
            int imageHeight = 60;
            var image = new Image<Rgba32>(imageWidth, imageHeight); 
            image.Mutate(ctx =>
            {
                ctx.Fill(Color.White); 
                var font = SystemFonts.CreateFont("Arial", 32);
                var textOptions = new RichTextOptions(font)
                {
                    HorizontalAlignment = HorizontalAlignment.Center, 
                    VerticalAlignment = VerticalAlignment.Center, 
                    Origin = new PointF(imageWidth / 2, imageHeight / 2) 
                };
                ctx.DrawText(textOptions, Text, Color.ParseHex("#323133"));
            });

            return image;
        }

        public byte[] ToByteArray()
        {
            using (var ms = new MemoryStream())
            {
                GetImage().SaveAsPng(ms);
                return ms.ToArray();
            }
        }

        private string GenerateCaptchaText(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }
            return new string(result);
        }
    }
}
