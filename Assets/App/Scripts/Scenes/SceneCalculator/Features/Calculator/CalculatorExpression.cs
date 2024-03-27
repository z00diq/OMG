namespace App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public class CalculatorExpression : ICalculatorExpression
    {

        public int Execute(string expression)
        {
            return 8;
        }

        public void SetExpression(string expressionKey, string expression)
        {
        }

        public int Get(string expressionKey)
        {
            return 6;
        }
    }
}