namespace LoadTester
{
    public class LoadTesterService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(LoadTest);
        }

        private async Task LoadTest()
        {
            var tasks = new List<Task>();

            for (var i = 0; i < 5; i++)
            {
                var church = RandomGenerator.Church();

                for (var j = 0; j < 20; j++)
                {
                    var team = RandomGenerator.TeamName();

                    var task = new LoadRunner(church, team).Run();

                    tasks.Add(task);

                    await Task.Delay(200);
                }
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("Completed load test");
        }
    }
}
