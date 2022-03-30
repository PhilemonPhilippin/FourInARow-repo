namespace FourInARow.Entities
{
    public class Game
    {
        const int TOTALCOLUMNS = 7;
        const int TOTALROWS = 6;
        const char COINABSENCE = '_';
        const char PLAYERONECOIN = 'O';
        const char PLAYERTWOCOIN = 'X';

        bool isGameWon;
        bool isGridFull;
        List<Player> playerList;
        char[,] grid;
        int insertedCoinColumn;
        int insertedCoinRow;

        public void RunGame()
        {
            playerList = GetPlayerList();
            //activePlayer c'est toujours l'index du joueur dont c'est le tour de déposer une pièce
            int activePlayer = GetFirstPlayer();
            grid = GridSetUp();
            while (!isGameWon && !isGridFull)
            {
                ShowGrid();
                int playerColumnChoice = GetPlayerChoice(playerList[activePlayer]);
                InsertCoin(playerColumnChoice, playerList[activePlayer]);
                CheckIfGameWon(playerList[activePlayer]);
                activePlayer = (activePlayer == 0) ? 1 : 0;
                CheckIfGridIsFull();
            }
        }
        private void CheckIfGridIsFull()
        {
            isGridFull = true;
            for (int col = 0; col < TOTALCOLUMNS; col++)
            {
                for (int row = 0; row < TOTALROWS; row++)
                {
                    if (grid[col, row] == '_')
                        isGridFull = false;
                }
            }
            if (isGridFull)
                Console.WriteLine("Game Over. No one wins. The grid is full.");
        }

        private void CheckIfGameWon(Player player)
        {
            CheckIfGameWonVertical(player);
            CheckIfGameWonHorizontal(player);
            CheckIfGameWonDiagonal(player);
        }
        private void CheckIfGameWonVertical(Player player)
        {
            int coinCounter = 0;
            for (int row = 0; row < TOTALROWS; row++)
            {
                coinCounter = (grid[insertedCoinColumn, row] == player.PlayerCoin) ? ++coinCounter : 0;
                if (coinCounter == 4)
                {
                    ShowGrid();
                    Console.WriteLine($"Congratulations! Player {player.PlayerName} won.");
                    isGameWon = true;
                }
            }

        }

        private void CheckIfGameWonHorizontal(Player player)
        {
            int coinCounter = 0;
            for (int col = 0; col < TOTALCOLUMNS; col++)
            {
                coinCounter = (grid[col, insertedCoinRow] == player.PlayerCoin) ? ++coinCounter : 0;
                if (coinCounter == 4)
                {
                    ShowGrid();
                    Console.WriteLine($"Congratulations! Player {player.PlayerName} won.");
                    isGameWon = true;
                }
            }
        }

        private void CheckIfGameWonDiagonal(Player player)
        {
            int coinCounter = 0;
            int startColDiagonal = insertedCoinColumn - FindMinusOfDiagonal();
            int startRowDiagonal = insertedCoinRow - FindMinusOfDiagonal();
            int endColDiagonal = insertedCoinColumn + FindPlusOfDiagonal();
            int endRowDiagonal = insertedCoinRow + FindPlusOfDiagonal();

            while (startColDiagonal <= endColDiagonal && startRowDiagonal <= endRowDiagonal)
            {

                if (grid[startColDiagonal, startRowDiagonal] == player.PlayerCoin)
                    coinCounter++;
                else
                    coinCounter = 0;

                startColDiagonal++;
                startRowDiagonal++;

                if (coinCounter == 4)
                {
                    ShowGrid();
                    Console.WriteLine($"Congratulations! Player {player.PlayerName} won.");
                    isGameWon = true;
                }
            }
        }
        private int FindMinusOfDiagonal()
        {
            int minusOfDiagonal = 0;
            while (insertedCoinColumn - minusOfDiagonal > 0 && insertedCoinRow - minusOfDiagonal > 0)
                minusOfDiagonal++;
            return minusOfDiagonal;
        }
        private int FindPlusOfDiagonal()
        {
            int plusOfDiagonal = 0;
            while (insertedCoinColumn + plusOfDiagonal < TOTALCOLUMNS - 1 && insertedCoinRow + plusOfDiagonal < TOTALROWS - 1)
                plusOfDiagonal++;
            return plusOfDiagonal;
        }
        private int GetPlayerChoice(Player player)
        {
            int playerColumn;
            bool isColumnFull;
            do
            {
                playerColumn = AskPlayerExistingColumn(player);
                isColumnFull = CheckIfColumnIsFull(playerColumn);
            } while (isColumnFull);
            return playerColumn;
        }
        private bool CheckIfColumnIsFull(int playerColumn)
        {
            bool isColumnFull = true;
            for (int row = TOTALROWS - 1; row >= 0; row--)
            {
                if (grid[playerColumn, row] == COINABSENCE)
                    isColumnFull = false;
            }
            if (isColumnFull)
                Console.WriteLine("The column is full. Choose another one.");
            return isColumnFull;
        }
        private int AskPlayerExistingColumn(Player player)
        {
            int playerColumn = -1;
            bool isParsed;
            do
            {
                Console.WriteLine($"{player.PlayerName}'s turn to play. Choose the number of the column you want to insert your coin into (between 1 and 7).");
                isParsed = int.TryParse(Console.ReadLine(), out playerColumn);
                playerColumn--;
            } while (playerColumn < 0 || playerColumn > TOTALCOLUMNS - 1 || !isParsed);
            return playerColumn;
        }
        private void InsertCoin(int playerColumnChoice, Player player)
        {
            for (int row = 0; row < TOTALROWS; row++)
            {
                if (grid[playerColumnChoice, row] == COINABSENCE)
                {
                    grid[playerColumnChoice, row] = player.PlayerCoin;
                    insertedCoinColumn = playerColumnChoice;
                    insertedCoinRow = row;
                    return;
                }
            }
        }
        private void ShowGrid()
        {
            for (int i = 1; i <= TOTALCOLUMNS; i++)
            {
                Console.Write("|" + i + "|");
            }
            Console.WriteLine();
            for (int row = TOTALROWS - 1; row >= 0; row--)
            {
                for (int column = 0; column < TOTALCOLUMNS; column++)
                {
                    Console.Write("|" + grid[column, row] + "|");
                }
                Console.WriteLine();
            }
        }
        private char[,] GridSetUp()
        {
            char[,] grid = new char[TOTALCOLUMNS, TOTALROWS];
            for (int column = 0; column < TOTALCOLUMNS; column++)
            {
                for (int row = 0; row < TOTALROWS; row++)
                {
                    grid[column, row] = COINABSENCE;
                }
            }
            return grid;
        }

        private int GetFirstPlayer()
        {
            Random rand = new Random();
            int coinFlip = rand.Next(0, 2);
            int firstPlayer;
            if (coinFlip == 0)
            {
                Console.WriteLine($"Coin is flipped. Result: Head. Player {playerList[0].PlayerName} starts.");
                firstPlayer = 0;
            }
            else
            {
                Console.WriteLine($"Coin is flipped. Result: Tails. Player {playerList[1].PlayerName} starts.");
                firstPlayer = 1;
            }
            return firstPlayer;
        }
        private List<Player> GetPlayerList()
        {
            Console.WriteLine("Hello. Player One: Write your name please.");
            string playerOneName = Console.ReadLine();
            Console.WriteLine($"Welcome {playerOneName} in the Four in a Row Game. You start if coin flip is Head.Your coin is {PLAYERONECOIN}");
            Player playerOne = new Player()
            {
                PlayerName = playerOneName,
                PlayerCoin = PLAYERONECOIN
            };
            Console.WriteLine("Hello. Player Two: Write your name please.");
            string playerTwoName = Console.ReadLine();
            Console.WriteLine($"Welcome {playerTwoName} in the Four in a Row Game. You start if coin flip is Tails. Your coin is {PLAYERTWOCOIN}");
            Player playerTwo = new Player()
            {
                PlayerName = playerTwoName,
                PlayerCoin = PLAYERTWOCOIN
            };
            List<Player> playerList = new List<Player>();
            playerList.Add(playerOne);
            playerList.Add(playerTwo);
            return playerList;
        }
    }
}
