using VideoGameCharacterApi.Dtos;

namespace VideoGameCharacterApi.Services
{
    public interface IRequestContext
    {
        string Method { get; }
        string Path { get; }
        DateTime StartTimeUtc { get; }
        string CorrelationId { get; }

        // Small helper: health/ping skip
        bool IsHealthOrPing { get; }


        // Minimal, useful capability backed by IVideoGameCharacterService
        Task<ConnectionCheckResponse> CheckDbAsync();


    }
}
