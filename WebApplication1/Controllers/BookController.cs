using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private static List<Book> _books = new List<Book>
        {
            new Book { ID = 1, Name = "ASP.NET Core ���Ž̳�", Price = 59.9 },
            new Book { ID = 2, Name = "C# ���ָ��", Price = 69.5 },
            new Book { ID = 3, Name = "WebAPI ʵս", Price = 79.0 }
        };

        // GET: api/Book
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            return _books;
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id)
        {
            var book = _books.FirstOrDefault(b => b.ID == id);
            
            if (book == null)
            {
                return NotFound();
            }
            
            return book;
        }

        // POST: api/Book
        [HttpPost]
        public ActionResult<Book> PostBook(Book book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            // �Զ�����һ����ID
            if (_books.Count > 0)
            {
                book.ID = _books.Max(b => b.ID) + 1;
            }
            else
            {
                book.ID = 1;
            }

            _books.Add(book);
            
            return CreatedAtAction(nameof(GetBook), new { id = book.ID }, book);
        }

        // PUT: api/Book/5
        [HttpPut("{id}")]
        public IActionResult PutBook(int id, Book book)
        {
            if (id != book.ID)
            {
                return BadRequest();
            }

            var existingBook = _books.FirstOrDefault(b => b.ID == id);
            if (existingBook == null)
            {
                return NotFound();
            }

            // �����鼮��Ϣ
            existingBook.Name = book.Name;
            existingBook.Price = book.Price;
            
            return NoContent();
        }

        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _books.FirstOrDefault(b => b.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            _books.Remove(book);
            
            return NoContent();
        }
    }
}
