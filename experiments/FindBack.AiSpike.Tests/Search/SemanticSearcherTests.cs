using FindBack.AiSpike.Chunking;
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

        [Fact]
        public async Task SearchAsync_ReturnsTopKHighestChunks()
        {
            // Arrange
            FakeEmbeddingService fakeEmbeddingService = new FakeEmbeddingService();
            IndexedChunk indexedChunkDocA = new IndexedChunk("document-a", 0, "", [1f, 0f]);
            IndexedChunk indexedChunkDocB = new IndexedChunk("document-b", 0, "", [0.8f, 0.2f]);
            IndexedChunk indexedChunkDocC = new IndexedChunk("document-c", 0, "", [0f, 1f]);
            List<IndexedChunk> indexedChunks = [indexedChunkDocA, indexedChunkDocB, indexedChunkDocC];
            SemanticSearcher semanticSearcher = new SemanticSearcher(indexedChunks, fakeEmbeddingService);


            // Act
            var result = await semanticSearcher.SearchAsync("query", 2);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(indexedChunkDocA, result[0].Chunk);
            Assert.Equal(indexedChunkDocB, result[1].Chunk);
        }

        [Fact]
        public async Task SearchAsync_WhenTopKExceedsChunkCount_ReturnsAllChunks()
        {
            // Arrange
            FakeEmbeddingService fakeEmbeddingService = new FakeEmbeddingService();
            IndexedChunk indexedChunkDocA = new IndexedChunk("document-a", 0, "", [1f, 0f]);
            IndexedChunk indexedChunkDocB = new IndexedChunk("document-b", 0, "", [0.8f, 0.2f]);
            IndexedChunk indexedChunkDocC = new IndexedChunk("document-c", 0, "", [0f, 1f]);
            List<IndexedChunk> indexedChunks = [indexedChunkDocA, indexedChunkDocB, indexedChunkDocC];
            SemanticSearcher semanticSearcher = new SemanticSearcher(indexedChunks, fakeEmbeddingService);

            // Act
            var result = await semanticSearcher.SearchAsync("query", 10);

            // Assert 
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task SearchAsync_WhenNoChunks_ReturnsEmptyList()
        {
            // Arrange
            FakeEmbeddingService fakeEmbeddingService = new FakeEmbeddingService();
            List<IndexedChunk> indexedChunks = [];
            SemanticSearcher semanticSearcher = new SemanticSearcher(indexedChunks, fakeEmbeddingService);

            // Act
            var result = await semanticSearcher.SearchAsync("query", 3);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchAsync_WhenTopKIsLessThanOne_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            FakeEmbeddingService fakeEmbeddingService = new FakeEmbeddingService();
            List<IndexedChunk> indexedChunks = [];
            SemanticSearcher semanticSearcher = new SemanticSearcher(indexedChunks, fakeEmbeddingService);

            // Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => semanticSearcher.SearchAsync("query", 0));
        }
    }
}
