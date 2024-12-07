using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;

namespace BlockTest.Models
{
    public class Blockchain
    {
        public List<Block> Chain { get; set; }
        public List<Transaction> PendingTransactions { get; set; }
        public decimal MiningReward { get; set; }
        public List<string> Nodes { get; set; }

        public Blockchain()
        {
            Chain = new List<Block> { CreateGenesisBlock() };
            PendingTransactions = new List<Transaction>();
            MiningReward = 10; // Mining reward for miners
            Nodes = new List<string>(); // List of peer nodes (for P2P network)
        }

        // Create the genesis block
        public Block CreateGenesisBlock()
        {
            return new Block(0, DateTime.Now, new List<Transaction>(), "0");
        }

        // Add a new transaction to the pending transactions
        public void AddTransaction(Transaction transaction)
        {
            PendingTransactions.Add(transaction);
        }

        // Mine the pending transactions into a new block
        public void MinePendingTransactions(string minerAddress)
        {
            var block = new Block(Chain.Count, DateTime.Now, PendingTransactions, Chain.Last().Hash);
            block.MineBlock(2); // Difficulty level (leading zeros)
            Chain.Add(block);

            // Clear pending transactions
            PendingTransactions.Clear();

            // Reward the miner with mining reward
            AddTransaction(new Transaction(null, minerAddress, MiningReward));
        }

        // Validate the blockchain
        public bool IsChainValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];
                var previousBlock = Chain[i - 1];

                // Validate the block's hash and previous block's hash
                if (currentBlock.Hash != currentBlock.CalculateHash() || currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }
            return true;
        }

        // Add new node to the list of peers (for P2P network)
        public void AddNode(string address)
        {
            Nodes.Add(address);
        }

    }
}
