using FindBack.AiSpike.Chunking;

namespace FindBack.AiSpike.Tests.Chunking;

public class FixedSizeTextChunkerTest
{

    [Fact]
    public void Chunk_TextShorterThanChunkSize_ReturnsSingleChunk()
    {
        // Arrange
        var chunker = new FixedSizeTextChunker(10, 1);
        var text = "Bonjour je suis ici";

        // Act
        var result = chunker.Chunk("test", text);

        // Assert
        Assert.Single(result);
        Assert.Equal(text, result[0].Content);
    }

    [Fact]
    public void Chunk_WhenTextIsEmpty_ReturnsEmptyCollection()
    {
        // Arrange
        var chunker = new FixedSizeTextChunker(5, 1);
        var text = "";

        // Act
        var result = chunker.Chunk("test", text);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Chunk_WithOverlap_ReusesWordsBetweenChunks()
    {
        // Arrange
        var chunker = new FixedSizeTextChunker(3, 1);
        var text = "A B C D E";

        // Act
        var result = chunker.Chunk("test", text);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("A B C", result[0].Content);
        Assert.Equal("C D E", result[1].Content);
    }

    [Fact]
    public void Chunk_WhenTextFitsInSingleChunk_ReturnsSingleChunk()
    {
        // Arrange
        var chunker = new FixedSizeTextChunker(10, 1);
        var text = "Bonjour";

        // Act
        var result = chunker.Chunk("test", text);

        // Assert
        Assert.Single(result);
        Assert.Equal("Bonjour", result[0].Content);
    }

    [Fact]
    public void Chunk_WhenWordsIsEqualToChunkSize_ReturnsSingleChunk()
    {
        // Arrange
        var chunker = new FixedSizeTextChunker(3, 1);
        var text = "A B C";

        // Act
        var result = chunker.Chunk("test", text);

        // Assert
        Assert.Single(result);
        Assert.Equal("A B C", result[0].Content);
    }

    [Fact]
    public void Constructor_WhenChunkSizeIsZero_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FixedSizeTextChunker(0, 0));
    }

    [Fact]
    public void Constructor_WhenOverlapIsNegative_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FixedSizeTextChunker(5, -1));
    }

    [Fact]
    public void Constructor_WhenChunkSizeIsLessOrEqualToZero_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FixedSizeTextChunker(0, 2));
    }

    [Fact]
    public void Constructor_WhenOverlapIsGreaterThanOrEqualToChunkSize_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FixedSizeTextChunker(5, 5));
    }
}
