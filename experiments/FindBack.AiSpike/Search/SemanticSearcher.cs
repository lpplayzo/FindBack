using FindBack.AiSpike.AI;
using FindBack.AiSpike.Models;

namespace FindBack.AiSpike.Search
{
    public class SemanticSearcher
    {
        private readonly IEmbeddingService _embeddingService;
        private readonly IReadOnlyList<IndexedChunk> _indexedChunks;
        public SemanticSearcher(IReadOnlyList<IndexedChunk> indexedChunks, IEmbeddingService embeddingService)
        {
            _indexedChunks = indexedChunks;
            _embeddingService = embeddingService;
        }

        public async Task<IReadOnlyList<(IndexedChunk Chunk, double Score)>> SearchAsync(string query, int topK)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(topK, 1);
            var result = new List<(IndexedChunk Chunk, double Score)>();
            var queryEmbedding = await _embeddingService.GenerateAsync(query);

            foreach (var chunk in _indexedChunks)
            {
                var similarity = VectorMath.CosineSimilarity(chunk.Embedding, queryEmbedding);
                result.Add((chunk, similarity));
            }
            var orderedResult = result.OrderByDescending(x => x.Score).Take(topK).ToList();
            return orderedResult;
        }

    }
}
