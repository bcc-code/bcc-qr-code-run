using LoadTester;

Console.WriteLine();

var tasks = new List<Task>();

for (var i = 0; i < 5; i++)
{
    var church = RandomGenerator.Church();

    for (var j = 0; j < 10; j++)
    {
        var team = RandomGenerator.TeamName();

        var task = new LoadRunner(church, team).Run();

        tasks.Add(task);
    }
}

await Task.WhenAll(tasks);