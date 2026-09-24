using OllamaSharp;

var ollama = new OllamaApiClient(
    "http://localhost:11434",
    "nomic-embed-text-v2-moe");

var sentenceA =
    "PostgreSQL est une base de données relationnelle.";

var sentenceB =
    "J'utilise Postgres pour stocker les données de mon application.";

var sentenceC =
    "Mon chat dort sur le canapé.";

var embeddingA =
    (await ollama.EmbedAsync(sentenceA)).Embeddings[0];

var embeddingB =
    (await ollama.EmbedAsync(sentenceB)).Embeddings[0];

var embeddingC =
    (await ollama.EmbedAsync(sentenceC)).Embeddings[0];

Console.WriteLine(
    $"A ↔ B : {CosineSimilarity(embeddingA, embeddingB):F4}");

Console.WriteLine(
    $"A ↔ C : {CosineSimilarity(embeddingA, embeddingC):F4}");

static double CosineSimilarity(float[] a, float[] b)
{
    if (a.Length != b.Length)
        throw new ArgumentException("Vectors must have the same dimensions.");

    double dotProduct = 0;
    double magnitudeA = 0;
    double magnitudeB = 0;

    for (int i = 0; i < a.Length; i++)
    {
        dotProduct += a[i] * b[i];
        magnitudeA += a[i] * a[i];
        magnitudeB += b[i] * b[i];
    }

    if (magnitudeA == 0 || magnitudeB == 0)
        return 0;

    return dotProduct /
           (Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
}