using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public class CalculatorExpression : ICalculatorExpression
    {
        private const string s_correctKeyPatteren = @"^(?![а-яА-Я\d]+$)(?!.\d)(?=.[a-z])[a-zA-Z]+$";
        private const string s_correctExpressionPattern = @"^[0-9+\-*/()A-Za-z\s]+$";
        private const string s_operations = "+ / - *";

        private Dictionary<string, string> _expressions = new Dictionary<string, string>();
        private Regex _regex;

        public int Execute(string expression)
        {
            Stack<char> symbols = new Stack<char>();
            string priorityExpression = string.Empty;
            Expression calculatable = new Expression();

            foreach (char s in expression)
            {
                if(s!=' ')
                    symbols.Push(s);
            }

            char nextSymbol = '\0';
            char previousSymbol = '\0';

            while (symbols.Count > 0)
            {
                nextSymbol = symbols.Pop();

                if (int.TryParse(nextSymbol.ToString(), out int value))
                    calculatable.AddValue(value);

                if (s_operations.Contains(previousSymbol))
                    calculatable.AddValue(calculatable.Calculate());

                previousSymbol = nextSymbol;
            }


            return calculatable.Calculate();
        }

        public void SetExpression(string expressionKey, string expression)
        {
            _regex = new Regex(s_correctKeyPatteren);

            if (_regex.IsMatch(expressionKey) == false)
                throw new ExceptionExecuteExpression("Неверный формат ключа");

            _regex = new Regex(s_correctExpressionPattern);

            if (_regex.IsMatch(expression) == false)
                throw new ExceptionExecuteExpression("Неверный формат выражения");

            if (expression.Contains(expressionKey))
                throw new ExceptionExecuteExpression("Выражение содержит добавляемый ключ");

            if(_expressions.ContainsKey(expressionKey))
                   throw new ExceptionExecuteExpression("Выражение c таким ключем уже существует");

            _expressions[expressionKey] = expression;
        }

        public int Get(string expressionKey)
        {
            return Execute(_expressions[expressionKey]);
        }
    }

    public class Expression
    {
        private List<int> _values = new List<int>(2);
        public char Operation { get; set; }

        public void AddValue(int value) =>
            _values.Add(value);

        public int Calculate()
        {
            int result = 0;

            switch (Operation)
            {
                case '+':
                    result = Sum();
                    break;
                case '-':
                    result = Substract();
                    break;
                case '*':
                    result = Multiplay();
                    break;
                case '/':
                    result = Divide();
                    break;
                default:
                    throw new ExceptionExecuteExpression("uavn");
            }

            return result;
        }

        private int Divide()
        {
            int result = 0;

            foreach (int item in _values)
                result /= item;

            _values.Clear();

            return result;
        }

        private int Multiplay()
        {
            int result = 0;

            foreach (int item in _values)
                result *= item;

            _values.Clear();

            return result;
        }

        private int Substract()
        {
            int result = 0;

            foreach (int item in _values)
                result += item;

            _values.Clear();

            return result;
        }

        private int Sum()
        {
            int result = 0;

            foreach (int item in _values)
                result += item;

            _values.Clear();

            return result;
        }
    }
}