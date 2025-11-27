using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly List<Rectangle> rectangles = [];
    private readonly Point center;
    private ISpiral spiral;
 
    public IReadOnlyList<Rectangle> Rectangles => rectangles;
    
    public CircularCloudLayouter(Point center)
    {
        spiral = new ArchimedeanSpiral(center);
        this.center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
        {
            throw new ArgumentException("Received non-positive height or length, but expected positive");
        }
        
        var newRectangle = CreateRectangleAtSuitablePointOnSpiral(rectangleSize);
        var intersectingRectangle = rectangles.FirstOrDefault(x => x.IntersectsWith(newRectangle));
        
        if (rectangles.Count > 0)
        {
            while (intersectingRectangle.IsEmpty)
            {
                newRectangle = ShiftRectangleToCenter(newRectangle);
                intersectingRectangle = rectangles.FirstOrDefault(x => x.IntersectsWith(newRectangle));
            }
        }
        
        while (!intersectingRectangle.IsEmpty)
        {
            newRectangle = ShiftRectangleFromCenter(newRectangle);
            intersectingRectangle = rectangles.FirstOrDefault(x => x.IntersectsWith(newRectangle));
        }

        rectangles.Add(newRectangle);
        
        return newRectangle;
    }
    
    public void SetNewSpiral(ISpiral newSpiral)
    {
        spiral = newSpiral;
    }

    private Rectangle CreateRectangleAtSuitablePointOnSpiral(Size rectangleSize)
    {
        var point = spiral.GetNextPoint();
        
        while (rectangles.Any(x => x.Contains(point)))
        {
            point = spiral.GetNextPoint();
        }
        
        var newRectangle = CreateRectangle(point, rectangleSize);
        var intersectingRectangle = rectangles.FirstOrDefault(x => x.IntersectsWith(newRectangle));

        while (!intersectingRectangle.IsEmpty)
        {
            newRectangle = CreateRectangle(spiral.GetNextPoint(), rectangleSize);
            intersectingRectangle = rectangles.FirstOrDefault(x => x.IntersectsWith(newRectangle));
        }

        return newRectangle;
    }

    private Rectangle ShiftRectangleFromCenter(Rectangle rectangle)
    {
        var rectangleCenter = rectangle.GetCenter();
        var offsetX = Math.Sign(rectangleCenter.X - center.X);
        var offsetY = Math.Sign(rectangleCenter.Y - center.Y);
        
        rectangle.Offset(offsetX, offsetY);
        
        return rectangle;
    }

    private Rectangle ShiftRectangleToCenter(Rectangle rectangle)
    {
        var rectangleCenter = rectangle.GetCenter();
        var offsetX = Math.Sign(center.X - rectangleCenter.X);
        var offsetY = Math.Sign(center.Y - rectangleCenter.Y);
        
        rectangle.Offset(offsetX, offsetY);
        
        return rectangle;
    }

    private static Rectangle CreateRectangle(Point pointCenter, Size size)
    {
        return new Rectangle(pointCenter.X - size.Width / 2, pointCenter.Y - size.Height / 2, size.Width, size.Height);
    }
}