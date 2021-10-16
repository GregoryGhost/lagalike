namespace ThingsStore.Pagination
{
    using System.Collections.Generic;
    using System.Linq;

    public class Paginator<TItem>
        where TItem : class
    {
        public Paged<TItem> Paginate(Pagination paging, IEnumerable<TItem> products)
        {
            var items = products.Skip((int)paging.Offset).Take((int)paging.Limit);
            return new Paged<TItem>
            {
                Items = items,
                Limit = paging.Limit,
                Offset = paging.Offset,
                Total = (uint)products.Count()
            };
        }
    }
}