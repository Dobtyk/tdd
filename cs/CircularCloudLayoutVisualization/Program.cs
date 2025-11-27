using System.Drawing;
using System.Runtime.Versioning;
using TagsCloudVisualization;

namespace CircularCloudLayoutVisualization;

class Program
{
    [SupportedOSPlatform("Windows")]
    private static void Main()
    {
        var imageSize = 1000;
        var spiralStep = Math.PI / 180;
        var spiralParameterA = 0.1f;
        GenerateImageCloudLayouter(imageSize, "Cloud-1.png", spiralStep, spiralParameterA);
        GenerateImageCloudLayouter(imageSize, "Cloud-2.png", spiralStep, spiralParameterA);
        GenerateImageCloudLayouter(imageSize, "Cloud-3.png", spiralStep, spiralParameterA);
    }

    [SupportedOSPlatform("Windows")]
    private static void GenerateImageCloudLayouter(int imageSize, string fileName, double step, double parameterA)
    {
        var spiral = new ArchimedeanSpiral(new Point(imageSize / 2, imageSize / 2), step, parameterA);
        var circularCloudLayouter = GenerateCloudLayouterWithRectangles(imageSize, spiral);
        using var bitmap = new Bitmap(imageSize, imageSize);
        using var graphics = Graphics.FromImage(bitmap);
        using var pen = new Pen(Color.Black, 1);
        
        graphics.Clear(Color.White);
        
        foreach (var rectangle in circularCloudLayouter.Rectangles)
        {
            graphics.DrawRectangle(pen, rectangle);
        }
        
        var projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var imagesDirectory = Path.Combine(projectDirectory, "Images");
        Directory.CreateDirectory(imagesDirectory);

        var filePath = Path.Combine(imagesDirectory, fileName);
        bitmap.Save(filePath);
    }
    
    private static CircularCloudLayouter GenerateCloudLayouterWithRectangles(int imageSize, ISpiral spiral)
    {
        var circularCloudLayouter = new CircularCloudLayouter(new Point(imageSize /2, imageSize / 2));
        var rectangleSize = imageSize / 10;
        var random = new Random();
        
        circularCloudLayouter.SetNewSpiral(spiral);

        for (var i = 0; i < 100; i++)
        {
            var width = random.Next(2, rectangleSize);
            var height = random.Next(2, width);
            circularCloudLayouter.PutNextRectangle(new Size(width, height));
        }
        return circularCloudLayouter;
    }
}