using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public interface ILibraryOperationsService
    {
        Task<bool> BorrowBookAsync(int bookId, int memberId);
        Task<bool> ReturnBookAsync(int loanId);
    }

    public class LibraryOperationsService : ILibraryOperationsService
    {
        private readonly LibraryDbContext _context;

        public LibraryOperationsService(LibraryDbContext context)
        {
            _context = context; // Dependency Injection
        }

        public async Task<bool> BorrowBookAsync(int bookId, int memberId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null || !book.IsAvailable) return false;

            var loan = new Loan { BookId = bookId, MemberId = memberId };
            book.IsAvailable = false; // Update state

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReturnBookAsync(int loanId)
        {
            var loan = await _context.Loans.Include(l => l.Book).FirstOrDefaultAsync(l => l.Id == loanId);
            if (loan == null || loan.ReturnDate != null) return false;

            loan.ReturnDate = DateTime.UtcNow;
            if (loan.Book != null) loan.Book.IsAvailable = true;

            await _context.SaveChangesAsync();
            return true;
        }
    }

}
