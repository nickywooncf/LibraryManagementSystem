using LibraryManagement.Models;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryOperationsService _libraryService;

        public LibraryController(ILibraryOperationsService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowRequestDto request)
        {
            var success = await _libraryService.BorrowBookAsync(request.BookId, request.MemberId);
            if (!success) return BadRequest("Unable to locate this book!!");

            return Ok("Book borrowed successfully.");
        }
    }
}
