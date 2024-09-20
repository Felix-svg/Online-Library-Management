using System;

namespace OnlineLibraryManagement
{
    public enum Department
    {
        ECE,
        EEE,
        CSE
    }

    public enum Gender
    {
        Male,
        Female,
        Transgender
    }

    public class User
    {
        private static int s_userID = 3000;
        public string UserID { get; set; }
        public string UserName { get; set; }
        public Gender Gender { get; set; }
        public Department Department { get; set; }
        public string MobileNumber { get; set; }
        public string MailID { get; set; }
        public decimal WalletBalance { get; set; }

        public decimal WalletRecharge(decimal amount)
        {
            WalletBalance += amount;
            return WalletBalance;
        }

        public decimal DeductBalance(decimal amount)
        {
            WalletBalance -= amount;
            return WalletBalance;
        }

        public User(string userName, Gender gender, Department department, string mobileNumber, string maillID, decimal walletBalance)
        {
            UserID = $"SF{s_userID++}";
            UserName = userName;
            Gender = gender;
            Department = department;
            MobileNumber = mobileNumber;
            MailID = maillID;
            WalletBalance = walletBalance;
        }

    }
}