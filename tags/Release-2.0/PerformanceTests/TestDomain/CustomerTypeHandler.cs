using System;
using System.Collections.Generic;
using System.Text;
using JsonExSerializer.MetaData;
using JsonExSerializer;

namespace PerformanceTests.TestDomain
{
    public class CustTypeHandlerFactory : TypeHandlerFactory
    {
        public CustTypeHandlerFactory(SerializationContext Context)
            : base(Context)
        {
        }

        protected override ITypeHandler CreateNew(Type forType)
        {
            if (forType == typeof(Customer))
                return new CustomerTypeHandler(forType, this.Context);
            else
                return base.CreateNew(forType);
        }
    }
    public class CustomerTypeHandler : TypeHandler
    {
        public CustomerTypeHandler(Type t, SerializationContext ctx)
            : base(t, ctx)
        {
        }

        protected override void ReadProperties(out IList<IPropertyHandler> Properties, out IList<IPropertyHandler> ConstructorArguments)
        {
            Properties = new List<IPropertyHandler>();
            Properties.Add(new FirstPH(this.ForType));
            Properties.Add(new LastPH(this.ForType));
            Properties.Add(new PhonePH(this.ForType));
            Properties.Add(new SSNPH(this.ForType));
            Properties.Add(new AgePH(this.ForType));
            ConstructorArguments = new List<IPropertyHandler>();
        }
    }

    public abstract class CustomerPHBase : MemberHandlerBase, IPropertyHandler {
        private string _name;

        public CustomerPHBase(Type forType, string name) : base(forType) {
            _name = name;
        }

        public string Name { get { return _name; } }
        public int Position { get { return -1; } }
        public bool IsConstructorArgument { get { return false; } }
        public bool Ignored
        {
            get { return false; }
            set { ; }
        }

        public abstract object GetValue(object instance);
        public abstract Type PropertyType { get; }
        public abstract void SetValue(object instance, object value);
        protected override JsonExSerializer.TypeConversion.IJsonTypeConverter CreateTypeConverter()
        {
            return null;
        }
    }

    public class FirstPH : CustomerPHBase
    {
        public FirstPH(Type forType)
            : base(forType, "FirstName")
        {
        }

        public override object GetValue(object instance)
        {
            return ((Customer)instance).FirstName;
        }
        public override Type PropertyType
        {
            get { return typeof(string); }
        }
        public override void SetValue(object instance, object value)
        {
            ((Customer)instance).FirstName = (string)value;
        }
    }
    public class LastPH : CustomerPHBase
    {
        public LastPH(Type forType)
            : base(forType, "LastName")
        {
        }

        public override object GetValue(object instance)
        {
            return ((Customer)instance).LastName;
        }
        public override Type PropertyType
        {
            get { return typeof(string); }
        }
        public override void SetValue(object instance, object value)
        {
            ((Customer)instance).LastName = (string)value;
        }
    }
    public class PhonePH : CustomerPHBase
    {
        public PhonePH(Type forType)
            : base(forType, "Phone")
        {
        }

        public override object GetValue(object instance)
        {
            return ((Customer)instance).Phone;
        }
        public override Type PropertyType
        {
            get { return typeof(string); }
        }
        public override void SetValue(object instance, object value)
        {
            ((Customer)instance).Phone = (string)value;
        }
    }
    public class SSNPH : CustomerPHBase
    {
        public SSNPH(Type forType)
            : base(forType, "Ssn")
        {
        }

        public override object GetValue(object instance)
        {
            return ((Customer)instance).Ssn;
        }
        public override Type PropertyType
        {
            get { return typeof(string); }
        }
        public override void SetValue(object instance, object value)
        {
            ((Customer)instance).Ssn = (string)value;
        }
    }
    public class AgePH : CustomerPHBase
    {
        public AgePH(Type forType)
            : base(forType, "Age")
        {
        }

        public override object GetValue(object instance)
        {
            return ((Customer)instance).Age;
        }
        public override Type PropertyType
        {
            get { return typeof(int); }
        }
        public override void SetValue(object instance, object value)
        {
            ((Customer)instance).Age = (int)value;
        }
    }

}