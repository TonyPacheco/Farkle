using Farkle;

Console.WriteLine("FARKLE");
Console.WriteLine("Target score?");
var target = 0;
while(target == 0)
{
    var input = Console.ReadLine();
    int.TryParse(input, out target);
}
Console.Clear();

new Game(target).Play();

Console.ReadLine();
