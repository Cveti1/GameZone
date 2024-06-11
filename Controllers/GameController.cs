using GameZone.Data;
using GameZone.Models.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using GameZone.Data.Models;
using GameZone.Models.Genre;

namespace GameZone.Controllers
{
    [Authorize]
    public class GameController : Controller
    {

        private readonly GameZoneDbContext data;

        public GameController(GameZoneDbContext _data)
        {
            data = _data;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var game = await data
                .Games
                .Select(e => new GameViewShortModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    ImageUrl = e.ImageUrl,
                    ReleasedOn = e.ReleasedOn.ToString("yyyy-MM-dd"),
                    Genre = e.Genre.Name,
                    Publisher = e.Publisher.UserName
                })
                .ToListAsync();

            return View(game);
        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            GameFormModel game = new GameFormModel()
            {
                Genres = GetGenre()
            };

            return View(game);
        }


        [HttpPost]
        public async Task<IActionResult> Add(GameFormModel game)
        {

            if (!ModelState.IsValid)
            {
                return View(game);
            }


            string currentUserId = GetUserId();

            var gameToAdd = new Game()
            {
                Title = game.Title,
                Description = game.Description,
                ImageUrl = game.ImageUrl,
                ReleasedOn = game.ReleasedOn,
                GenreId = game.GenreId,
                PublisherId = currentUserId
                
            };

            await data.Games.AddAsync(gameToAdd);
            await data.SaveChangesAsync();

            return RedirectToAction("All", "Game");
           
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var gameForEdit = await data.Games.FindAsync(id);

            if (gameForEdit == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            if (currentUserId != gameForEdit.PublisherId)
            {
                return Unauthorized();
            }

            GameFormModel newGame = new GameFormModel()
            {
                Title = gameForEdit.Title,
                Description = gameForEdit.Description,
                ImageUrl = gameForEdit.ImageUrl,
                ReleasedOn = gameForEdit.ReleasedOn,
                GenreId = gameForEdit.GenreId,
                Genres = GetGenre()
            };
           
            return View(newGame);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, GameFormModel game)
        {
            var gameToEdit = await data.Games.FindAsync(id);

            if (gameToEdit == null)
            {
                return BadRequest();
            }

            string currentUser = GetUserId();

            if (currentUser != gameToEdit.PublisherId)
            {
                return Unauthorized();
            }


            gameToEdit.Title = game.Title;
            gameToEdit.Description = game.Description;
            gameToEdit.ImageUrl = game.ImageUrl;
            gameToEdit.ReleasedOn = game.ReleasedOn;
            gameToEdit.GenreId = game.GenreId;


            await data.SaveChangesAsync();
            return RedirectToAction("All", "Game");
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var game = await data
                .Games
                .Where(e => e.Id == id)
                .Select(e => new GameViewDetailsModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    ImageUrl = e.ImageUrl,
                    ReleasedOn = e.ReleasedOn.ToString("yyyy-MM-dd"),
                    Genre = e.Genre.Name,
                    Publisher = e.Publisher.UserName

                })
                .FirstOrDefaultAsync();

            if (game == null)
            {
                return BadRequest();
            }

            return View(game);
        }

        public async Task<IActionResult> AddToMyZone(int id)
        {
            var gameToJoin = await data
                .Games
                .FindAsync(id);

            if (gameToJoin == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            var entry = new GamerGame()
            {
                GameId = gameToJoin.Id,
                GamerId = currentUserId,
            };

            if (await data.GamersGames.ContainsAsync(entry))
            {
                return RedirectToAction("All", "Game");
            }

            await data.GamersGames.AddAsync(entry);
            await data.SaveChangesAsync();

            return RedirectToAction("MyZone", "Game");
        }



        public async Task<IActionResult> MyZone()
        {
            string currentUserId = GetUserId();

            var userGames = await data
                .GamersGames
                .Where(s => s.GamerId == currentUserId)
                .Select(s => new GameJoinedModel()
                {
                    Id = s.GameId,
                    Title = s.Game.Title,
                    ImageUrl = s.Game.ImageUrl,
                    ReleasedOn = s.Game.ReleasedOn.ToString("yyyy-MM-dd"),
                    Genre = s.Game.Genre.Name,
                    Publisher = s.Game.Publisher.UserName,
                })
                .ToListAsync();

            return View(userGames);
        }



        public async Task<IActionResult> StrikeOut(int id)
        {
            var gameId = id;
            var currentUser = GetUserId();

            var gameToLeave = data.Games.FindAsync(gameId);

            if (gameToLeave == null)
            {
                return BadRequest();
            }

            var entry =  data.GamersGames
                .FirstOrDefault(ep => ep.GamerId == currentUser && ep.GameId == gameId);
            data.GamersGames.Remove(entry);
            await data.SaveChangesAsync();

            return RedirectToAction("MyZone", "Game");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();


            var game = await data
                .Games
                .FindAsync(id);



            if (game == null || game.PublisherId != userId)
            {
                return RedirectToAction("All", "Game");
            }

            var newGame = new GameDeleteViewModel()
            {
                Id = game.Id,
                Title = game.Title
               
            };

            return View(newGame);
        }

        [HttpPost]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetUserId();

           var game = await data
                .Games
                .FindAsync(id);

            if (game == null || game.PublisherId != userId)
            {
                return RedirectToAction("All", "Game");
            }

            data.Games.Remove(game);
            await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }


        



    //Helpers
    private IEnumerable<GenreViewModel> GetGenre()
            => data
                .Genres
                .Select(c => new GenreViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                });



        private string GetUserId()
            => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
