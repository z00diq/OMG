using App.Scripts.Scenes.SceneCalculator.Features.Calculator;
using NUnit.Framework;
using Tests.App.Scripts.Tests.SceneCalculator;
using UnityEngine;
using File = System.IO.File;


public class TestCalculator
{
    private const string PathTestCase = "Assets/App/Scripts/Tests/SceneCalculator/TestCases/{0}.json";
    
    [Test]
    [TestCase("3 +5", 8)]
    [TestCase("-3 +5", 2)]
    [TestCase("2 - (3 +5) ", -6)]
    [TestCase("8 *2 - (3 +5)  ", 8)]
    public void TestCalculatorOneLineExpression(string expression, int expected)
    {
        var calculator = new CalculatorExpression();

        int result = calculator.Execute(expression);

        Assert.AreEqual(expected, result);
    }

    [Test]
    [TestCase("scenario_0")]
    public void TestParamsCalculator(string testFileKey)
    {
        var testPath = string.Format(PathTestCase, testFileKey);

        var testText = File.ReadAllText(testPath);
        var testData = JsonUtility.FromJson<TestExpression>(testText);
        
        var calculator = new CalculatorExpression();

        foreach (var expression in testData.expressions)
        {
            calculator.SetExpression(expression.key, expression.value);
        }

        foreach (var expressionCase in testData.expected)
        {
            int result = calculator.Get(expressionCase.key);
            Assert.AreEqual(expressionCase.result, result);
        }
    }
    
}
