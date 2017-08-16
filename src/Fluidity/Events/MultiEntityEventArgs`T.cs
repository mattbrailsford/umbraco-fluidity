using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluidity.Events
{
    public class MultiEntityEventArgs<TEntity> : EventArgs
    {
        public IEnumerable<TEntity> Entities { get; set; }
    }
}
