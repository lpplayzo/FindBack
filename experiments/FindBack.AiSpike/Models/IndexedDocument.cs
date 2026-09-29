namespace FindBack.AiSpike.Models
{
    public sealed record IndexedDocument(string FileName, string Content, float[] Embedding);

    public sealed record IndexedChunk(string FileName, int ChunkIndex, string Content, float[] Embedding);
}
