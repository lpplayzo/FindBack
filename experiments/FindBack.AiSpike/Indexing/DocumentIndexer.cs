using FindBack.AiSpike.Chunking;
using FindBack.AiSpike.Models;
using Microsoft.Extensions.AI;

namespace FindBack.AiSpike.Indexing
{
    public class DocumentIndexer
    {
        private readonly ITextChunker _textChunker;
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;
        public DocumentIndexer(ITextChunker textChunker, IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
        {
            _textChunker = textChunker;
            _embeddingGenerator = embeddingGenerator;
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
                var embeddings = await _embeddingGenerator.GenerateVectorAsync(chunk.Content);
                var indexedChunk = new IndexedChunk(chunk.DocumentName, chunk.Index, chunk.Content, embeddings.ToArray());
                result.Add(indexedChunk);
            }
            return result;
        }
    }
}
