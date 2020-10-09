using CourseLibrary.API.Services;
using CourseLibrary.API.Entities;
using DemoAppAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAppAPI.Helpers;
using AutoMapper;

namespace DemoAppAPI.Controllers
{
    //decoration
    [ApiController]
    [Route("api/authors")]
    //[Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        // [HttpGet("api/authors")]
        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorDTO>> GetAuthors([FromQuery]string mainCategory, string searchCategory)
        {
            var authorsFromRepo = _courseLibraryRepository.GetAuthors();

            if (authorsFromRepo == null)
            {
                return NotFound();
            }
           
            // return new JsonResult(authorsFromRepo);
           // return Ok(authorsFromRepo);
            return Ok(_mapper.Map<IEnumerable<AuthorDTO>>(authorsFromRepo));
        }

        [HttpGet("{authorId}")]
        public IActionResult GetAuthor(Guid authorId)
        {
            //if (!_courseLibraryRepository.AuthorExists(authorId))
            //{
            //    return NotFound();
            //}

           
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }
            // return new JsonResult(authorFromRepo);
            return Ok(_mapper.Map<AuthorDTO>(authorFromRepo));
        }
        [HttpPost]
        public ActionResult<AuthorDTO> CreateAuthor(AuthorCreateDto author)
        {
            var authorEntity = _mapper.Map<CourseLibrary.API.Entities.Author>(author);
            _courseLibraryRepository.AddAuthor(authorEntity);
            _courseLibraryRepository.Save();
            var authorToReturn = _mapper.Map<AuthorDTO>(authorEntity);
            return CreatedAtRoute("GetAuthor", new { authorId = authorToReturn.Id }, authorToReturn);
        }

    }
}
