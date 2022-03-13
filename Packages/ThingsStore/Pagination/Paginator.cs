namespace ThingsStore.Pagination
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Paginator
    {
        public static Paged<TItem> Paginate<TItem>(this IEnumerable<TItem> sourceItems, Pagination paging)
            where TItem : class
        {
            var items = sourceItems.Skip((int)paging.Offset).Take((int)paging.Limit).ToList();
            return new Paged<TItem>
            {
                Items = items,
                Limit = paging.Limit,
                Offset = paging.Offset,
                Total = (uint)sourceItems.Count()
            };
        }
    }
}