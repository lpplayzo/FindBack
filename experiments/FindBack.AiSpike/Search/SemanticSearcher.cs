using FindBack.AiSpike.Models;
using Microsoft.Extensions.AI;

namespace FindBack.AiSpike.Search
{
    public class SemanticSearcher
    {
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;
        private readonly IReadOnlyList<IndexedChunk> _indexedChunks;
        public SemanticSearcher(IReadOnlyList<IndexedChunk> indexedChunks, IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
        {
            _indexedChunks = indexedChunks;
            _embeddingGenerator = embeddingGenerator;
        }

        public async Task<IReadOnlyList<(IndexedChunk Chunk, double Score)>> SearchAsync(string query, int topK)
        {
            var result = new List<(IndexedChunk Chunk, double Score)>();
            var queryEmbedding = await _embeddingGenerator.GenerateVectorAsync(query);

            foreach (var chunk in _indexedChunks)
            {
                var similarity = VectorMath.CosineSimilarity(chunk.Embedding, queryEmbedding.Span);
                result.Add((chunk, similarity));
            }
            var orderedResult = result.OrderByDescending(x => x.Score).Take(topK).ToList();
            return orderedResult;
        }

    }
}
