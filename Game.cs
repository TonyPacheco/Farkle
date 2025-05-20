namespace Farkle;

public class Game
{
    public static Game Instance = null!;
    private readonly Player _player1;
    private readonly Player _player2;
    private readonly int _targetScore;

    public Game(Player player1, Player player2, int targetScore)
    {
        Instance = this;
        _player1 = player1;
        _player2 = player2;
        _targetScore = targetScore;
    }

    public void Play()
    {
        while(NeitherPlayerHasTargetScore() || PlayersNeedTieBreak())
        {
            PrintState();
            _player1.TakeTurn();

            PrintState();
            _player2.TakeTurn();
        }

        Console.Clear();
        PrintState();
        Console.WriteLine($"{(_player1.Score > _player2.Score ? _player1.Name : _player2.Name)} WINS!!");
    }

    public bool NeitherPlayerHasTargetScore() => _player1.Score < _targetScore && _player2.Score < _targetScore;
    public bool PlayersNeedTieBreak() => _player1.Score == _player2.Score && _player1.Score >= _targetScore;

    public void PrintState() => Console.WriteLine($"{_player1.Name}: {_player1.Score}\t{_player2.Name}: {_player2.Score}\tTarget: {_targetScore}");
}

public class Player(int id, string? name = null)
{
    public int Id { get; private set; } = id;
    public string Name { get; private set; } = string.IsNullOrWhiteSpace(name) ? $"PLAYER {id}" : name;
    public int Score { get; set; } = 0;

    public int TakeTurn()
    {
        var turnScore = new Turn(this).Take();
        Score += turnScore;
        return turnScore;
    }
}
