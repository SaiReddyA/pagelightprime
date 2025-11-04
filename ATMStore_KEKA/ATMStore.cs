namespace ATMStore_KEKA
{
    public class AddCashRequest
    {
        public Dictionary<int, int> Notes { get; set; } // Example: {500:2, 200:1}
    }

    public class WithdrawRequest
    {
        public int Amount { get; set; }
    }

    public class Transaction
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } // Add or Withdraw
        public int Amount { get; set; }
        public string Details { get; set; } // e.g. "500x2, 200x1"
    }
    public static class ATMStore
    {
        public static Dictionary<int, int> CashNotes = new Dictionary<int, int>()
        {
            {20, 0}, {50, 0}, {100, 0}, {200, 0}, {500, 0}, {2000, 0}
        };

        public static List<Transaction> Transactions = new List<Transaction>();
    }

}
