using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageSanitizerCLI
{
  public class Sanitizer
  {
    const int RESOLUTION = 256;

    static int Main(string[] args)
    {
      if(args.Length == 0)
      {
        Console.WriteLine("Please enter the path to the directory where the images are stored.");
        Console.WriteLine("Usage: Sanitizer <path to images directory>");
        return 1;
      }

      Sanitizer sanitizer = new Sanitizer(args[0]);
      Console.WriteLine("Beginning sanitization process");
      sanitizer.DeleteNonWhiteCornerImages();
      sanitizer.ScaleImages();
      Console.WriteLine("Sanitization process has ended");

      Console.ReadKey();
      return 0;
    }

    public string ImagesPath { get; set; }

    public Sanitizer(string imagesPath)
    {
      ImagesPath = Path.GetFullPath(imagesPath);
    }

    public void DeleteNonWhiteCornerImages()
    {
      Console.WriteLine("Beginning deleting of non-white corner images in {0}", ImagesPath);

      Color trueWhite = System.Drawing.Color.FromArgb(255, 255, 255, 255);
      System.Drawing.GraphicsUnit pixel = GraphicsUnit.Pixel;
      int width = 0;
      int height = 0;
      Bitmap bmp;

      int deleted = 0;
      int notDeleted = 0;
      foreach (string file in Directory.GetFiles(ImagesPath, "*.jpg"))
      {
        bmp = new Bitmap(file);

        width = Convert.ToInt32(bmp.GetBounds(ref pixel).Width) - 1;
        height = Convert.ToInt32(bmp.GetBounds(ref pixel).Height) - 1;

        //check if all the corner pixels are white
        if (!bmp.GetPixel(0,0).Equals(trueWhite) 
          || !bmp.GetPixel(0, height).Equals(trueWhite)
          || !bmp.GetPixel(width, 0).Equals(trueWhite)
          || !bmp.GetPixel(width, height).Equals(trueWhite))
        {
          bmp.Dispose();
          File.Delete(file);
          deleted++;
          if(deleted % 100 == 0)
          {
            Console.WriteLine("...deleted {0} images so far", deleted);
          }
        }
        else
        {
          notDeleted++;
        }
      }

      Console.WriteLine("Deleted images: {0}", deleted);
      Console.WriteLine("Remaining images: {0}", notDeleted);
    }

    public void ScaleImages()
    {
      Console.WriteLine("Beginning scaling in {0}", ImagesPath);

      System.Drawing.GraphicsUnit pixel = GraphicsUnit.Pixel;
      int width = 0;
      int height = 0;
      int scaled = 0;
      Bitmap bmp;
      
      foreach (string file in Directory.GetFiles(ImagesPath, "*.jpg"))
      {
        bmp = new Bitmap(file);

        width = Convert.ToInt32(bmp.GetBounds(ref pixel).Width);
        height = Convert.ToInt32(bmp.GetBounds(ref pixel).Height);
        
        bmp.Dispose();
        
        if(width != RESOLUTION || height != RESOLUTION)
        {
          //now apply resolution 
          //parts from https://stackoverflow.com/a/6501997
          using (Image image = Image.FromFile(file))
          using (Image newImage = ScaleImage(image, RESOLUTION, RESOLUTION))
          {
            image.Dispose();
            newImage.Save(file, ImageFormat.Jpeg);
          }
          scaled++;
          if(scaled % 100 == 0)
          {
            Console.WriteLine("...scaled {0} images so far", scaled);
          }
        }

      }

      Console.WriteLine("Scaled {0} images", scaled);
      Console.WriteLine("Scaling process has ended");
  }

    public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
    {
      //parts from https://stackoverflow.com/a/6501997
      var ratioX = (double)maxWidth / image.Width;
      var ratioY = (double)maxHeight / image.Height;
      var ratio = Math.Min(ratioX, ratioY);

      var newWidth = (int)(image.Width * ratio);
      var newHeight = (int)(image.Height * ratio);

      var newImage = new Bitmap(RESOLUTION, RESOLUTION);

      using (var graphics = Graphics.FromImage(newImage))
      {
        graphics.Clear(Color.White);
        graphics.DrawImage(
          image, 
          Math.Max(0, (RESOLUTION - image.Width) / 2),
          Math.Max(0, (RESOLUTION - image.Height) / 2), 
          newWidth, 
          newHeight);
      }

      return newImage;
    }
  }
}
