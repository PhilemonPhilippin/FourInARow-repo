namespace FourInARow.Entities
{
    public class Player
    {
        private char _playerCoin;
        public string PlayerName { get; set; }
        public char PlayerCoin
        {
            get { return _playerCoin; }
            set { _playerCoin = value; }
        }
    }
}
