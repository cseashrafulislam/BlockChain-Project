using System.Security.Cryptography;
using System.Text;
using System.Transactions;

namespace BlockTest.Models
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public int Nonce { get; set; }

        public Block(int index, DateTime timestamp, List<Transaction> transactions, string previousHash)
        {
            Index = index;
            Timestamp = timestamp;
            Transactions = transactions;
            PreviousHash = previousHash;
            Hash = CalculateHash();
        }

        // Method to calculate the hash of the block
        public string CalculateHash()
        {
            string blockData = $"{Index}{Timestamp}{PreviousHash}{string.Join("", Transactions.Select(t => t.Sender + t.Receiver + t.Amount))}{Nonce}";
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(blockData));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        // Method to mine the block (proof of work)
        public void MineBlock(int difficulty)
        {
            string target = new string('0', difficulty); // Difficulty defines how many leading zeros the hash must have
            while (Hash.Substring(0, difficulty) != target)
            {
                Nonce++;
                Hash = CalculateHash();
            }
        }
    }
}
