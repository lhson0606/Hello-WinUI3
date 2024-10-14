using _21120127_Week04.Db;
using _21120127_Week04.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21120127_Week04.DAO
{
    public class BookDAO
    {
        private static readonly IBookDb _db = new MockBookDb();

        public static async Task<Book> SaveBook(Book book)
        {
            return await _db.SaveBook(book);
        }

        public static async Task<Book> GetBook()
        {
            return await _db.GetBookAsync();
        }

        public static async Task Initialize()
        {
            await _db.Initialize();
        }
    }
}
