using FindBack.AiSpike.AI;

namespace FindBack.AiSpike.Tests.Search
{
    public class FakeEmbeddingService : IEmbeddingService
    {
        public Task<float[]> GenerateAsync(string text, CancellationToken cancellationToken = default)
        {
            switch (text)
            {
                case "query":
                    return Task.FromResult(new float[] { 1, 0 });
                case "document-a":
                    return Task.FromResult(new float[] { 1, 0 });
                case "document-b":
                    return Task.FromResult(new float[] { 0, 1 });
                default:
                    return Task.FromResult(new float[] { 0, 0 });
            }
        }
    }
}