/*
 * Copyright (c) 2007, Ted Elliott
 * Code licensed under the New BSD License:
 * http://code.google.com/p/jsonexserializer/wiki/License
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonExSerializer.Expression
{
    class ValueEvaluator : EvaluatorBase {

        public ValueEvaluator(ValueExpression expression)
            : base(expression)
        {
        }

        public override object Evaluate() {
            if (Expression.ResultType.IsEnum)
                return Enum.Parse(Expression.ResultType, Expression.StringValue);
            else if (Expression.ResultType == typeof(object))
                return Expression.StringValue;
            else if (Expression.ResultType == typeof(string))
                return Expression.StringValue;
            else
                return Convert.ChangeType(Expression.StringValue, Expression.ResultType);
        }

        public new ValueExpression Expression
        {
            get { return (ValueExpression)_expression; }
            set { _expression = value; }
        }

        protected override object Construct()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void UpdateResult()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    sealed class NumericEvaluator : ValueEvaluator
    {
        public NumericEvaluator(NumericExpression expression)
            : base(expression)
        {
        }

        public override object Evaluate()
        {
            if (Expression.ResultType == typeof(object))
            {
                if (Expression.StringValue.Contains("."))
                    return Convert.ToDouble(Expression.StringValue);
                else
                    return Convert.ToInt64(Expression.StringValue);
            }
            else
            {
                return base.Evaluate();
            }
        }
    }

    sealed class BooleanEvaluator : ValueEvaluator
    {
        public BooleanEvaluator(BooleanExpression expression)
            : base(expression)
        {
        }

        public override object Evaluate()
        {
            if (Expression.ResultType == typeof(object))
            {
                return Convert.ToBoolean(Expression.StringValue);
            }
            else
            {
                return base.Evaluate();
            }
        }
    }
}