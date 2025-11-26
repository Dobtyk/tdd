using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization;

[TestFixture]
public class CircularCloudLayouterTests
{
    private CircularCloudLayouter cloudLayouter;
    
    [SetUp]
    public void SetUp()
    {
        cloudLayouter = new CircularCloudLayouter(new Point(100, 100));
    }
    
    [TestCaseSource(nameof(CasesWhenInvalidSize))]
    public void PutNextRectangle_ThrowsArgumentException_WhenInvalidSize(Size input)
    {
        var action = () => cloudLayouter.PutNextRectangle(input);
        
        action.Should().Throw<ArgumentException>();
    }
    
    [TestCaseSource(nameof(CasesWhenRandomSize))]
    public void PutNextRectangle_ReturnsCorrectSizeRectangle_WhenRandomSize(Size input)
    {
        var result = cloudLayouter.PutNextRectangle(input);
        
        result.Height.Should().Be(input.Height);
        result.Width.Should().Be(input.Width);
    }
    
    [TestCaseSource(nameof(CasesWhenSeveralRandomSizes))]
    public void PutNextRectangle_NonIntersectingRectangles_WhenSeveralSizes(IEnumerable<Size> input)
    {
        foreach (var size in input)
        {
            cloudLayouter.PutNextRectangle(size);
        }

        for (var i = 0; i < cloudLayouter.Rectangles.Count; i++)
        {
            for (var j = i + 1; j < cloudLayouter.Rectangles.Count; j++)
            {
                cloudLayouter.Rectangles[i].IntersectsWith(cloudLayouter.Rectangles[j]).Should().Be(false);
            }
        }
    }
    
    public static IEnumerable<TestCaseData> CasesWhenInvalidSize()
    {
        yield return new TestCaseData(new Size(1, 0));
        yield return new TestCaseData(new Size(0, 1));
        yield return new TestCaseData(new Size(-4, 1));
        yield return new TestCaseData(new Size(1, -4));
        yield return new TestCaseData(new Size(0, 0));
        yield return new TestCaseData(new Size(-4, -4));
    }
    
    public static IEnumerable<TestCaseData> CasesWhenSeveralRandomSizes()
    {
        var random = new Random();
        var testData = new List<Size>();
        for (var i = 0; i < 10; i++)
        {
            var width = random.Next(1, 101);
            var height = random.Next(1, 101);
            testData.Add(new Size(width, height));
        }
        yield return new TestCaseData(testData);
    }
    
    public static IEnumerable<TestCaseData> CasesWhenRandomSize()
    {
        var random = new Random();
        for (var i = 0; i < 10; i++)
        {
            var width = random.Next(1, 101);
            var height = random.Next(1, 101);
            yield return new TestCaseData(new Size(width, height));
        }

    }
}