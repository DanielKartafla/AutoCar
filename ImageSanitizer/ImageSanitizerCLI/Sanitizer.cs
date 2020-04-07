using System;
using System.Drawing;
using System.IO;

namespace ImageSanitizerCLI
{
  public class Sanitizer
  {
    static int Main(string[] args)
    {
      if(args.Length == 0)
      {
        Console.WriteLine("Please enter the path to the directory where the images are stored.");
        Console.WriteLine("Usage: Sanitizer <path to images directory>");
        return 1;
      }

      Sanitizer sanitizer = new Sanitizer(args[0]);
      sanitizer.Sanitize();

      Console.ReadKey();
      return 0;
    }

    public string ImagesPath { get; set; }

    public Sanitizer(string imagesPath)
    {
      ImagesPath = Path.GetFullPath(imagesPath);
    }

    public void Sanitize()
    {
      Console.WriteLine("Beginning sanitization in {0}", ImagesPath);

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
          Console.WriteLine("Deleting {0}", file);
          File.Delete(file);
          deleted++;
        }
        else
        {
          notDeleted++;
        }
      }

      Console.WriteLine("Deleted images: {0}", deleted);
      Console.WriteLine("Remaining images: {0}", notDeleted);
      Console.WriteLine("Sanitization process has ended");
    }
  }
}
