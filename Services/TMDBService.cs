using CineScope.Models;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace CineScope.Services
{
    public class TMDBService
    {
        private readonly HttpClient _http;

        public TMDBService(HttpClient _http, IConfiguration config)
        {
            _http = _http;

            string tmdbApiKey = config["TmdbAccessKey"];
            if (!string.IsNullOrEmpty(tmdbApiKey))
            {
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {tmdbApiKey}");
            }
        }
    }

    public async Task<MovieListResponse> GetNowPlayingMovies()
        {
            string apiUrl = $"https://api.themoviedb.org/3/movie/now_playing?region=GB&languages=en";
            string imageBaseUrl = "https://image.tmdb.org/t/p/w500";


            MovieListResponse response = await _http.GetFromJsonAsync<MovieListResponse>(apiUrl)
                 ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to load now playing movies");


            // Results is the property that contains the list of movies in the response from the class MovieListResponse
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
