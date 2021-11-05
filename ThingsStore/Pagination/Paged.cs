namespace ThingsStore.Pagination
{
    using System.Collections.Generic;
    using System.Linq;

    public record Paged<TItem>
        where TItem : class
    {
        public IEnumerable<TItem> Items { get; init; } = Enumerable.Empty<TItem>();

        public uint Limit { get; init; }

        public uint Offset { get; init; }

        public uint Total { get; init; }

        public Pagination NextPage => new ()
        {
            Limit = Limit,
            Offset = Offset + 1
        };
    }
}