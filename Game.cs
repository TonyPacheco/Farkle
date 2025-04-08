namespace Farkle;

public class Game
{
    public static Game Instance = null!;
    public int CurrentPlayer { get; private set; } = 0;

    private int _player1Score = 0;
    private int _player2Score = 0;

    private readonly int _targetScore;

    public Game(int targetScore)
    {
        Instance = this;
        _targetScore = targetScore;
    }

    public void Play()
    {
        while(_player1Score < _targetScore && _player2Score < _targetScore)
        {
            PrintState();
            CurrentPlayer = 1;
            _player1Score += new Turn().Take();

            PrintState();
            CurrentPlayer = 2;
            _player2Score += new Turn().Take();
        }

        Console.Clear();
        PrintState();
        Console.WriteLine($"PLAYER {(_player1Score > _player2Score ? "1" : "2")} WINS!!");
    }

    public void PrintState() => Console.WriteLine($"P1: {_player1Score}\tP2: {_player2Score}\tTarget: {_targetScore}");
}
