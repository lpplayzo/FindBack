using FindBack.AiSpike.Models;
using FindBack.AiSpike.Search;

namespace FindBack.AiSpike.Tests.Search
{
    public class SemanticSearcherTests
    {
        [Fact]
        public async Task SearchAsync_ReturnsChunkWithHighestSimilarity()
        {
            // Arrange
            FakeEmbeddingService fakeEmbeddingService = new FakeEmbeddingService();
            IndexedChunk indexedChunkDocA = new IndexedChunk("document-a", 0, "", [1, 0]);
            IndexedChunk indexedChunkDocB = new IndexedChunk("document-b", 0, "", [0, 1]);
            List<IndexedChunk> indexedChunks = [indexedChunkDocA, indexedChunkDocB];

            SemanticSearcher semanticSearcher = new SemanticSearcher(indexedChunks, fakeEmbeddingService);

            var result = await semanticSearcher.SearchAsync("query", 1);
            Assert.Single(result);
            Assert.Same(indexedChunkDocA, result[0].Chunk);
        }
    }
}
