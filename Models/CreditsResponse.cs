using System.Text.Json.Serialization;

namespace CineScope.Models
{
    /// <summary>
    /// Represents the credits response from the TMDB API,
    /// containing the cast and crew for a specific movie.
    /// </summary>
    public class CreditsResponse
    {
        /// <summary>The TMDB movie ID these credits belong to.</summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>The cast members billed in the movie.</summary>
        [JsonPropertyName("cast")]
        public Cast[] Cast { get; set; } = [];

        /// <summary>The crew members who worked on the movie.</summary>
        [JsonPropertyName("crew")]
        public Crew[] Crew { get; set; } = [];
    }

    /// <summary>
    /// Represents a single cast member returned by the TMDB credits API.
    /// </summary>
    public class Cast
    {
        /// <summary>Indicates whether the cast member is an adult content actor.</summary>
        [JsonPropertyName("adult")]
        public bool Adult { get; set; }

        /// <summary>Gender identifier returned by the TMDB API (0 = not set, 1 = female, 2 = male).</summary>
        [JsonPropertyName("gender")]
        public int Gender { get; set; }

        /// <summary>The TMDB person ID.</summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>The department the cast member is best known for (e.g. "Acting").</summary>
        [JsonPropertyName("known_for_department")]
        public string KnownForDepartment { get; set; } = string.Empty;

        /// <summary>The cast member's display name.</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>The cast member's original name.</summary>
        [JsonPropertyName("original_name")]
        public string OriginalName { get; set; } = string.Empty;

        /// <summary>The TMDB popularity score for this cast member.</summary>
        [JsonPropertyName("popularity")]
        public double Popularity { get; set; }

        /// <summary>
        /// The relative path to the cast member's profile image on TMDB.
        /// Nullable — the API returns null when no image is available.
        /// </summary>
        [JsonPropertyName("profile_path")]
        public string? ProfilePath { get; set; }

        /// <summary>The unique ID of this cast entry within the movie's credits.</summary>
        [JsonPropertyName("cast_id")]
        public int CastId { get; set; }

        /// <summary>The character name played by this cast member.</summary>
        [JsonPropertyName("character")]
        public string Character { get; set; } = string.Empty;

        /// <summary>The unique TMDB credit ID for this entry.</summary>
        [JsonPropertyName("credit_id")]
        public string CreditId { get; set; } = string.Empty;

        /// <summary>The billing order position of this cast member.</summary>
        [JsonPropertyName("order")]
        public int Order { get; set; }
    }

    /// <summary>
    /// Represents a single crew member returned by the TMDB credits API.
    /// </summary>
    public class Crew
    {
        /// <summary>Indicates whether the crew member is an adult content worker.</summary>
        [JsonPropertyName("adult")]
        public bool Adult { get; set; }

        /// <summary>Gender identifier returned by the TMDB API (0 = not set, 1 = female, 2 = male).</summary>
        [JsonPropertyName("gender")]
        public int Gender { get; set; }

        /// <summary>The TMDB person ID.</summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>The department the crew member is best known for (e.g. "Directing").</summary>
        [JsonPropertyName("known_for_department")]
        public string KnownForDepartment { get; set; } = string.Empty;

        /// <summary>The crew member's display name.</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>The crew member's original name.</summary>
        [JsonPropertyName("original_name")]
        public string OriginalName { get; set; } = string.Empty;

        /// <summary>The TMDB popularity score for this crew member.</summary>
        [JsonPropertyName("popularity")]
        public double Popularity { get; set; }

        /// <summary>
        /// The relative path to the crew member's profile image on TMDB.
        /// Nullable — the API returns null when no image is available.
        /// </summary>
        [JsonPropertyName("profile_path")]
        public string? ProfilePath { get; set; }

        /// <summary>The unique TMDB credit ID for this entry.</summary>
        [JsonPropertyName("credit_id")]
        public string CreditId { get; set; } = string.Empty;

        /// <summary>The production department this crew member worked in (e.g. "Camera").</summary>
        [JsonPropertyName("department")]
        public string Department { get; set; } = string.Empty;

        /// <summary>The specific job title held by this crew member (e.g. "Director of Photography").</summary>
        [JsonPropertyName("job")]
        public string Job { get; set; } = string.Empty;
    }
}
