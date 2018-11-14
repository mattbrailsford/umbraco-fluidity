using System;

namespace Fluidity.Models
{
    internal class GetterAndSetter
    {
        internal string PropertyName { get; set; }
        internal Func<object, object> Getter { get; set; }
        internal Action<object, object> Setter { get; set; }
    }
}
