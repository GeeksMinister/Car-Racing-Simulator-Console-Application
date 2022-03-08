#pragma warning disable CA1416 // Validate platform compatibility
Task.Run(() => SetWindowSize(90, 50));

CarRacing.StartRace();

while (true)
{
    Thread.Sleep(1000);
}

