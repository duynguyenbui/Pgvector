namespace Database.PgVector;

public class AIOptions
{
    /// <summary>Settings related to the use of OpenAI.</summary>
    public EmbeddingAIOptions EmbeddingAI { get; set; } = new();
}

public class EmbeddingAIOptions
{
    /// <summary>The name of the embedding model to use.</summary>
    /// <remarks>When using Azure OpenAI, this should be the "Deployment name" of the embedding model.</remarks>
    public string Endpoint { get; set; }
    public string AuthorizationToken { get; set; }
    
}