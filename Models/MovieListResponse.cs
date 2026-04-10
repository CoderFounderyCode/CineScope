using System.Text.Json.Serialization;

namespace CineScope.Models
{
    public class MovieListResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("results")]
        public List<Movie> Results { get; set; } = [];

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
    }
}
