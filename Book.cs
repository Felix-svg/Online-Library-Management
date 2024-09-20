namespace OnlineLibraryManagement
{
    public class Book
    {
        private static int s_bookID = 1000;
        public string BookID { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public int BookCount { get; set; }

        public Book(string bookName, string authorName, int bookCount)
        {
            BookID = $"BID{s_bookID++}";
            BookName = bookName;
            AuthorName = authorName;
            BookCount = bookCount;
        }
    }
}