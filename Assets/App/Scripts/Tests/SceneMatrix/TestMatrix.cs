
using System.IO;
using App.Scripts.Modules.Grid;
using App.Scripts.Scenes.SceneMatrix.Features.FigureProvider;
using App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser;
using App.Scripts.Scenes.SceneMatrix.Features.FigureRotator.Services;
using NUnit.Framework;

public class TestMatrix
{
    private const string PathTests = "Assets/App/Scripts/Tests/SceneMatrix/TestCases";
    private const string TestDataFile = PathTests + "/{0}.txt";
    
    [Test]
    [TestCase("g_block", "g_block_-1_expected", -1)]
    [TestCase("g_block", "g_block_1_expected", 1)]
    [TestCase("g_block", "g_block_2_expected", 2)]
    [TestCase("g_block", "g_block_3_expected", 3)]
    [TestCase("tri_block", "tri_block_-2_expected", -2)]
    [TestCase("tri_block", "tri_block_1_expected", 1)]
    [TestCase("tri_block", "tri_block", 4)]
    [TestCase("tri_block", "tri_block_1_expected", 5)]
    public void TestFigures(string fileKey, string expectedFileKey, int rotationCount)
    {
        ProcessFileTest(fileKey, expectedFileKey, rotationCount);
    }

    [Test]
    [TestCase("tri_block", "tri_block_1_expected", 1)]
    public void TestConcrete(string fileKey, string expectedFileKey, int rotationCount)
    {
        ProcessFileTest(fileKey, expectedFileKey, rotationCount);
    }

    private void ProcessFileTest(string fileKey, string expectedFileKey, int rotationCount)
    {
        string pathTest = string.Format(TestDataFile, fileKey);
        string pathTestExpected = string.Format(TestDataFile, expectedFileKey);

        Grid<bool> inputMatrix = LoadMatrixFromFile(pathTest);
        Grid<bool> expectedMatrix = LoadMatrixFromFile(pathTestExpected);

        var figureRotator = new FigureRotatorDummy();

        var resultMatrix = figureRotator.RotateFigure(inputMatrix, rotationCount);

        Assert.AreEqual(expectedMatrix, resultMatrix);
    }

    private Grid<bool> LoadMatrixFromFile(string file)
    {
        var parser = new ParserFigureDummy();
        var figureTxt = File.ReadAllText(file);

        return parser.ParseFile(figureTxt);
    }
}
