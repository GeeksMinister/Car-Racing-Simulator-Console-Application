using System.Timers;

static class CarRacing
{
#pragma warning disable CS8622
    public static List<Car>? Racers;
    public static List<Action> ThreadList = new List<Action>();
    public static List<Car>? Winners = new List<Car>();
    public static void ReturnList()
    {
        Racers = new List<Car>()
        {
            new Car("ABC001", "Shadow", 320),
            new Car("BCD002", "Smokey", 320),
            new Car("CDE003", "Furiosa", 320),
            new Car("DEF004", "Inferno", 320),
            new Car("EFG005", "Torcher", 320),
            new Car("FGH006", "SpeedX", 320),
            new Car("GHI007", "G-Lion", 320),
            new Car("HIJ008", "Panther", 320),
            new Car("IJK009", "Reaper", 320),
            new Car("JKL010", "ZeroXL", 320),
        };
    }

    public static void PrintContester()
    {
        ReturnList();
        WriteLine("\n\t\t\t\t* ( (  - CAR RACE -  ) ) *\n\n\n" +
            "   Here's a list of our contesters:\n");
        foreach (Car car in Racers)
        {
            Write($"\n\tName: {car.Name}\t\tREG: {car.RegNum}\tSpeed: {car.Speed}Km/h\n");
        }
        WriteLine("\n\n\t\t * Press 'Enter' to start the race *");
        ReadLine();
        Clear();
    }

    public static void PrintWinners()
    {
        WriteLine("\n\tAll cars have made it.  Press 'Enter' to see the results");
        ReadLine();
        Clear();
        int place = 0;
        foreach (Car car in Winners)
        {
            place++;
            Write($"\n\t{car.Name}\t\tPlace: [{place}]\n");
        }
        Winners.Clear();
        WriteLine("\n\n\t\tPress 'Enter' to repeat the race");
        ReadLine();
        StartRace();
    }

    public static void StartRace()
    {
        Clear();
        PrintContester();
        SetDistance();
    }

    public static void SetDistance()
    {
        foreach (var car in Racers)
        {
            car.Start(10);
            ThreadList.Add(() =>
            {
                new Thread(car.RacingToFinishLineTimer).Start();
            });
        }
        ChooseRaceScreen();

    }

    private static void ChooseRaceScreen()
    {
        string answer = string.Empty;
        while (answer != "YES" && answer != "NO")
        {
            Write("\n   Would like to get the status feed during the race ?\n\n" +
                "Type in 'Yes' or 'No' : ");
            answer = ReadLine().Trim().ToUpper();
        }
        if (answer == "YES")
        {
            Clear();
            CursorVisible = false;
            Parallel.ForEach(ThreadList, car => car.Invoke());
            Car.MoveRacersToWinnersTimer();
            Car.IncidentRandomizerTimer();
            Car.PrintStatusTimer();
            Car.PrintStatus();
        }
        else if (answer == "NO")
        {
            Clear();
            CursorVisible = false;
            Parallel.ForEach(ThreadList, car => car.Invoke());
            Car.IncidentRandomizerTimer();
            Car.MoveRacersToWinnersTimer();
            WriteLine("\n\t\t\t\t!!  THE RACE IS ON  !!\n");
        }
    }
}
