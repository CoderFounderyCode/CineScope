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
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {tmdbApiKey}");
            }
        }

        /// <summary>
        /// Fetches a list of movies from the TMDB API and resolves poster image paths.
        /// </summary>
        /// <param name="apiUrl">The full TMDB API URL to request movies from.</param>
        /// <returns>A <see cref="MovieListResponse"/> containing the list of movies with resolved poster paths.</returns>
        /// <exception cref="HttpIOException">Thrown when the API returns an invalid or null response.</exception>
        private async Task<MovieListResponse> GetMovies(string apiUrl)
        {
            MovieListResponse response = await _http.GetFromJsonAsync<MovieListResponse>(apiUrl)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to load movies");

            foreach (var movie in response.Results)
            {
                movie.PosterPath = string.IsNullOrEmpty(movie.PosterPath)
                    ? "/images/poster.png"
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
            return await GetMovies("https://api.themoviedb.org/3/movie/now_playing?region=GB&languages=en");
        }

        /// <summary>
        /// Retrieves a list of currently popular movies in Great Britain.
        /// </summary>
        /// <returns>A <see cref="MovieListResponse"/> containing popular movies.</returns>
        public async Task<MovieListResponse> GetPopularMoviesAsync()
        {
            return await GetMovies("https://api.themoviedb.org/3/movie/popular?region=GB&languages=en");
        }

        /// <summary>
        /// Searches for movies on TMDB matching the specified query string.
        /// </summary>
        /// <param name="query">The search term to query against the TMDB movie database.</param>
        /// <returns>A <see cref="MovieListResponse"/> containing movies matching the search query.</returns>
        /// <exception cref="HttpIOException">Thrown when the API returns an invalid or null response.</exception>
        public async Task<MovieListResponse> SearchMoviesAsync(string query)
        {
            string apiUrl = $"https://api.themoviedb.org/3/search/movie?query={query}&region=GB&languages=en";
            string imageBaseUrl = "https://image.tmdb.org/t/p/w500";

            MovieListResponse response = await _http.GetFromJsonAsync<MovieListResponse>(apiUrl)
                 ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to load search results");

            foreach (var movie in response.Results)
            {
                if (string.IsNullOrEmpty(movie.PosterPath))
                {
                    movie.PosterPath = "/images/poster.png";
                }
                else
                {
                    movie.PosterPath = $"{imageBaseUrl}{movie.PosterPath}";
                }
            }
            return response;
        }
    }
}