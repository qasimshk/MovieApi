using Microsoft.AspNetCore.Mvc;
using MovieApi.Models;
using MovieApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private readonly IRepository _repository;

        public MovieController(IRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Api A (see “Api Specification” section for more detail) should return the details of movies that pass certain filter criteria provided by the api consumers.
        /// </summary>
        /// <returns>retrun a list of movies that matches the filter</returns>
        /// <param name="search">API Filter must have atleast one search filter. Filters( YEAR, GENRE, TITLE )</param>
        [HttpGet("Filter")]
        public IActionResult FilterMovie(FilterMovie search)
        {
            //Validating filter parameter
            if (search == null)
                return BadRequest();

            if (string.IsNullOrEmpty(search.Title)
                && search.Year == 0
                && string.IsNullOrEmpty(search.Genres))
                return BadRequest();

            var resp = _repository.GetMovieDetails()
                .Where(x => search.Year != 0 && x.Year == search.Year ||
                !string.IsNullOrEmpty(search.Genres) && x.Genres.Contains(search.Genres) ||
                !string.IsNullOrEmpty(search.Title) && x.Title.Equals(search.Title))
                .OrderBy(y => y.Title)
                .ToList();

            if (resp.Count == 0)
                return NotFound();

            return Ok(resp);
        }

        /// <summary>
        /// Api B should return the details of the top 5 movies based on total user average ratings;
        /// </summary>
        /// <returns>return a list of top five movies rated by all the users</returns>
        [HttpGet("Top5")]
        public IActionResult TopFiveMovies()
        {
            var resp = _repository.GetMovieRatings()
                 .GroupBy(m => m.MovieId, r => r.MovieRating)
                 .Select(rGroup => new
                 {
                     mid = rGroup.Key,
                     rate = rGroup.Average()
                 })
                 .Join(_repository.GetMovieDetails(),
                       r => r.mid,
                       m => m.MovieId,
                     (r, m) => new MovieRatedDto
                     {
                         title = m.Title,
                         year = m.Year,
                         genre = m.Genres,
                         rating = RoundOfAverage(r.rate)
                     })
               .OrderByDescending(m => m.rating)
               .ThenBy(m => m.title)
               .Take(5)
               .ToList();

            if (resp.Count() == 0)
                return NotFound();

            return Ok(resp);
        }

        /// <summary>
        /// Api C should return the details of the top 5 movies based on the highest ratings given by a specific user,
        /// </summary>
        /// <returns>return top five movie detail list which is filtered by user</returns>
        /// <param name="UserID">User id of user</param>
        [HttpGet("Top5/{UserID}")]
        public IActionResult TopFiveMoviesByUser(string UserID)
        {
            // Validate GUID
            Guid UID;
            if (!Guid.TryParse(UserID, out UID))
                return BadRequest();

            var resp = _repository.GetMovieRatings()
                .Where(u => u.UserId == UID)
                .Join(_repository.GetUsers(),
                    r => r.UserId,
                    u => u.UserId,
                    (r, u) => new
                    {
                        movieId = r.MovieId,
                        fullname = u.FullName,
                        rating = r.MovieRating,
                    })
                .Join(_repository.GetMovieDetails(),
                    r => r.movieId,
                    m => m.MovieId,
                    (r, m) => new MovieRatedByUserDto
                    {
                        FullName = r.fullname,
                        genre = m.Genres,
                        rating = r.rating,
                        title = m.Title,
                        year = m.Year
                    })
                    .Take(5)
                    .OrderByDescending(x => x.rating)
                    .ThenBy(y => y.title)
                    .ToList();

            if (resp.Count() == 0)
                return NotFound();

            return Ok(resp);
        }

        /// <summary>
        /// api consumers to add a rating to a movie for a certain user.
        /// </summary>
        /// <returns>Created status with successful message response</returns>
        /// <param name="req">user can insert or update its rating on movie</param>
        [HttpPost("Rating")]
        public IActionResult UserRating([FromBody] AddUserRatingDto req)
        {
            // Check null request
            if (req == null)
                return BadRequest();

            // Validate the request
            if (!ModelState.IsValidate(req))
                return new UnprocessableEntityObjectResult(ModelState);

            // Check user exist
            if (_repository.GetUsers()
                .Where(u => u.UserId == req.userId)
                .ToList().Count() == 0)
                return NotFound();

            // Check movie exist
            if (_repository.GetMovieDetails()
                .Where(m => m.MovieId == req.MovieId)
                .ToList().Count() == 0)
                return NotFound();

            _repository.Save(req);
            return Created("", "successful");
        }

        public double RoundOfAverage(double value)
        {
            return Math.Round(value * 2, MidpointRounding.AwayFromZero) / 2;
        }
    }
}