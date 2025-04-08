namespace Farkle;

public static class ScoreAnalyzer
{
    private static readonly Dictionary<int, int> ScoreCounts = [];
    private static void InitializeScoreCounts()
    {
        for(var i = 1; i <= 6; i++)
        {
            ScoreCounts[i] = 0;
        }
    }

    public static int Evaluate(IEnumerable<Die> dice)
    {
        if(!dice.Any())
        {
            return 0;
        }
        InitializeScoreCounts();
        foreach(var die in dice)
        {
            ScoreCounts[die.Face]++;
        }
        if(IsStraight(large: true))
        {
            return 1500;
        }
        if(IsStraight(large: false))
        {
            if(ScoreCounts[1] == 2)
            {
                //1, 1, 2, 3, 4, 5 => plus 100 for the extra 1
                return 850;
            }
            if(ScoreCounts[5] == 2)
            {
                //2, 3, 4, 5, 5, 6 => plus 50 for the extra 5
                return 800;
            }
            return 750;
        }
        for(var i = 1; i <= 6; i++)
        {
            //check for 6-of-a-kind for all face values
            if(HaveDieCount(i, 6))
            {
                return GetTripleValue(i) * 8;
            }
        }
        for(var i = 1; i <= 6; i++)
        {
            //check for-5-of-a-kind for all face values
            if(HaveDieCount(i, 5))
            {
                var quintValue = GetTripleValue(i) * 4;
                if(i != 1 && ScoreCounts[1] == 1)
                {
                    return quintValue + 100;
                }
                if(i != 5 && ScoreCounts[5] == 1)
                {
                    return quintValue + 50;
                }
                return quintValue;
            }
        }
        for(var i = 1; i <= 6; i++)
        {
            //check for 4-of-a-kind for all face values
            if(HaveDieCount(i, 4))
            {
                var quadValue = GetTripleValue(i) * 2;
                if(i != 1)
                {
                    quadValue += ScoreCounts[1] * 100;
                }
                if(i != 5)
                {
                    quadValue += ScoreCounts[5] * 50;
                }
                return quadValue;
            }
        }
        var firstTrip = 0;
        var secondTrip = 0;
        for(var i = 1; i <= 6; i++)
        {
            //check for 3-of-a-kind for all face values
            //its possible to get two 3-of-a-kinds
            if(HaveDieCount(i, 3))
            {
                if(firstTrip == 0)
                {
                    firstTrip = i;
                }
                else
                {
                    secondTrip = i;
                    break;
                }
            }
        }
        if(secondTrip != 0)
        {
            return GetTripleValue(firstTrip) + GetTripleValue(secondTrip);
        }
        if(firstTrip != 0)
        {
            var tripValue = GetTripleValue(firstTrip);
            if(firstTrip != 1)
            {
                tripValue += ScoreCounts[1] * 100;
            }
            if(firstTrip != 5)
            {
                tripValue += ScoreCounts[5] * 50;
            }
            return tripValue;
        }

        var score = (ScoreCounts[1] * 100) + (ScoreCounts[5] * 50);
        if(score > 0)
        {
            return score;
        }

        return 0;
    }

    private static bool IsStraight(bool large) =>
           ScoreCounts[2] > 0
        && ScoreCounts[3] > 0
        && ScoreCounts[4] > 0
        && ScoreCounts[5] > 0
        && ((!large && (ScoreCounts[1] > 0 || ScoreCounts[6] > 0)) || (large && ScoreCounts[1] > 0 && ScoreCounts[6] > 0));

    private static bool HaveDieCount(int face, int count) => ScoreCounts[face] == count;

    private static int GetTripleValue(int face) => face switch
    {
        1 => 1000,
        2 => 200,
        3 => 300,
        4 => 400,
        5 => 500,
        6 => 600,
        _ => throw new NotImplementedException()
    };
}
