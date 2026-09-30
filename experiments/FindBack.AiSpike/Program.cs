using FindBack.AiSpike.AI;
using FindBack.AiSpike.Chunking;
using FindBack.AiSpike.Indexing;
using FindBack.AiSpike.Search;
using Microsoft.Extensions.AI;
using OllamaSharp;

const string modelName = "nomic-embed-text-v2-moe";
const string searchQuery = "Comment lancer PostgreSQL avec Docker ?";

string dataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "documents");

IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
        new OllamaApiClient(
            new Uri("http://localhost:11434/"),
            modelName);

MicrosoftEmbeddingService microsoftEmbeddingService = new(embeddingGenerator);

var textChunker = new FixedSizeTextChunker(10, 1);
var documentIndexer = new DocumentIndexer(textChunker, microsoftEmbeddingService);
var indexedChunks = await documentIndexer.IndexDirectoryAsync(dataFilePath);
var semanticSearcher = new SemanticSearcher(indexedChunks, microsoftEmbeddingService);

foreach (var res1 in await semanticSearcher.SearchAsync(searchQuery, 3))
{
    Console.WriteLine($"Chunk {res1.Chunk.ChunkIndex} : Nom du fichier : {res1.Chunk.FileName} Score {res1.Score.ToString("F3")} Contenu : {res1.Chunk.Content}");
}