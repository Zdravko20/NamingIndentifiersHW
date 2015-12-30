using System;
using System.Collections.Generic;

namespace Application2
{
    public class GameEngine
    {
        private string inputLine = string.Empty;
        private char[,] gameField = CreateGameField();
        private char[,] mines = GenerateMines();
        private int count = 0;
        private bool hasExploded = false;
        private List<LeaderBoard> champions;
        private int row = 0;
        private int col = 0;
        private bool hasStarted = true;
        private const int maxTurns = 35;
        private bool hasEnded = false;

        public GameEngine()
        {
            champions = new List<LeaderBoard>(6);
        }

        public void Run()
        {
            do
            {
                if (this.hasStarted)
                {
                    Console.WriteLine(
                        "Let's play minesweeper! The top command shows top 5 players, " +
                        "reset command resets the game and the exit " +
                        "command stops the game. Good luck!");
                    DumpGameField(gameField);
                    hasStarted = false;
                }

                Console.Write("Choose row and column : ");
                try
                {
                    inputLine = Console.ReadLine().Trim();
                    if (inputLine.Length == 3)
                    {
                        if (int.TryParse(inputLine[0].ToString(), out row) &&
                            int.TryParse(inputLine[2].ToString(), out col)
                            && row <= gameField.GetLength(0) && col <= gameField.GetLength(1))
                        {
                            inputLine = "turn";
                        }
                    }
                    ManageCommand();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (hasExploded)
                {
                    DumpGameField(mines);
                    Console.Write("\nYou died hroicly with {0} to4ki. " + "Enter your name: ", count);
                    string nickName = Console.ReadLine();
                    LeaderBoard leaderBoard = new LeaderBoard(nickName, count);
                    if (champions.Count < 5)
                    {
                        champions.Add(leaderBoard);
                    }
                    else
                    {
                        for (int i = 0; i < champions.Count; i++)
                        {
                            if (champions[i].Points < leaderBoard.Points)
                            {
                                champions.Insert(i, leaderBoard);
                                champions.RemoveAt(champions.Count - 1);
                                break;
                            }
                        }
                    }

                    champions.Sort((LeaderBoard r1, LeaderBoard r2) => String.Compare(r2.PlayerName, r1.PlayerName, StringComparison.Ordinal));
                    champions.Sort((LeaderBoard r1, LeaderBoard r2) => r2.Points.CompareTo(r1.Points));
                    CreateLeaderBoard(champions);

                    gameField = CreateGameField();
                    mines = GenerateMines();
                    count = 0;
                    hasExploded = false;
                    hasStarted = true;
                }

                if (hasEnded)
                {
                    Console.WriteLine("\nCongratulations! You opened 35 cells without spilling any blood!");
                    DumpGameField(mines);
                    Console.WriteLine("Enter your name: ");
                    string name = Console.ReadLine();
                    LeaderBoard points = new LeaderBoard(name, count);
                    champions.Add(points);
                    CreateLeaderBoard(champions);
                    gameField = CreateGameField();
                    mines = GenerateMines();
                    count = 0;
                    hasEnded = false;
                    hasStarted = true;
                }
            }
            while (inputLine != "exit");
            Console.WriteLine("Made in Bulgaria");
            Console.Read();
        }

        private void ManageCommand()
        {
            switch (inputLine)
            {
                case "top":
                    CreateLeaderBoard(champions);
                    break;
                case "restart":
                    gameField = CreateGameField();
                    mines = GenerateMines();
                    DumpGameField(gameField);
                    hasExploded = false;
                    hasStarted = false;
                    break;
                case "exit":
                    Console.WriteLine("Bye, bye!");
                    break;
                case "turn":
                    if (mines[row, col] != '*')
                    {
                        if (mines[row, col] == '-')
                        {
                            ContinueTurns(gameField, mines, row, col);
                            count++;
                        }

                        if (maxTurns == count)
                        {
                            hasEnded = true;
                        }
                        else
                        {
                            DumpGameField(gameField);
                        }
                    }
                    else
                    {
                        hasExploded = true;
                    }

                    break;
                default:
                    Console.WriteLine("\nInvalid command!\n");
                    break;
            }
        }

        private static void CreateLeaderBoard(List<LeaderBoard> points)
        {
            Console.WriteLine("\nPoints:");
            if (points.Count > 0)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    Console.WriteLine("{0}. {1} --> {2} cells", i + 1, points[i].PlayerName, points[i].Points);
                }

                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("No entries!\n");
            }
        }

        private static void ContinueTurns(char[,] gameField, char[,] mines, int row, int col)
        {
            char mineCount = CountMines(mines, row, col);
            mines[row, col] = mineCount;
            gameField[row, col] = mineCount;
        }

        private static void DumpGameField(char[,] board)
        {
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);
            Console.WriteLine("\n    0 1 2 3 4 5 6 7 8 9");
            Console.WriteLine("   ---------------------");
            for (int i = 0; i < rows; i++)
            {
                Console.Write("{0} | ", i);
                for (int j = 0; j < cols; j++)
                {
                    Console.Write("{0} ", board[i, j]);
                }
                Console.Write("|");
                Console.WriteLine();
            }

            Console.WriteLine("   ---------------------\n");
        }

        private static char[,] CreateGameField()
        {
            int boardRows = 5;
            int boardColumns = 10;
            char[,] board = new char[boardRows, boardColumns];
            for (int i = 0; i < boardRows; i++)
            {
                for (int j = 0; j < boardColumns; j++)
                {
                    board[i, j] = '?';
                }
            }

            return board;
        }

        private static char[,] GenerateMines()
        {
            int rows = 5;
            int cols = 10;
            char[,] gameField = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    gameField[i, j] = '-';
                }
            }

            List<int> randomInts = new List<int>();
            while (randomInts.Count < 15)
            {
                Random random = new Random();
                int randomInt = random.Next(50);
                if (!randomInts.Contains(randomInt))
                {
                    randomInts.Add(randomInt);
                }
            }

            foreach (int randomInt in randomInts)
            {
                int col = randomInt / cols;
                int row = randomInt % cols;
                if (row == 0 && randomInt != 0)
                {
                    col--;
                    row = cols;
                }
                else
                {
                    row++;
                }

                gameField[col, row - 1] = '*';
            }

            return gameField;
        }

        private static void Calculate(char[,] field)
        {
            int col = field.GetLength(0);
            int row = field.GetLength(1);

            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    if (field[i, j] != '*')
                    {
                        char mineCount = CountMines(field, i, j);
                        field[i, j] = mineCount;
                    }
                }
            }
        }

        private static char CountMines(char[,] mines, int row, int col)
        {
            int minesQuantity = 0;
            int rows = mines.GetLength(0);
            int cols = mines.GetLength(1);

            if (row - 1 >= 0)
            {
                if (mines[row - 1, col] == '*')
                {
                    minesQuantity++;
                }
            }

            if (row + 1 < rows)
            {
                if (mines[row + 1, col] == '*')
                {
                    minesQuantity++;
                }
            }

            if (col - 1 >= 0)
            {
                if (mines[row, col - 1] == '*')
                {
                    minesQuantity++;
                }
            }

            if (col + 1 < cols)
            {
                if (mines[row, col + 1] == '*')
                {
                    minesQuantity++;
                }
            }

            if ((row - 1 >= 0) && (col - 1 >= 0))
            {
                if (mines[row - 1, col - 1] == '*')
                {
                    minesQuantity++;
                }
            }

            if ((row - 1 >= 0) && (col + 1 < cols))
            {
                if (mines[row - 1, col + 1] == '*')
                {
                    minesQuantity++;
                }
            }

            if ((row + 1 < rows) && (col - 1 >= 0))
            {
                if (mines[row + 1, col - 1] == '*')
                {
                    minesQuantity++;
                }
            }

            if ((row + 1 < rows) && (col + 1 < cols))
            {
                if (mines[row + 1, col + 1] == '*')
                {
                    minesQuantity++;
                }
            }

            return char.Parse(minesQuantity.ToString());
        }
    }
}