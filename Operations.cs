using System;
using System.Collections.Generic;

namespace OnlineLibraryManagement
{
    public class Operations
    {
        private static List<User> users = [];
        private static List<Book> books = [];
        private static List<Borrow> borrows = [];
        private static User currentLoggedInUser;

        public static void MainMenu()
        {
            bool flag = true;

            do
            {
                Console.WriteLine("Welcome to Syncfusion Library\nPlease choose an option to continue\n");
                Console.WriteLine("1. User Registration\n2. User Login\n3. Exit");

                string userChoice = Console.ReadLine().Trim();
                switch (userChoice)
                {
                    case "1":
                        UserRegistration();
                        break;
                    case "2":
                        UserLogin();
                        break;
                    case "3":
                        flag = false;
                        Console.WriteLine("Goodbye");
                        Environment.Exit(1);
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            } while (flag);

        }

        public static void UserRegistration()
        {
            Console.WriteLine("Enter your name");
            string name = Console.ReadLine();
            Console.WriteLine("Enter your Gender (Male/Female/Transgender)");
            Gender gender = Enum.Parse<Gender>(Console.ReadLine(), true);
            Console.WriteLine("Enter your department (ECE/EEE/CSE)");
            Department department = Enum.Parse<Department>(Console.ReadLine(), true);
            Console.WriteLine("Enter your mobile number");
            string mobileNumber = Console.ReadLine();
            Console.WriteLine("Enter your mail ID");
            string mailID = Console.ReadLine();
            Console.WriteLine("Enter your wallet balance");
            decimal walletBalance = decimal.Parse(Console.ReadLine());

            User user = new(name, gender, department, mobileNumber, mailID, walletBalance);
            Console.WriteLine($"User registration successful, and User ID is {user.UserID}");
            users.Add(user);

            MainMenu();
        }

        public static void UserLogin()
        {
            Console.WriteLine("Enter user ID");
            string userID = Console.ReadLine().ToUpper().Trim();

            bool flag = true;
            foreach (User user in users)
            {
                if (userID == user.UserID)
                {
                    flag = false;
                    currentLoggedInUser = user;
                    SubMenu();
                    break;
                }
            }
            if (flag)
            {
                Console.WriteLine("Invalid User ID. Please enter a valid one");
                UserLogin();
            }
        }

        public static void SubMenu()
        {
            Console.WriteLine("1. Borrow Book\n2. Show Borrow History\n3. Return Books\n4. Wallet Recharge\n5. Exit");
            string userChoice = Console.ReadLine().Trim();

            switch (userChoice)
            {
                case "1":
                    BorrowBook();
                    break;
                case "2":
                    ShowBorrowHistory();
                    break;
                case "3":
                    ReturnBooks();
                    break;
                case "4":
                    WalletRecharge();
                    break;
                case "5":
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }

        public static void BorrowBook()
        {
            bool flag = true;
            foreach (Book book in books)
            {
                flag = false;
                Console.Write($"Book ID: {book.BookID} | Book Name: {book.BookName} | Author Name: {book.AuthorName} | Book Count: {book.BookCount}\n");
            }
            if (flag)
            {
                Console.WriteLine("No books found");
            }

            Console.WriteLine("Enter book ID to borrow");
            string bookID = Console.ReadLine().ToUpper().Trim();

            bool flag1 = true;
            foreach (Book book in books)
            {
                if (book.BookID == bookID)
                {
                    flag1 = false;
                    Console.WriteLine("Enter book count");
                    int bookCount = int.Parse(Console.ReadLine().Trim());

                    if (book.BookCount > 0)
                    {
                        int count = 0;
                        foreach (Borrow borrow in borrows)
                        {
                            if (borrow.UserID == currentLoggedInUser.UserID && borrow.Status == Status.Borrowed)
                            {
                                count++;
                            }
                        }

                        Console.WriteLine(count);
                        try
                        {
                            foreach (Borrow borrow1 in borrows)
                            {
                                if (count == 3)
                                {
                                    Console.WriteLine("You have borrowed 3 books already");
                                    BorrowBook();
                                    break;
                                }
                                else if (count > 3 && bookCount > 3)
                                {
                                    Console.WriteLine($"You can have maximum of 3 borrowed books. Your already borrowed books count is {borrow1.BorrowBookCount} and requested count is {bookCount}, which exceeds 3");
                                    BorrowBook();
                                    break;
                                }
                                else
                                {
                                    Borrow borrow = new(book.BookID, currentLoggedInUser.UserID, DateTime.Now, bookCount, Status.Borrowed, 0);
                                    book.BookCount -= bookCount;
                                    borrows.Add(borrow);
                                    Console.WriteLine("Book Borrowed Successfully");
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Books are not available for the selected count");

                        //print next available date of book
                        foreach (Borrow borrow in borrows)
                        {
                            if (borrow.BookID == book.BookID)
                            {
                                DateTime returnDate = borrow.BorrowedDate.AddDays(15);
                                Console.WriteLine($"The book will be available on {returnDate.ToString("dd/MM/yyyy")}");
                                break;
                            }
                        }
                        SubMenu();
                    }
                }
            }
            if (flag1)
            {
                Console.WriteLine("Invalid Book ID, please enter a valid ID");
                BorrowBook();
            }
            SubMenu();
        }

        public static void ShowBorrowHistory()
        {
            bool flag = true;
            foreach (Borrow borrow in borrows)
            {
                if (borrow.UserID == currentLoggedInUser.UserID)
                {
                    flag = false;
                    Console.Write($"ID: {borrow.BorrowID} | Book ID: {borrow.BookID} | User ID: {borrow.UserID} | Status: {borrow.Status} | Borrow  Date: {borrow.BorrowedDate.ToString("dd/MM/yyyy")} | Return Date: {borrow.BorrowedDate.AddDays(15).ToString("dd/MM/yyyy")} | Books borrowed: {borrow.BorrowBookCount}\n");
                }
            }
            if (flag)
            {
                Console.WriteLine("No borrow history found");
            }
            SubMenu();
        }

        public static void ReturnBooks()
        {
            bool flag = true;
            foreach (User user in users)
            {
                if (user.UserID == currentLoggedInUser.UserID)
                {
                    flag = false;

                    bool flag1 = true;
                    foreach (Borrow borrow in borrows)
                    {
                        if (borrow.UserID == user.UserID && borrow.Status == Status.Borrowed)
                        {
                            flag1 = false;
                            DateTime returnDate = borrow.BorrowedDate.AddDays(15);
                            Console.Write($"ID: {borrow.BorrowID} | Book Count: {borrow.BorrowBookCount} | Book ID: {borrow.BookID} | Return date: {returnDate.ToString("dd/MM/yyyy")}\n");

                            TimeSpan diff = DateTime.Now - returnDate;

                            Console.WriteLine("Enter Borrow ID to return book");
                            string borrowID = Console.ReadLine();

                            double fine = 0;
                            if (diff.TotalDays > 15)
                            {
                                fine = 1 * diff.TotalDays;
                                Console.WriteLine($"Fine amount is {string.Format("{0:C}", (decimal)fine)}");
                            }

                            if ((double)user.WalletBalance >= fine)
                            {
                                user.WalletBalance -= (decimal)borrow.PaidFineAmount;
                                borrow.Status = Status.Returned;
                                borrow.PaidFineAmount = fine;
                                Console.WriteLine("Book returned successfully");
                            }
                            else
                            {
                                Console.WriteLine("Insufficient balance. Please recharge to proceed");
                                WalletRecharge();
                            }
                        }
                    }
                    if (flag1)
                    {
                        Console.WriteLine("User has not borrowed any book");
                    }
                }
            }
            if (flag)
            {
                Console.WriteLine("Invalid User");
            }
            SubMenu();
        }

        public static void WalletRecharge()
        {
            foreach (User user in users)
            {
                if (user.UserID == currentLoggedInUser.UserID)
                {
                    Console.WriteLine("Do you wish to recharge your wallet? (yes/no)");
                    string userChoice = Console.ReadLine().ToLower().Trim();

                    if (userChoice == "yes")
                    {
                        Console.WriteLine("Enter recharge amount");
                        decimal rechargeAmount = decimal.Parse(Console.ReadLine());
                        user.WalletRecharge(rechargeAmount);
                        Console.WriteLine($"Your new balance is {user.WalletBalance}");
                    }
                    else
                    {
                        SubMenu();
                    }

                    break;
                }
            }
        }

        public static void DefaultData()
        {
            // user objects
            User user1 = new("Ravichandran", Gender.Male, Department.EEE, "993888333", "ravi@gmail.com", 100);
            User user2 = new("Priyadharshini", Gender.Female, Department.CSE, "9944444455", "priya@gmail.com", 10);

            users.Add(user1);
            users.Add(user2);

            // book objects
            Book book1 = new("C#", "Author1", 3);
            Book book2 = new("HTML", "Author2", 5);
            Book book3 = new("CSS", "Author1", 3);
            Book book4 = new("JS", "Author1", 3);
            Book book5 = new("TS", "Author2", 2);

            books.Add(book1);
            books.Add(book2);
            books.Add(book3);
            books.Add(book4);
            books.Add(book5);

            //borrow objects
            Borrow borrow1 = new(book1.BookID, user1.UserID, DateTime.Parse("10/09/2023"), 2, Status.Borrowed, 0);
            Borrow borrow2 = new(book3.BookID, user1.UserID, DateTime.Parse("12/09/2023"), 1, Status.Borrowed, 0);
            Borrow borrow3 = new(book4.BookID, user1.UserID, DateTime.Parse("14/09/2023"), 1, Status.Returned, 16);
            Borrow borrow4 = new(book2.BookID, user2.UserID, DateTime.Parse("11/09/2023"), 2, Status.Borrowed, 0);
            Borrow borrow5 = new(book5.BookID, user2.UserID, DateTime.Parse("09/09/2023"), 1, Status.Returned, 20);

            borrows.Add(borrow1);
            borrows.Add(borrow2);
            borrows.Add(borrow3);
            borrows.Add(borrow4);
            borrows.Add(borrow5);
        }
    }
}