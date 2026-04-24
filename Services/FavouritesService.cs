using Microsoft.JSInterop;
using System.Text.Json;
using CineScope.Models;

namespace CineScope.Services
{
    /// <summary>
    /// Manages a user's favourite movies, persisting them to and retrieving them
    /// from browser local storage via JavaScript interop.
    /// </summary>
    public class FavouritesService(IJSRuntime jsRuntime)
    {
        private const string LocalStorageKey = "favouriteMovies";

        /// <summary>Raised whenever the favourites list is added to or removed from.</summary>
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
                var json = await jsRuntime.InvokeAsync<string>("localStorage.getItem", LocalStorageKey);
                favourites = JsonSerializer.Deserialize<List<Movie>>(json) ?? [];
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving favourites: {ex.Message}");
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
                await jsRuntime.InvokeVoidAsync("localStorage.setItem", LocalStorageKey, json);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving favourites: {ex.Message}");
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
        /// Removes a movie from the favourites list by its ID,
        /// saves the updated list to local storage, and notifies subscribers of the change.
        /// </summary>
        /// <param name="movie">The <see cref="Movie"/> to remove from favourites.</param>
        public async Task RemoveFromFavouritesAsync(Movie movie)
        {
            var favourites = await GetFavouritesAsync();
            favourites = favourites.Where(f => f.Id != movie.Id).ToList();
            await SaveFavouritesAsync(favourites);
            OnFavouritesChanged?.Invoke();
        }

        /// <summary>
        /// Checks whether a specific movie is in the user's favourites list.
        /// </summary>
        /// <param name="movieId">The TMDB ID of the movie to check.</param>
        /// <returns><see langword="true"/> if the movie is a favourite; otherwise <see langword="false"/>.</returns>
        public async Task<bool> IsFavouriteAsync(int movieId)
        {
            var favourites = await GetFavouritesAsync();
            return favourites.Any(m => m.Id == movieId);
        }
    }
}