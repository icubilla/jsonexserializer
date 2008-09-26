using System;
using System.Collections.Generic;
using System.Text;
using JsonExSerializer.Expression;

namespace JsonExSerializer.Framework.ObjectHandlers
{
    public class NullObjectHandler : ObjectHandlerBase
    {
        public override ExpressionBase GetExpression(object data, JsonPath CurrentPath, ISerializerHandler Serializer)
        {
            return new NullExpression();
        }

        public override bool CanHandle(Type ObjectType)
        {
            return false;
        }

        public override bool CanHandle(ExpressionBase Expression)
        {
            return (Expression is NullExpression);
        }

        public override object Evaluate(ExpressionBase Expression, IDeserializerHandler Deserializer)
        {
            NullExpression nullExpr = (NullExpression)Expression;
            return null;
        }

        public override object Evaluate(ExpressionBase Expression, object ExistingObject, IDeserializerHandler Deserializer)
        {
            return ExistingObject;
        }
    }
}
