using System;
using System.Collections.Generic;
using System.Text;

namespace FindBack.AiSpike.Search
{
    public static class VectorMath
    {
        public static double CosineSimilarity(ReadOnlySpan<float> a, ReadOnlySpan<float> b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException("Vectors must have the same dimensions");

            double dotProduct = 0;
            double magnitudeA = 0;
            double magnitudeB = 0;

            for (int i = 0; i < a.Length; i++)
            {
                dotProduct += a[i] * b[i];
                magnitudeA += a[i] * a[i];
                magnitudeB += b[i] * b[i];
            }

            if (magnitudeA == 0 || magnitudeB == 0) return 0;

            return dotProduct / (Math.Sqrt(magnitudeA) *  Math.Sqrt(magnitudeB));
        }
    }
}
