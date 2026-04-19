using Microsoft.JSInterop;
using System.Text.Json;
using CineScope.Models;

namespace CineScope.Services
{
    public class FavouritesService(IJSRuntime jsRuntime)
    {
        private readonly string _localStorageKey = "favouriteMovies";

        public event Action? OnFavouritesChanged;

        /// <summary>
        /// Retrieves the list of favourite movies from local storage.
        /// Returns an empty list if no favourites are found or an error occurs.
        /// </summary>
        /// <returns>A list of favourite <see cref="Movie"/> objects.</returns>
        public async Task<List<Movie>> GetFavouritesAsync()
        {
            List<Movie> favourites = [];
            try
            {
                var json = await jsRuntime.InvokeAsync<string>("localStorage.getItem", _localStorageKey);
                favourites = JsonSerializer.Deserialize<List<Movie>>(json) ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving favourites: {ex.Message}");
            }
            return favourites;
        }

        /// <summary>
        /// Serializes and saves the provided list of movies to local storage.
        /// </summary>
        /// <param name="movies">The list of <see cref="Movie"/> objects to save.</param>
        public async Task SaveFavouritesAsync(List<Movie> movies)
        {
            try
            {
                var json = JsonSerializer.Serialize(movies);
                await jsRuntime.InvokeVoidAsync("localStorage.setItem", _localStorageKey, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving favourites: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a movie to the favourites list if it is not already present,
        /// saves the updated list to local storage, and notifies subscribers of the change.
        /// </summary>
        /// <param name="movie">The <see cref="Movie"/> to add to favourites.</param>
        public async Task AddToFavouritesAsync(Movie movie)
        {
            var favourites = await GetFavouritesAsync();
            if (favourites.Any(m => m.Id == movie.Id)) return;
            favourites.Add(movie);
            await SaveFavouritesAsync(favourites);
            OnFavouritesChanged?.Invoke();
        }

        /// <summary>
        /// Removes a movie from the favourites list based on its ID. It retrieves the current list of favourites, finds the movie to remove, and updates the list in local storage.
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task RemoveFromFavouritesAsync(Movie movie)
        {
            var favourites = await GetFavouritesAsync();
            favourites = favourites.Where(f => f.Id != movie.Id).ToList();
            await SaveFavouritesAsync(favourites);
            OnFavouritesChanged?.Invoke();
        }


        /// <summary>
        /// Returns a boolean indicating whether a specific movie is in the favourites list. It retrieves the current list of favourites and checks if any movie in the list matches the provided movie ID.
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public async Task<bool> IsFavouriteAsync(int movieId)
        {
            var favourites = await GetFavouritesAsync();
            bool isFavourite = favourites.Any(m => m.Id == movieId);
            return isFavourite;
        }
    }
}