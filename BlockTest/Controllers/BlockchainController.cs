using Microsoft.AspNetCore.Mvc;
using BlockTest.Models;
using BlockTest.ViewModel;
using System.Threading.Tasks;
using System.Text.Json;

namespace BlockTest.Controllers
{
    public class BlockchainController : Controller
    {
        private static Blockchain _blockchain = new Blockchain();

        // View the blockchain
        public IActionResult Index()
        {
            var blockchainData = new BlockchainViewModel
            {
                Title = "My Blockchain",
                Blocks = _blockchain.Chain.Select(block => new BlockViewModel
                {
                    Index = block.Index,
                    Data = string.Join(", ", block.Transactions.Select(t => $"{t.Sender} -> {t.Receiver}: {t.Amount}")),
                    Hash = block.Hash,
                    PreviousHash = block.PreviousHash
                }).ToList()
            };

            return View(blockchainData);
        }

        // Add a transaction
        [HttpPost]
        public IActionResult AddTransaction(string sender, string receiver, decimal amount)
        {
            var transaction = new Transaction(sender, receiver, amount);
            _blockchain.AddTransaction(transaction);
            return RedirectToAction("Index");
        }

        // Mine the pending transactions
        [HttpPost]
        public IActionResult MineBlock([FromBody] JsonElement data)
        {
            if (!data.TryGetProperty("minerAddress", out var minerAddressProperty))
            {
                return BadRequest("Miner address is required.");
            }

            string minerAddress = minerAddressProperty.GetString();

            if (string.IsNullOrEmpty(minerAddress))
            {
                return BadRequest("Miner address is required.");
            }

            _blockchain.MinePendingTransactions(minerAddress);

            // Return a success message
            return Ok(new { message = "Block successfully mined!" });
        }



        // Validate the blockchain
        public IActionResult Validate()
        {
            bool isValid = _blockchain.IsChainValid();
            return Json(new { isValid });
        }
    }
}
