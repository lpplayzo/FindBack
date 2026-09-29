using FindBack.AiSpike.Models;

namespace FindBack.AiSpike.Chunking
{
    public sealed class FixedSizeTextChunker : ITextChunker
    {
        private readonly int _chunkSize;
        private readonly int _overlap;

        public FixedSizeTextChunker(int chunkSize, int overlap)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(chunkSize, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(overlap, 0);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(overlap, chunkSize);

            _chunkSize = chunkSize;
            _overlap = overlap;
        }

        public IReadOnlyList<DocumentChunk> Chunk(string documentName, string text)
        {
            string[] separators = { "\n", " ", "\t" };
            var chunkList = new List<DocumentChunk>();
            var list = text.Split(separators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            for (int i = 0; i < list.Length; i += (_chunkSize - _overlap))
            {
                if (_overlap >= (list.Length - i) && i > 0) break;
                var chunkWords = list.Skip(i).Take(_chunkSize);
                DocumentChunk words = new(documentName, chunkList.Count, string.Join(" ", chunkWords));
                chunkList.Add(words);
            }
            return chunkList;
        }
    }
}
