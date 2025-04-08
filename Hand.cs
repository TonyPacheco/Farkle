namespace Farkle;

public enum RollResult
{
    Ok,
    Bust
}

public class Hand
{
    public Die[] Dice =
    [
        new Die(), new Die(), new Die(), new Die(), new Die(), new Die()
    ];
    public int UnlockedDiceCount => Dice.Count(d => d.State != DieState.Locked);
    public bool Busted => ScoreAnalyzer.Evaluate(Dice.Where(d => d.State != DieState.Locked)) == 0;

    private IEnumerable<Die> HeldDice => Dice
        .Where(d => d.State == DieState.Held)
        .OrderBy(d => d.Face)
        .ToArray();

    public RollResult Roll()
    {
        foreach(var die in Dice.Where(d => d.State == DieState.Open))
        {
            die.Roll();
        }

        Dice = Dice.OrderBy(d => d.State == DieState.Locked)
            .ThenBy(d => d.Face)
            .ToArray();

        return ScoreAnalyzer.Evaluate(Dice.Where(d => d.State == DieState.Open)) switch
        {
            0 => RollResult.Bust,
            _ => RollResult.Ok,
        };
    }

    public int EvaluateHeldScore()
    {
        return ScoreAnalyzer.Evaluate(HeldDice);
    }

    public bool HeldIncludesNonScoringDice()
    {
        //This method relies on the fact that removing a non-scoring die from
        //a selection of scoring dice would not change the score,
        //so try removing each die in the held selection one at a time
        var tentativeScore = EvaluateHeldScore();
        foreach(var die in HeldDice)
        {
            var testSet = HeldDice.Except([die]);
            var testScore = ScoreAnalyzer.Evaluate(testSet);
            if(tentativeScore == testScore)
            {
                return true;
            }
        }
        return false;
    }

    public void ToggleHold(int index)
    {
        Dice[index].State = Dice[index].State switch
        {
            DieState.Open => DieState.Held,
            DieState.Held => DieState.Open,
            _ => throw new NotImplementedException()
        };
    }
}
