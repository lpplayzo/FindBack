using FindBack.AiSpike;
using Microsoft.Extensions.AI;
using OllamaSharp;

const string modelName = "nomic-embed-text-v2-moe";
const string searchQuery = "\"Comment lancer PostgreSQL avec Docker ?";


string dataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "documents");
var filesPath = Directory.GetFiles(dataFilePath);

IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
        new OllamaApiClient(
            new Uri("http://localhost:11434/"),
            modelName);

var queryEmbedding = await embeddingGenerator.GenerateVectorAsync(searchQuery);

var textChunker = new FixedSizeTextChunker(10, 1);
var indexedChunks = new List<IndexedChunk>();
var searchResults = new List<(IndexedChunk Chunk, double Score)>();


List<IReadOnlyList<DocumentChunk>> res = new List<IReadOnlyList<DocumentChunk>>();

foreach (var file in filesPath)
{
    var data = File.ReadAllText(file);
    var fileName = Path.GetFileName(file);
    res.Add(textChunker.Chunk(fileName, data));
}

foreach(var result in res)
{
    if(result.Count > 0) Console.WriteLine(result.FirstOrDefault().DocumentName);
    for (int i = 0; i < result.Count; i++)
    {
        var embedding = await embeddingGenerator.GenerateVectorAsync(result[i].Content);
        var indexedChunk = new IndexedChunk(result[i].DocumentName, result[i].Index, result[i].Content, embedding.ToArray());
        indexedChunks.Add(indexedChunk);
    }
}

foreach (var chunk in indexedChunks)
{
    var similarity = VectorMath.CosineSimilarity(chunk.Embedding, queryEmbedding.Span);
    searchResults.Add((chunk, similarity));
}

var orderedResults = searchResults.OrderByDescending(x => x.Score).Take(3);

foreach (var res1 in orderedResults)
{
    Console.WriteLine($"Chunk {res1.Chunk.ChunkIndex} : Nom du fichier : {res1.Chunk.FileName} Score {res1.Score.ToString("F3")} Contenu : {res1.Chunk.Content}");
}