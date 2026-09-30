using FindBack.AiSpike.AI;
using FindBack.AiSpike.Chunking;
using FindBack.AiSpike.Models;

namespace FindBack.AiSpike.Indexing
{
    public class DocumentIndexer
    {
        private readonly ITextChunker _textChunker;
        private readonly IEmbeddingService _embeddingService;
        public DocumentIndexer(ITextChunker textChunker, IEmbeddingService embeddingService)
        {
            _textChunker = textChunker;
            _embeddingService = embeddingService;
        }

        public async Task<IReadOnlyList<IndexedChunk>> IndexDirectoryAsync(string directoryPath)
        {
            var result = new List<IndexedChunk>();
            var files = Directory.GetFiles(directoryPath, "*.txt");
            foreach (var file in files)
            {
                var indexedChunk = await IndexFileAsync(file);
                result.AddRange(indexedChunk);
            }
            return result;
        }

        public async Task<IReadOnlyList<IndexedChunk>> IndexFileAsync(string filePath)
        {
            var fileContent = await File.ReadAllTextAsync(filePath);
            var fileName = Path.GetFileName(filePath);
            var result = new List<IndexedChunk>();
            var chunks = _textChunker.Chunk(fileName, fileContent);
            foreach(var chunk in chunks)
            {
                var embeddings = await _embeddingService.GenerateAsync(chunk.Content);
                var indexedChunk = new IndexedChunk(chunk.DocumentName, chunk.Index, chunk.Content, embeddings);
                result.Add(indexedChunk);
            }
            return result;
        }
    }
}
