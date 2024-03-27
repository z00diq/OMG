namespace App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public interface ICalculatorExpression
    {
        /// <summary>
        /// выполняет выражение, если в нем есть переменные пробует их подставить
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        int Execute(string expression);
        
        /// <summary>
        /// устанавливает переменную и выражение для нее внутри калькулятора, при обращении по этому ключу
        /// будет вычисляться это выражение
        /// </summary>
        /// <param name="expressionKey"></param>
        /// <param name="expression"></param>
        void SetExpression(string expressionKey, string expression);
        
        /// <summary>
        /// запрашиваем выражение по ключу и выполняем его
        /// </summary>
        /// <param name="expressionKey"></param>
        /// <returns></returns>
        int Get(string expressionKey);
    }
}