using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VideoGameCharacterApi.Data;
using VideoGameCharacterApi.Dtos;
using VideoGameCharacterApi.Models;

namespace VideoGameCharacterApi.Services;

public class VideoGameCharacterService(AppDbContext context) : IVideoGameCharacterService
{
    public async Task<CharacterResponse> AddCharacterAsync(CreateCharacterRequest character)
    {
        var newCharacter = new Character
        {
            Name = character.Name,
            Game = character.Game,
            Role = character.Role
        };

        context.Characters.Add(newCharacter);
        await context.SaveChangesAsync();

        return new CharacterResponse
        {
            Id = newCharacter.Id,
            Name = newCharacter.Name,
            Game = newCharacter.Game,
            Role = newCharacter.Role
        };
    }

    public async Task<bool> DeleteCharacterAsync(int id)
    {
        var characterToDelete = await context.Characters.FindAsync(id);
        if (characterToDelete is null)
            return false;

        context.Characters.Remove(characterToDelete);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<List<CharacterResponse>> GetAllCharactersAsync()
        => await context.Characters.Select(c => new CharacterResponse
        {
            Id = c.Id,
            Name = c.Name,
            Game = c.Game,
            Role = c.Role
        }).ToListAsync();

    public async Task<CharacterResponse?> GetCharacterByIdAsync(int id)
    {
        var result = await context.Characters
            .Where(c => c.Id == id)
            .Select(c => new CharacterResponse
            {
                Id = c.Id,
                Name = c.Name,
                Game = c.Game,
                Role = c.Role
            })
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<bool> UpdateCharacterAsync(int id, UpdateCharacterRequest character)
    {
        var existingCharacter = await context.Characters.FindAsync(id);
        if (existingCharacter is null)
            return false;

        existingCharacter.Name = character.Name;
        existingCharacter.Game = character.Game;
        existingCharacter.Role = character.Role;

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<ConnectionCheckResponse> TestConnectionAsync()
    {

        bool efCoreOk = false;
        bool rawOk = false;
        string? error = null;

        try
        {
            // EF Core layer check: returns true/false (may throw if EF is misconfigured)
            efCoreOk = await context.Database.CanConnectAsync();
        }
        catch (Exception ex)
        {
            // EF misconfiguration, provider issues, DI problems, etc.
            error = $"EF Core CanConnectAsync threw: {ex.Message}";
            return new ConnectionCheckResponse { EfCoreCanConnect = false, RawCanOpen = false, Error = error };
        }

        // If EF layer says “false”, still attempt raw to get a more specific exception
        var cs = context.Database.GetDbConnection().ConnectionString;

        try
        {
            // Raw ADO.NET: detailed exceptions on server not found, login failure, TLS, etc.
            using var conn = new SqlConnection(cs);
            await conn.OpenAsync();
            rawOk = true;
        }
        catch (Exception ex)
        {
            rawOk = false;
            error = $"SqlConnection.OpenAsync threw: {ex.Message}";
        }

        return new ConnectionCheckResponse
        {
            EfCoreCanConnect = efCoreOk,
            RawCanOpen = rawOk,
            Error = error
        };

    }
}
