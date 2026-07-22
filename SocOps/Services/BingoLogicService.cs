using SocOps.Data;
using SocOps.Models;

namespace SocOps.Services;

public class BingoLogicService
{
    private const int BoardSize = 5;
    private const int CenterIndex = 12; // 5x5 grid, center is index 12 (row 2, col 2)
    private static readonly Random _random = new();
    private static readonly List<BingoLine> WinningLines = BuildWinningLines();

    /// <summary>
    /// Shuffle an array using Fisher-Yates algorithm
    /// </summary>
    private static List<T> ShuffleArray<T>(List<T> array)
    {
        var shuffled = new List<T>(array);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }

        return shuffled;
    }

    /// <summary>
    /// Generate a new 5x5 bingo board
    /// </summary>
    public static List<BingoSquareData> GenerateBoard()
    {
        var shuffledQuestions = ShuffleArray(Questions.QuestionsList)
            .Take(BoardSize * BoardSize - 1)
            .ToList();

        var board = new List<BingoSquareData>(BoardSize * BoardSize);
        var questionIndex = 0;

        for (int i = 0; i < BoardSize * BoardSize; i++)
        {
            board.Add(i == CenterIndex
                ? CreateSquare(i, Questions.FREE_SPACE, isFreeSpace: true, isMarked: true)
                : CreateSquare(i, shuffledQuestions[questionIndex++], isFreeSpace: false, isMarked: false));
        }

        return board;
    }

    /// <summary>
    /// Toggle a square's marked state
    /// </summary>
    public static List<BingoSquareData> ToggleSquare(List<BingoSquareData> board, int squareId)
    {
        return board.Select(square =>
            square.Id == squareId && !square.IsFreeSpace
                ? CopySquare(square, !square.IsMarked)
                : square
        ).ToList();
    }

    private static BingoSquareData CreateSquare(int id, string text, bool isFreeSpace, bool isMarked) => new()
    {
        Id = id,
        Text = text,
        IsMarked = isMarked,
        IsFreeSpace = isFreeSpace
    };

    private static BingoSquareData CopySquare(BingoSquareData square, bool isMarked) => new()
    {
        Id = square.Id,
        Text = square.Text,
        IsMarked = isMarked,
        IsFreeSpace = square.IsFreeSpace
    };

    private static List<BingoLine> BuildWinningLines()
    {
        var lines = new List<BingoLine>(BoardSize * 2 + 2);

        for (int row = 0; row < BoardSize; row++)
        {
            lines.Add(BuildLine("row", row, GetRowSquares(row)));
        }

        for (int col = 0; col < BoardSize; col++)
        {
            lines.Add(BuildLine("column", col, GetColumnSquares(col)));
        }

        lines.Add(BuildLine("diagonal", 0, Enumerable.Range(0, BoardSize).Select(i => i * (BoardSize + 1))));
        lines.Add(BuildLine("diagonal", 1, Enumerable.Range(0, BoardSize).Select(i => (i + 1) * (BoardSize - 1))));

        return lines;
    }

    private static BingoLine BuildLine(string type, int index, IEnumerable<int> squares) => new()
    {
        Type = type,
        Index = index,
        Squares = squares.ToList()
    };

    private static List<int> GetRowSquares(int row) =>
        Enumerable.Range(0, BoardSize).Select(col => row * BoardSize + col).ToList();

    private static List<int> GetColumnSquares(int col) =>
        Enumerable.Range(0, BoardSize).Select(row => row * BoardSize + col).ToList();

    /// <summary>
    /// Check if there's a bingo and return the winning line
    /// </summary>
    public static BingoLine? CheckBingo(List<BingoSquareData> board)
    {
        foreach (var line in WinningLines)
        {
            if (line.Squares.All(idx => board[idx].IsMarked))
            {
                return line;
            }
        }

        return null;
    }

    /// <summary>
    /// Get the square IDs that are part of a winning line
    /// </summary>
    public static HashSet<int> GetWinningSquareIds(BingoLine? line) =>
        line is null ? new() : new(line.Squares);
}
