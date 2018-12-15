using MovieApi.Models;
using System.Collections.Generic;

namespace MovieApi.Repository
{
    public interface IRepository
    {
        List<MovieDetail> GetMovieDetails();

        List<Rating> GetMovieRatings();

        List<User> GetUsers();

        bool Save(AddUserRatingDto rating);
    }
}