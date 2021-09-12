namespace ThingsStore.Services
{
    public record Product
    {
        public uint Cost { get; init; }

        public string Description { get; init; }

        public string Name { get; init; }
    }
}