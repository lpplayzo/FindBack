using Microsoft.Extensions.AI;

namespace FindBack.AiSpike.AI
{
    public class MicrosoftEmbeddingService : IEmbeddingService
    {
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;

        public MicrosoftEmbeddingService(IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
        {
            _embeddingGenerator = embeddingGenerator;
        }
        public async Task<float[]> GenerateAsync(string text, CancellationToken cancellationToken = default)
        {
            var embedding = await _embeddingGenerator.GenerateVectorAsync(text, cancellationToken: cancellationToken);
            return embedding.ToArray();
        }
    }
}
