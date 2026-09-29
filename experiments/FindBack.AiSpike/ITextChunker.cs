namespace FindBack.AiSpike
{
    public interface ITextChunker
    {
        IReadOnlyList<DocumentChunk> Chunk(string documentName, string text);
    }
}
