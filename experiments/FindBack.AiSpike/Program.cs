using FindBack.AiSpike;
using Microsoft.Extensions.AI;
using OllamaSharp;

const string modelName = "nomic-embed-text-v2-moe";

IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
    new OllamaApiClient(
        new Uri("http://localhost:11434/"),
        modelName);

var documentsDirectory = Path.Combine(
    AppContext.BaseDirectory,
    "documents");

if (!Directory.Exists(documentsDirectory))
{
    Console.WriteLine(
        $"Documents directory not found: {documentsDirectory}");

    return;
}

var indexedDocuments = new List<IndexedDocument>();

foreach (var filePath in Directory.EnumerateFiles(
             documentsDirectory,
             "*.txt"))
{
    var content = await File.ReadAllTextAsync(filePath);

    var embedding =
        await embeddingGenerator.GenerateVectorAsync(content);

    indexedDocuments.Add(
        new IndexedDocument(
            Path.GetFileName(filePath),
            content,
            embedding.ToArray()));

    Console.WriteLine(
        $"Indexed: {Path.GetFileName(filePath)}");
}

Console.WriteLine();
Console.Write("Search: ");

var query = Console.ReadLine();

if (string.IsNullOrWhiteSpace(query))
{
    Console.WriteLine("Search query cannot be empty");
    return;
}

var queryEmbedding =
    await embeddingGenerator.GenerateVectorAsync(query);

var results = indexedDocuments
    .Select(document => new
    {
        Document = document,

        Score = VectorMath.CosineSimilarity(
            queryEmbedding.Span,
            document.Embedding)
    })
    .OrderByDescending(result => result.Score)
    .ToList();

Console.WriteLine();
Console.WriteLine("Results: ");
Console.WriteLine();

foreach (var result in results)
{
    Console.WriteLine(
        $"{result.Score:F4} - {result.Document.FileName}");
}