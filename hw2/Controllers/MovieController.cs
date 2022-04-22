using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Extensions;
using Microsoft.AspNetCore.Mvc;
using Models;
using Resources;
using Services.Interfaces;

namespace Controllers
{
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private readonly IMovieService movieService;
        private readonly IMapper mapper;

        public MovieController(IMovieService movieService, IMapper mapper)
        {
            this.movieService = movieService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<List<Movie>> GetAll()
        {
            return await movieService.GetAllAsync();
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] SaveMovieResource resource)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            
            var addedMovie = mapper.Map<SaveMovieResource, Movie>(resource);
            var result = await movieService.SaveAsync(addedMovie);

            if(!result.success)
                return BadRequest(result.message);
            
            var movieResource = mapper.Map<Movie, MovieResource>(result.movie);
            return Ok(movieResource);
        }
    }
}