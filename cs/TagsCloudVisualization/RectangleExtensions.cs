using System.Drawing;

namespace TagsCloudVisualization;

public static class RectangleExtensions
{
    public static PointF GetCenter(this Rectangle rectangle)
    {
        var rectangleCenterX = rectangle.Location.X + rectangle.Size.Width / 2f;
        var rectangleCenterY = rectangle.Location.Y + rectangle.Height / 2f;
        
        return new PointF(rectangleCenterX, rectangleCenterY);
    }
}