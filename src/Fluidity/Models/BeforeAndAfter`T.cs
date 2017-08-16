namespace Fluidity.Models
{
    public class BeforeAndAfter<TEntityType>
    {
        public TEntityType Before { get; set; }
        public TEntityType After { get; set; }
    }
}
