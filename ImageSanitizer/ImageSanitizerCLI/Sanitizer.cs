using System;
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



      Console.WriteLine("Sanitization process has ended");
    }
  }
}
