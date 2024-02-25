using System.Net.Http.Headers;
using Pgvector;

namespace Database.PgVector.Services;

public class CatalogAI : ICatalogAI
{
    private readonly string _aiEmbeddingEndpoint;
    private readonly string _authorizationToken;

    /// <summary>The web host environment.</summary>
    private readonly IWebHostEnvironment _environment;

    /// <summary>Logger for use in AI operations.</summary>
    private readonly ILogger _logger;

    private readonly HttpClient _client;

    public CatalogAI(IOptions<AIOptions> options, IWebHostEnvironment environment, ILogger<CatalogAI> logger,
        HttpClient client)
    {
        var aiOptions = options.Value;
        _aiEmbeddingEndpoint = options.Value.EmbeddingAI.Endpoint;
        _authorizationToken = options.Value.EmbeddingAI.AuthorizationToken;

        _environment = environment;
        _logger = logger;
        _client = client;

        IsEnabled = aiOptions != null;

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Embedding model: \"{model}\"", _aiEmbeddingEndpoint);
        }
    }

    /// <summary>Gets whether the AI system is enabled.</summary>
    public bool IsEnabled { get; }

    /// <summary>Gets an embedding vector for the specified text.</summary>
    public async ValueTask<Vector> GetEmbeddingAsync(string text)
    {
        if (!IsEnabled)
        {
            return null;
        }

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Getting embedding for \"{text}\"", text);
        }

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _aiEmbeddingEndpoint);
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationToken);
        httpRequestMessage.Content = JsonContent.Create(new { inputs = text });

        var response = await _client.SendAsync(httpRequestMessage);

        if (!response.IsSuccessStatusCode) return null;

        var embedding = await response.Content.ReadFromJsonAsync<float[]>();
        return new Vector(embedding);
    }

    /// <summary>Gets an embedding vector for the specified catalog item.</summary>
    public ValueTask<Vector> GetEmbeddingAsync(CatalogItem item) => IsEnabled
        ? GetEmbeddingAsync($"{item.Name} {item.Description}")
        : ValueTask.FromResult<Vector>(null);
}