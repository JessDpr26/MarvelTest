using System;
using System.Linq;
using System.Threading.Tasks;
using MarvelTest.Data;
using MarvelTest.Models;

namespace MarvelTest.Services
{
    public class DatabaseSeeder
    {
        private readonly MarvelDBContext _context;
        private readonly MarvelService _marvelApiService;

        public DatabaseSeeder(MarvelDBContext context, MarvelService marvelApiService)
        {
            _context = context;
            _marvelApiService = marvelApiService;
        }

        public async Task SeedDatabaseAsync()
        {
            try
            {
                if (_context.Characters.Any() && _context.Comics.Any())
                {
                    Console.WriteLine("Las tablas ya tienen información.");
                    return;
                }


                if (_context.Characters.Any())
                {
                    _context.Characters.RemoveRange(_context.Characters);
                }

                if (_context.Comics.Any())
                {
                    _context.Comics.RemoveRange(_context.Comics);
                }
                await _context.SaveChangesAsync();

                var charactersResponse = await _marvelApiService.GetCharacters();
                var comicsResponse = await _marvelApiService.GetComics();

                foreach (var character in charactersResponse)
                {
                    var dbCharacter = new Characters
                    {
                        id = character.id,
                        name = character.name,
                        description = character.description
                    };
                    _context.Characters.Add(dbCharacter);
                }

                foreach (var comic in comicsResponse)
                {
                    var dbComic = new Comics
                    {
                        id = comic.id,
                        title = comic.title,
                        description = comic.description
                    };
                    _context.Comics.Add(dbComic);
                }
                await _context.SaveChangesAsync();

                Console.WriteLine("Proceso Completo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la base de datos: {ex.Message}");
            }
        }
    }
}
