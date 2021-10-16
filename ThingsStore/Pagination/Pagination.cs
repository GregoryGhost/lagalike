namespace ThingsStore.Pagination
{
    public record Pagination
    {
        public uint Limit { get; init; }

        public uint Offset { get; init; }
    }
}