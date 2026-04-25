using CineScope.Models;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace CineScope.Services
{
    /// <summary>
    /// Provides methods for interacting with The Movie Database (TMDB) API.
    /// </summary>
    public class TMDBService
    {
        private readonly HttpClient _http;
        private const string ImageBaseUrl = "https://image.tmdb.org/t/p/w500";

        /// <summary>
        /// Initializes a new instance of <see cref="TMDBService"/>.
        /// </summary>
        /// <param name="http">The HTTP client used to make requests to the TMDB API.</param>
        /// <param name="config">The application configuration, used to retrieve the TMDB API access key.</param>
        public TMDBService(HttpClient http, IConfiguration config)
        {
            _http = http;

            string tmdbApiKey = config["TmdbAccessKey"];
            if (!string.IsNullOrEmpty(tmdbApiKey))
            {
                _http.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {tmdbApiKey}");
            }
            else {
                // deployed 
                _http.BaseAddress = new Uri("https://cinescopeplus.netlify.app/" + "tmdb/");
            }
        }

        /// <summary>
        /// Fetches a list of movies from the TMDB API and resolves poster image paths.
        /// </summary>
        /// <param name="apiUrl">The full TMDB API URL to request movies from.</param>
        /// <returns>A <see cref="MovieListResponse"/> containing the list of movies with resolved poster paths.</returns>
        /// <exception cref="HttpIOException">Thrown when the API returns an invalid or null response.</exception>
        private async Task<MovieListResponse> GetMoviesAsync(string apiUrl)
        {
            MovieListResponse response = await _http.GetFromJsonAsync<MovieListResponse>(apiUrl)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to load movies");

            foreach (var movie in response.Results)
            {
                movie.PosterPath = string.IsNullOrEmpty(movie.PosterPath)
                    ? "/images/Poster.png"
                    : $"{ImageBaseUrl}{movie.PosterPath}";
            }

            return response;
        }

        /// <summary>
        /// Retrieves a list of movies currently playing in cinemas in Great Britain.
        /// </summary>
        /// <returns>A <see cref="MovieListResponse"/> containing now playing movies.</returns>
        public async Task<MovieListResponse> GetNowPlayingMoviesAsync()
        {
            return await GetMoviesAsync("movie/now_playing?region=GB&languages=en");
        }

        /// <summary>
        /// Retrieves a list of currently popular movies in Great Britain.
        /// </summary>
        /// <returns>A <see cref="MovieListResponse"/> containing popular movies.</returns>
        public async Task<MovieListResponse> GetPopularMoviesAsync()
        {
            return await GetMoviesAsync("movie/popular?region=GB&languages=en");
        }

        /// <summary>
        /// Searches for movies on TMDB matching the specified query string.
        /// </summary>
        /// <param name="query">The search term to query against the TMDB movie database.</param>
        /// <returns>A <see cref="MovieListResponse"/> containing movies matching the search query.</returns>
        /// <exception cref="HttpIOException">Thrown when the API returns an invalid or null response.</exception>
        public async Task<MovieListResponse> SearchMoviesAsync(string query)
        {
            string apiUrl = $"search/movie?query={Uri.EscapeDataString(query)}&region=GB&languages=en";

            MovieListResponse response = await _http.GetFromJsonAsync<MovieListResponse>(apiUrl)
                 ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to load search results");

            foreach (var movie in response.Results)
            {
                movie.PosterPath = string.IsNullOrEmpty(movie.PosterPath)
                    ? "/images/poster.png"
                    : $"{ImageBaseUrl}{movie.PosterPath}";
            }

            return response;
        }

        /// <summary>
        /// Retrieves the full details of a specific movie by its TMDB ID.
        /// </summary>
        /// <param name="movieId">The TMDB movie ID.</param>
        /// <returns>A <see cref="CineScope.Models.MovieDetails"/> containing the movie details.</returns>
        /// <exception cref="HttpIOException">Thrown when the API returns an invalid or null response.</exception>
        public async Task<CineScope.Models.MovieDetails> GetMovieDetailsAsync(int movieId)
        {
            string apiUrl = $"movie/{movieId}";

            CineScope.Models.MovieDetails response = await _http.GetFromJsonAsync<CineScope.Models.MovieDetails>(apiUrl)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to load movie details");

            if (!string.IsNullOrEmpty(response.PosterPath))
            {
                response.PosterPath = $"{ImageBaseUrl}{response.PosterPath}";
            }

            if (!string.IsNullOrEmpty(response.BackdropPath))
            {
                response.BackdropPath = $"{ImageBaseUrl}{response.BackdropPath}";
            }

            return response;
        }

        /// <summary>
        /// Retrieves the first YouTube trailer video for a specific movie by its TMDB ID.
        /// </summary>
        /// <param name="movieId">The TMDB movie ID.</param>
        /// <returns>A <see cref="Video"/> representing the YouTube trailer, or <see langword="null"/> if no trailer is found.</returns>
        /// <exception cref="HttpIOException">Thrown when the API returns an invalid or null response.</exception>
        public async Task<Video?> GetMovieTrailersAsync(int movieId)
        {
            string apiUrl = $"movie/{movieId}/videos?region=GB&languages=en";
            var videos = await _http.GetFromJsonAsync<MovieTrailerResponse>(apiUrl)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to load movie trailers");

            Video? movieTrailer = videos.Results.FirstOrDefault(v => v.Site!.Contains("YouTube", StringComparison.OrdinalIgnoreCase)
                                                                && v.Type!.Contains("Trailer", StringComparison.OrdinalIgnoreCase));

            return movieTrailer;
        }

        public async Task<CreditsResponse> GetMovieCreditsAsync(int movieId)
        {
            string apiUrl = $"movie/{movieId}/credits?region=GB&languages=en";
            var credits = await _http.GetFromJsonAsync<CreditsResponse>(apiUrl)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to load movie credits");

            foreach (var actor in credits.Cast)
            {
                actor.ProfilePath = string.IsNullOrEmpty(actor.ProfilePath)
                    ? "/images/Profile.jpg"
                    : $"{ImageBaseUrl}{actor.ProfilePath}";
            }

            foreach (var crew in credits.Crew)
            {
                crew.ProfilePath = string.IsNullOrEmpty(crew.ProfilePath)
                    ? "/images/Profile.jpg"
                    : $"{ImageBaseUrl}{crew.ProfilePath}";
            }

            return credits;
        }
    }
}