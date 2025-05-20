namespace Farkle;

public class Turn(Player player)
{
    public int RunningScore = 0;
    public Hand MyHand = new();

    public int Take()
    {
        Console.WriteLine($"{player.Name} PRESS ENTER TO ROLL");
        Console.ReadLine();
        Console.Clear();

        var result = MyHand.Roll();
        if(result == RollResult.Bust)
        {
            HandleBust();
            return 0;
        }

        while(true)
        {
            PrintGameState();
            PrintCurrentHand();
            PrintOptions();
            var selection = Console.ReadKey();
            if(char.IsDigit(selection.KeyChar))
            {
                Console.Clear();
                var dieSelect = int.Parse(selection.KeyChar.ToString());
                if(dieSelect >= 1 && dieSelect <= MyHand.UnlockedDiceCount)
                {
                    MyHand.ToggleHold(dieSelect - 1);
                }
                else
                {
                    AlertForInvalidInput();
                }
            }
            else if(selection.Key == ConsoleKey.Enter)
            {
                Console.Clear();
                if(MyHand.EvaluateHeldScore() == 0 || MyHand.HeldIncludesNonScoringDice())
                {
                    //Can't roll without a valid held score or with non-scoring dice held
                    AlertForInvalidInput();
                    continue;
                }
                
                RunningScore += MyHand.EvaluateHeldScore();
                foreach(var die in MyHand.Dice)
                {
                    if(die.State == DieState.Held)
                    {
                        die.State = DieState.Locked;
                    }
                }
                if(MyHand.Dice.All(d => d.State == DieState.Locked))
                {
                    //All dice are held, reroll full hand
                    foreach(var die in MyHand.Dice)
                    {
                        die.State = DieState.Open;
                    }
                }
                result = MyHand.Roll();
                if(result == RollResult.Bust)
                {
                    HandleBust();
                    return 0;
                }
            }
            else if(selection.Key == ConsoleKey.F)
            {
                Console.Clear();
                return RunningScore + MyHand.EvaluateHeldScore();
            }
            else
            {
                //Invalid input
                AlertForInvalidInput();
                Console.Clear();
            }
        }
    }

    private void HandleBust()
    {
        Console.Clear();
        PrintGameState();
        PrintCurrentHand();
        Console.WriteLine("BUST!!!");
        Console.ReadLine();
        Console.Clear();
    }

    private static void AlertForInvalidInput()
    {
        Console.Beep();
    }

    private void PrintGameState()
    {
        Game.Instance.PrintState();
        var heldScore = MyHand.EvaluateHeldScore();
        if(MyHand.HeldIncludesNonScoringDice())
        {
            heldScore = 0;
        }
        Console.WriteLine($"Turn: {RunningScore + heldScore}");
    }

    private void PrintCurrentHand()
    {
        var diceByLockState = MyHand.Dice
            .GroupBy(d => d.State == DieState.Locked)
            .ToDictionary(d => d.Key, d => d.OrderBy(d => d.Face));

        Console.WriteLine();
        if(diceByLockState.TryGetValue(false, out var unlockedDice))
        {
            foreach(var die in unlockedDice)
            {
                Console.Write($"{die}");
            }
        }
        if(diceByLockState.TryGetValue(true, out var lockedDice))
        {
            //Draw the locked dice off to the right of the current roll
            Console.Write("          ");
            foreach(var die in lockedDice)
            {
                Console.Write($"<{die.Face}>");
            }
        }
        Console.WriteLine();
    }

    private void PrintOptions()
    {
        for(var i = 0; i < MyHand.UnlockedDiceCount; ++i)
        {
            //Draw the option below each non-locked die to toggle
            Console.Write($"({i + 1})");
        }
        Console.WriteLine("\nPress # To toggle Hold - Press F to End Turn - Press Enter to roll again");
    }
}
