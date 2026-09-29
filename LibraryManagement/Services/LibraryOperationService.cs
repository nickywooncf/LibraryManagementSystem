using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace LibraryManagement.Services
{
    public interface ILibraryOperationsService
    {
        Task<bool> BorrowBookAsync(int bookId, int memberId);
        Task<bool> ReturnBookAsync(int loanId);

        // Add this new method
        Task<bool> AddBookAsync(BookRequestDto request);
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
            try
            {

                var book = await _context.Books.FindAsync(bookId);
                if (book == null || !book.IsAvailable) return false;

                var loan = new Loan { BookId = bookId, MemberId = memberId };
                book.IsAvailable = false; // Update state

                _context.Loans.Add(loan);
                await _context.SaveChangesAsync();
              
            }
            catch (DbUpdateException dbEx)
            {
                await _context.Database.RollbackTransactionAsync();

                // Log the database update exception (dbEx) here if needed
                return false;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                return false;
            }

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

        public async Task<bool> AddBookAsync(BookRequestDto request)
        {
            // 1. Map the DTO to your actual Database Entity
            var newBook = new Book
            {
                Title = request.Title,
                Author = request.Author,
                ISBN = request.ISBN,
                IsAvailable = true // Defaults to true when a new book is added
            };

            // 2. Add to the In-Memory Database asynchronously
            await _context.Books.AddAsync(newBook);

            // 3. Save changes and return true if rows were affected
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }


    }

}
