using Farkle;

Console.Clear();
Console.WriteLine("FARKLE");
Console.WriteLine("Target score?");
var target = 0;
while(target == 0)
{
    var input = Console.ReadLine();
    int.TryParse(input, out target);
}
Console.Clear();

Console.WriteLine("Player 1 name?");
var player1 = new Player(1, Console.ReadLine());
Console.Clear();

Console.WriteLine("Player 2 name?");
var player2 = new Player(2, Console.ReadLine());
Console.Clear();

new Game(player1, player2, target).Play();

Console.ReadLine();
