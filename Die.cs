namespace Farkle;

public enum DieState
{
    Open,
    Held,
    Locked
};

public class Die
{
    public int Face { get; private set; }
    public DieState State { get; set; } = DieState.Open;

    public void Roll()
    {
        Face = Random.Shared.Next(1, 7);
    }

    public override string ToString()
    {
        var (left, right) = State switch
        {
            DieState.Open => ("[", "]"),
            DieState.Held or DieState.Locked  => ("<", ">"),
            _ => throw new NotImplementedException()
        };
        return $"{left}{Face}{right}";
    }
}
