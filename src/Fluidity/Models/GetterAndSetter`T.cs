using System;

namespace Fluidity.Models
{
    internal class GetterAndSetter<TEntityType, TValueType>
    {
        internal string PropertyName { get; set; }

        internal Func<TEntityType, TValueType> Getter { get; set; }
        internal Action<TEntityType, TValueType> Setter { get; set; }

        internal Func<object, object> ObjectGetter => (obj) => Getter.Invoke((TEntityType)obj) as object;
        internal Action<object, object> ObjectSetter => (obj, value) => Setter.Invoke((TEntityType)obj, (TValueType)value);
    }
}
