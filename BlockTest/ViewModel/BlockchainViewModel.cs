namespace BlockTest.ViewModel
{
    public class BlockchainViewModel
    {
        public string Title { get; set; }
        public List<BlockViewModel> Blocks { get; set; }
    }

    public class BlockViewModel
    {
        public int Index { get; set; }
        public string Data { get; set; }
        public string Hash { get; set; }
        public string PreviousHash { get; set; }
    }
}
