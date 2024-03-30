using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace App.Scripts.Scenes.SceneCalculator.Features.Calculator
{
    public class CalculatorExpression : ICalculatorExpression
    {
        private const string s_correctKeyPatteren = @"^(?![а-яА-Я\d]+$)(?!.\d)(?=.[a-z])[a-zA-Z]+$";
        private const string s_correctExpressionPattern = @"^[0-9+\-*/()A-Za-z\s]+$";

        private Dictionary<string, string> _expressions = new Dictionary<string, string>();
        private Dictionary<char, BinaryMathOperation> _operations = new Dictionary<char, BinaryMathOperation>
        {
            {'(', new BinaryMathOperation('(',0,null)},
            {'+', new BinaryMathOperation('+',1,(x,y)=> x+y)},
            {'-', new BinaryMathOperation('-',1,(x,y)=> x-y)},
            {'*', new BinaryMathOperation('*',2,(x,y)=> x*y)},
            {'/', new BinaryMathOperation('/',2,(x,y)=> x/y)},
        };

        private Regex _regex;

        public int Execute(string expression)
        {
            string postfixExpression = Parse(expression);
            return new MathExpressionExecutor(_operations).Execute(postfixExpression);
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

        private string Parse(string _infixExprs)
        {
            string output = string.Empty;
            Stack<char> operators = new Stack<char>();
            bool isNegativeNumber=false;

            for (int i = 0; i < _infixExprs.Length; i++)
            {
                if (Char.IsDigit(_infixExprs[i]))
                    output += TakeNumber(_infixExprs, ref i,ref isNegativeNumber);

                else if (_infixExprs[i] == '(')
                    operators.Push(_infixExprs[i]);

                else if (_infixExprs[i] == ')')
                    output += TakePriorityOperations(ref operators);

                else if (char.IsLetter(_infixExprs[i]))
                    output += Get(TakeName(_infixExprs, ref i)).ToString()+" ";

                else if (_operations.ContainsKey(_infixExprs[i])) 
                    if (IsMinusForNegativeDigit(_infixExprs, i))
                        isNegativeNumber = true;
                    else
                        output += TakeOperations(ref operators, _infixExprs[i]);
            }

            foreach (var item in operators)
                output += item;

            return output;
        }

        private bool IsMinusForNegativeDigit(string infixExprs, int i)
        {
            bool isNegative = true;

            if (infixExprs[i]!='-')
                return false;

            for (int j = i-1; j >=0; j--)
            {
                if (char.IsSeparator(infixExprs[j]))
                    continue;

                if (char.IsDigit(infixExprs[j]))
                {
                    isNegative = false;
                    break;
                }

                if (_operations.ContainsKey(infixExprs[j]))
                    break;
            }

            return isNegative;
        }

        private string TakeName(string infixExprs, ref int i)
        {
            string attrName = string.Empty;
            int j;

            for (j= i; j < infixExprs.Length; j++)
            {
                if (char.IsLetter(infixExprs[j]))
                    attrName += infixExprs[j];
                else
                    break;
            }

            i = --j;
            return attrName;
        }

        private string TakeOperations(ref Stack<char> operators, char currentOperaation)
        {
            string output = string.Empty;

            while (operators.Count > 0 && _operations[operators.Peek()].Priority > _operations[currentOperaation].Priority)
                output += operators.Pop();

            operators.Push(currentOperaation);

            return output;
        }

        private string TakePriorityOperations(ref Stack<char> operators)
        {
            string output = string.Empty;

            while (operators.Peek() != '(')
                output += operators.Pop();

            operators.Pop();

            return output;
        }

        private string TakeNumber(string str, ref int i, ref bool isNegative)
        {
            char separator = ' ';
            StringBuilder output = new StringBuilder();
            int i1;

            for (i1 = i; i1 < str.Length; i1++)
            {
                if (char.IsDigit(str[i1]))
                {
                    output.Append(str[i1]);
                }
                else
                {
                   
                    break;
                }
            }

            i = --i1;

            if (isNegative)
                output.Insert(0, '-');

            isNegative = false;

            string result = output.ToString() + separator;
            return result;
        }
    }

    struct MathExpressionExecutor
    {
        private Dictionary<char, BinaryMathOperation> _operations;

        public MathExpressionExecutor(Dictionary<char, BinaryMathOperation> operations) 
        {
            _operations = operations;
        }

        public int Execute(string _postfixExprs) 
        {
            int result = 0;
            Stack<int> values = new Stack<int>();
            bool isNegative = false;

            for (int i = 0; i < _postfixExprs.Length; i++)
            {
                if (Char.IsDigit(_postfixExprs[i]))
                {
                    string value = TakeNumber(_postfixExprs, ref i, ref isNegative);
                    values.Push(int.Parse(value));
                }
                else if (_operations.ContainsKey(_postfixExprs[i]))
                {
                    if (IsMinusForNegativeDigit(_postfixExprs, i))
                    {
                        isNegative = true;
                        continue;
                    }

                    int secondValue = values.Pop();
                    int firstValue = values.Pop();
                    result = _operations[_postfixExprs[i]].Operation.Invoke(firstValue, secondValue);
                    values.Push(result);
                }
            }

            return values.Pop();
        }

        private string TakeNumber(string str, ref int i, ref bool isNegative)
        {
            char separator = ' ';
            StringBuilder output = new StringBuilder();
            int i1;

            for (i1 = i; i1 < str.Length; i1++)
            {
                if (char.IsDigit(str[i1]))
                {
                    output.Append(str[i1]);
                }
                else
                {
                   
                    break;
                }
            }

            i = --i1;

            if (isNegative)
                output.Insert(0, '-');

            isNegative = false;

            string result = output.ToString() + separator;
            return result;
        }

        private bool IsMinusForNegativeDigit(string infixExprs, int i)
        {
            bool isNegative = true;

            if (infixExprs[i] != '-')
                return false;

            for (int j = i-1; j >= 0; j--)
            {
                if (char.IsSeparator(infixExprs[j]))
                    continue;

                if (char.IsDigit(infixExprs[j]))
                {
                    isNegative = false;
                    break;
                }

                if (_operations.ContainsKey(infixExprs[j]))
                {
                    isNegative = false;
                    break;
                }
            }

            return isNegative;
        }
    }

    struct BinaryMathOperation
    {
        public char Symbol { get; private set; }
        public int Priority { get; private set; }
        public Func<int,int,int> Operation { get; private set; }

        public BinaryMathOperation(char symbol, int priority, Func<int, int, int> operation)
        {
            Symbol = symbol;
            Priority = priority;
            Operation = operation;
        }
    }
}