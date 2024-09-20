using System;

namespace OnlineLibraryManagement
{
    public enum Status
    {
        Default,
        Borrowed,
        Returned
    }

    public class Borrow
    {
        private static int s_borrowID = 2000;
        public string BorrowID { get; set; }
        public string BookID { get; set; }
        public string UserID { get; set; }
        public DateTime BorrowedDate { get; set; }
        public int BorrowBookCount { get; set; }
        public Status Status { get; set; }
        public double PaidFineAmount { get; set; }

        public Borrow(string bookID, string userID, DateTime borrowedDate, int borrowBookCount, Status status, double paidFineAmount)
        {
            BorrowID = $"LB{s_borrowID++}";
            BookID = bookID;
            UserID = userID;
            BorrowedDate = borrowedDate;
            BorrowBookCount = borrowBookCount;
            Status = status;
            PaidFineAmount = paidFineAmount;
        }
    }
}