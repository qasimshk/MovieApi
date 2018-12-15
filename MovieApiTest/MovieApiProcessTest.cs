using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieApi.Models;
using Moq;
using System.Linq;
using FluentAssertions;
using System.Collections.Generic;
using MovieApi.Repository;
using MovieApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MovieApiTest
{
    [TestClass]
    public class MovieApiProcessTest
    {
        private readonly MovieController _movieController;
        private List<Rating> _rating { get; set; }
        private AddUserRatingDto AddUserRatingDto { get; set; }

        public MovieApiProcessTest()
        {
            var repository = new Mock<IRepository>();
            repository.Setup(x => x.GetMovieDetails()).Returns(GetAllMovies());
            repository.Setup(x => x.GetUsers()).Returns(GetAllUser());
            repository.Setup(x => x.GetMovieRatings()).Returns(GetAllRating());

            // Add or update user rating
            repository.Setup(mr => mr.Save(It.IsAny<AddUserRatingDto>())).Returns(
                (AddUserRatingDto target) =>
                {
                    return SaveData(target);
                });

            // Injecting dependencies
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            _movieController = new MovieController(repository.Object);
            _movieController.ObjectValidator = objectValidator.Object;                        
        }

        #region Repository Data 

        // User
        public List<User> GetAllUser()
        {
            return new List<User> {
                new User {
                    UserId = Guid.Parse("dffc62b8-3d28-4043-b159-062797a5ad65"),
                    FullName = "Person A"
                },
                new User {
                    UserId = Guid.Parse("f8d35fe9-e8be-409e-955d-c89d41552c96"),
                    FullName = "Person B"
                },
                new User {
                    UserId = Guid.Parse("4b331735-e980-4d00-a602-23fdd49acbc9"),
                    FullName = "Person C"
                },
                new User {
                    UserId = Guid.Parse("5d03df01-272b-4b54-b9cc-f4760530643a"),
                    FullName = "Person D"
                }
            };
        }

        // Movies
        public List<MovieDetail> GetAllMovies()
        {
            return new List<MovieDetail> {
                new MovieDetail {
                    MovieId = 1,
                    Title = "Inception",
                    Year = 2010,
                    Genres = "Sci-Fic,Thriller"
                },
                new MovieDetail {
                    MovieId = 2,
                    Title = "The Dark Knight",
                    Year = 2008,
                    Genres = "Drama"
                },
                new MovieDetail {
                    MovieId = 3,
                    Title = "Avatar",
                    Year = 2009,
                    Genres = "Fantasy,Sci-Fic"
                },
                new MovieDetail {
                    MovieId = 4,
                    Title = "The Avengers",
                    Year = 2012,
                    Genres = "Fantasy,Sci-Fic"
                },
                new MovieDetail {
                    MovieId = 5,
                    Title = "Titanic",
                    Year = 1997,
                    Genres = "Drama,Disaster"
                },
                new MovieDetail {
                    MovieId = 6,
                    Title = "The Lord of the Rings",
                    Year = 2001,
                    Genres = "Fantasy,Action"
                },
                new MovieDetail {
                    MovieId = 7,
                    Title = "Forrest Gump",
                    Year = 1994,
                    Genres = "Drama,Comedy"
                },
                new MovieDetail {
                    MovieId = 8,
                    Title = "Terminator 2: Judgment Day",
                    Year = 1991,
                    Genres = "Action,Mystery"
                },
                new MovieDetail {
                    MovieId = 9,
                    Title = "The Matrix",
                    Year = 1999,
                    Genres = "Fantasy,Sci-Fic"
                },
                new MovieDetail {
                    MovieId = 10,
                    Title = "Back to the Future",
                    Year = 1985,
                    Genres = "Fantasy,Sci-Fic"
                },
            };
        }

        // Rating
        public List<Rating> GetAllRating()
        {
            return new List<Rating> {

                // A
                new Rating {
                     Id = 1,
                     MovieId = 1,
                     MovieRating = 2,
                     UserId = Guid.Parse("dffc62b8-3d28-4043-b159-062797a5ad65")
                },
                new Rating {
                     Id = 2,
                     MovieId = 3,
                     MovieRating = 5,
                     UserId = Guid.Parse("dffc62b8-3d28-4043-b159-062797a5ad65")
                },
                new Rating {
                     Id = 3,
                     MovieId = 5,
                     MovieRating = 3,
                     UserId = Guid.Parse("dffc62b8-3d28-4043-b159-062797a5ad65")
                },
                new Rating {
                     Id = 4,
                     MovieId = 7,
                     MovieRating = 2,
                     UserId = Guid.Parse("dffc62b8-3d28-4043-b159-062797a5ad65")
                },
                new Rating {
                     Id = 5,
                     MovieId = 9,
                     MovieRating = 1,
                     UserId = Guid.Parse("dffc62b8-3d28-4043-b159-062797a5ad65")
                },

                // B
                new Rating {
                     Id = 6,
                     MovieId = 2,
                     MovieRating = 5,
                     UserId = Guid.Parse("f8d35fe9-e8be-409e-955d-c89d41552c96")
                },
                new Rating {
                     Id = 7,
                     MovieId = 4,
                     MovieRating = 4,
                     UserId = Guid.Parse("f8d35fe9-e8be-409e-955d-c89d41552c96")
                },
                new Rating {
                     Id = 8,
                     MovieId = 6,
                     MovieRating = 5,
                     UserId = Guid.Parse("f8d35fe9-e8be-409e-955d-c89d41552c96")
                },
                new Rating {
                     Id = 9,
                     MovieId = 8,
                     MovieRating = 3,
                     UserId = Guid.Parse("f8d35fe9-e8be-409e-955d-c89d41552c96")
                },
                new Rating {
                     Id = 10,
                     MovieId = 10,
                     MovieRating = 2,
                     UserId = Guid.Parse("f8d35fe9-e8be-409e-955d-c89d41552c96")
                },

                // C
                new Rating {
                     Id = 11,
                     MovieId = 1,
                     MovieRating = 4,
                     UserId = Guid.Parse("4b331735-e980-4d00-a602-23fdd49acbc9")
                },
                new Rating {
                     Id = 12,
                     MovieId = 3,
                     MovieRating = 1,
                     UserId = Guid.Parse("4b331735-e980-4d00-a602-23fdd49acbc9")
                },
                new Rating {
                     Id = 13,
                     MovieId = 5,
                     MovieRating = 2,
                     UserId = Guid.Parse("4b331735-e980-4d00-a602-23fdd49acbc9")
                },
                new Rating {
                     Id = 14,
                     MovieId = 7,
                     MovieRating = 1,
                     UserId = Guid.Parse("4b331735-e980-4d00-a602-23fdd49acbc9")
                },
                new Rating {
                     Id = 15,
                     MovieId = 9,
                     MovieRating = 2,
                     UserId = Guid.Parse("4b331735-e980-4d00-a602-23fdd49acbc9")
                },

                // D
                new Rating {
                     Id = 16,
                     MovieId = 2,
                     MovieRating = 2,
                     UserId = Guid.Parse("5d03df01-272b-4b54-b9cc-f4760530643a")
                },
                new Rating {
                     Id = 17,
                     MovieId = 4,
                     MovieRating = 5,
                     UserId = Guid.Parse("5d03df01-272b-4b54-b9cc-f4760530643a")
                },
                new Rating {
                     Id = 18,
                     MovieId = 6,
                     MovieRating = 3,
                     UserId = Guid.Parse("5d03df01-272b-4b54-b9cc-f4760530643a")
                },
                new Rating {
                     Id = 19,
                     MovieId = 8,
                     MovieRating = 1,
                     UserId = Guid.Parse("5d03df01-272b-4b54-b9cc-f4760530643a")
                },
                new Rating {
                     Id = 20,
                     MovieId = 10,
                     MovieRating = 4,
                     UserId = Guid.Parse("5d03df01-272b-4b54-b9cc-f4760530643a")
                },
            };
        }

        // Add or update user rating
        public bool SaveData(AddUserRatingDto rating)
        {
            _rating = GetAllRating();
            var record = _rating.Where(r => r.UserId == rating.userId && r.MovieId == rating.MovieId)
                .ToList();

            if (record.Count() == 1)
                _rating.Remove(record.FirstOrDefault());

            _rating.Add(new Rating
            {
                Id = (record.Count() == 0) ? 21 : record.FirstOrDefault().Id,
                MovieId = rating.MovieId,
                UserId = rating.userId,
                MovieRating = rating.Rating
            });
            return true;
        }

        #endregion

        #region Round of average value function

        [TestMethod]
        [Description("Test calculate average Function")]
        [TestCategory("Function Testing")]
        public void RoundOFAverageValue_TestValues()
        {
            var resp = new List<double>();
            resp.Add(_movieController.RoundOfAverage(2.91));
            resp.Add(_movieController.RoundOfAverage(3.249));
            resp.Add(_movieController.RoundOfAverage(3.25));
            resp.Add(_movieController.RoundOfAverage(3.6));
            resp.Add(_movieController.RoundOfAverage(3.75));

            Assert.IsNotNull(resp);
            Assert.AreEqual(3.0, resp[0]);
            Assert.AreEqual(3.0, resp[1]);
            Assert.AreEqual(3.5, resp[2]);
            Assert.AreEqual(3.5, resp[3]);
            Assert.AreEqual(4.0, resp[4]);
        }

        #endregion

        #region API A

        [TestMethod]
        [Description("Testing api filtering by year")]
        [TestCategory("API A")]
        public void ServiceA_FilterByYear()
        {
            var resp = _movieController.FilterMovie(new FilterMovie
            {
                Year = 1985
            }) as OkObjectResult;

            Assert.IsNotNull(resp);

            Assert.IsInstanceOfType(resp, typeof(OkObjectResult));

            Assert.AreEqual("Back to the Future", resp.Value.As<List<MovieDetail>>()
                .FirstOrDefault().Title);
        }

        [TestMethod]
        [Description("Testing api filtering by Genre and Year")]
        [TestCategory("API A")]
        public void ServiceA_FilterByTitle()
        {
            var resp = _movieController.FilterMovie(new FilterMovie
            {
                Genres = "Drama",
                Year = 1994
            }) as OkObjectResult;

            Assert.IsNotNull(resp);

            Assert.IsInstanceOfType(resp, typeof(OkObjectResult));

            Assert.AreEqual(3, resp.Value.As<List<MovieDetail>>().Count());
        }

        [TestMethod]
        [Description("Testing api with no filter provided")]
        [TestCategory("API A")]
        public void ServiceA_NoFilter()
        {
            var resp = _movieController.FilterMovie(default(FilterMovie));
            Assert.IsInstanceOfType(resp, typeof(BadRequestResult));
        }

        [TestMethod]
        [Description("Testing api filters with data which is not available")]
        [TestCategory("API A")]
        public void ServiceA_NoMovieFound()
        {
            var resp = _movieController.FilterMovie(new FilterMovie
            {
                Year = 2018,
                Genres = "Horror"
            });

            Assert.IsInstanceOfType(resp, typeof(NotFoundResult));
        }

        #endregion

        #region API B

        [TestMethod]
        [Description("testing top five movie rated by viewers")]
        [TestCategory("API B")]
        public void ServiceB_TestTop5Movies()
        {
            var resp = _movieController.TopFiveMovies()
                 as OkObjectResult;

            Assert.IsNotNull(resp);

            Assert.IsInstanceOfType(resp, typeof(OkObjectResult));

            Assert.AreEqual(5, resp.Value.As<List<MovieRatedDto>>().Count());
        }

        #endregion

        #region API C 

        [TestMethod]
        [Description("testing top 5 movies based on the highest ratings given by a specific user")]
        [TestCategory("API C")]
        public void ServiceC_TestTop5MoviesByUser()
        {
            var resp = _movieController
                .TopFiveMoviesByUser("dffc62b8-3d28-4043-b159-062797a5ad65")
                 as OkObjectResult;

            Assert.IsNotNull(resp);

            Assert.IsInstanceOfType(resp, typeof(OkObjectResult));

            Assert.AreEqual(5, resp.Value.As<List<MovieRatedByUserDto>>().Count());
        }

        [TestMethod]
        [Description("testing API with an invalid user that is not registered")]
        [TestCategory("API C")]
        public void ServiceC_TestTop5MoviesByUser_NOTFOUND()
        {
            var resp = _movieController
                .TopFiveMoviesByUser("dffc62b8-3d28-4043-b157-062797b5ad99");

            Assert.IsInstanceOfType(resp, typeof(NotFoundResult));
        }

        [TestMethod]
        [Description("testing API invalid GUID")]
        [TestCategory("API C")]
        public void ServiceC_TestTop5MoviesByUser_INVALID_GUID()
        {
            var resp = _movieController
                .TopFiveMoviesByUser("0000-576575765-688");

            Assert.IsInstanceOfType(resp, typeof(BadRequestResult));
        }

        #endregion

        #region API D

        [TestMethod]
        [Description("testing to api consumers to update a rating to a movie for a certain user")]
        [TestCategory("API D")]
        public void ServiceD_TestUpdateUserRating()
        {
            AddUserRatingDto = new AddUserRatingDto
            {
                MovieId = 1,
                userId = Guid.Parse("dffc62b8-3d28-4043-b159-062797a5ad65"),
                Rating = 4
            };

            var resp = _movieController.UserRating(AddUserRatingDto);

            Assert.IsInstanceOfType(resp, typeof(CreatedResult));

            // BEFORE
            var PreviousData = GetAllRating()
                .FirstOrDefault(x => x.UserId == AddUserRatingDto.userId &&
                x.MovieId == AddUserRatingDto.MovieId);

            Assert.AreEqual(2, PreviousData.MovieRating);

            // AFTER
            var RecentData = _rating
                .FirstOrDefault(x => x.MovieId == AddUserRatingDto.MovieId &&
                x.UserId == AddUserRatingDto.userId);

            Assert.AreEqual(4, RecentData.MovieRating);
        }

        [TestMethod]
        [Description("testing to api consumers to add a rating to a movie for a certain user")]
        [TestCategory("API D")]
        public void ServiceD_TestAddUserRating()
        {
            AddUserRatingDto = new AddUserRatingDto
            {
                MovieId = 3,
                userId = Guid.Parse("5d03df01-272b-4b54-b9cc-f4760530643a"),
                Rating = 5
            };

            var resp = _movieController.UserRating(AddUserRatingDto);

            Assert.IsInstanceOfType(resp, typeof(CreatedResult));

            // BEFORE
            var PreviousData = GetAllRating()
                .FirstOrDefault(x => x.UserId == AddUserRatingDto.userId &&
                x.MovieId == AddUserRatingDto.MovieId);

            Assert.IsNull(PreviousData);

            // AFTER
            var RecentData = _rating
                .FirstOrDefault(x => x.MovieId == AddUserRatingDto.MovieId &&
                x.UserId == AddUserRatingDto.userId);

            Assert.AreEqual(5, RecentData.MovieRating);
        }

        [TestMethod]
        [Description("testing to api validation - null request")]
        [TestCategory("API D")]
        public void ServiceD_TestValidation_NULLREQUEST()
        {            
            var resp = _movieController.UserRating(null);            
            Assert.IsInstanceOfType(resp, typeof(BadRequestResult));
        }

        [TestMethod]
        [Description("testing to api validation - wrong rate")]
        [TestCategory("API D")]
        public void ServiceD_TestValidation_WRONGRATE()
        {   
            var resp = _movieController.UserRating(new AddUserRatingDto
            {
                MovieId = 1,
                userId = Guid.Parse("dffc62b8-3d28-4043-b159-062797a5ad65"),
                Rating = 7
            });

            Assert.IsInstanceOfType(resp, typeof(UnprocessableEntityObjectResult));
        }
        
        [TestMethod]
        [Description("testing to api validation - unregistered user")]
        [TestCategory("API D")]
        public void ServiceD_TestValidation_UNRegisteredUser()
        {
            var resp = _movieController.UserRating(new AddUserRatingDto
            {
                MovieId = 1,
                userId = Guid.Parse("dffc62b8-3d28-4043-b159-062797a5ad34"), // wrong UserId
                Rating = 3
            });

            Assert.IsInstanceOfType(resp, typeof(NotFoundResult));
        }

        [TestMethod]
        [Description("testing to api validation - Wrong MovieID")]
        [TestCategory("API D")]
        public void ServiceD_TestValidation_WrongMovieID()
        {
            var resp = _movieController.UserRating(new AddUserRatingDto
            {
                MovieId = 12,  // wrong Id
                userId = Guid.Parse("dffc62b8-3d28-4043-b159-062797a5ad65"),
                Rating = 3
            });

            Assert.IsInstanceOfType(resp, typeof(NotFoundResult));
        }
        #endregion
    }
}
