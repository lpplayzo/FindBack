using FindBack.AiSpike.Models;

namespace FindBack.AiSpike.Chunking
{
    public interface ITextChunker
    {
        IReadOnlyList<DocumentChunk> Chunk(string documentName, string text);
    }
}
