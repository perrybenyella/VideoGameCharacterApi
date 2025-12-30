namespace VideoGameCharacterApi.Dtos
{
    public class ConnectionCheckResponse
    {
        public bool EfCoreCanConnect { get; init; }
        public bool RawCanOpen { get; init; }
        public string? Error { get; init; }
    }
}
