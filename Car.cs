using System.Security.Cryptography;
using System.Timers;

public class Car
{
#pragma warning disable CS8604
#pragma warning disable CS8622
    public string RegNum { get; set; }
    public string Name { get; set; }
    public double Speed { get; set; }
    public bool Assigned { get; set; } = false;
    private int _trackNum = 0;
    System.Timers.Timer? _raceTimer = new System.Timers.Timer(1000);
    static System.Timers.Timer? _checkWinnerTimer = new System.Timers.Timer(200);
    static System.Timers.Timer? _incidentTimer = new System.Timers.Timer(30000);
    static System.Timers.Timer? _statusTimer = new System.Timers.Timer(10000);
    public Car(string regNum, string name, double speed)
    {
        RegNum = regNum;
        Name = name;
        Speed = speed;
    }
    public double TimeToFinishLine = 0;
    public void Start(double distance) => TimeToFinishLine = (distance / Speed) * 60 * 60;

    public void RacingToFinishLineTimer()
    {
        _raceTimer.Elapsed += RacingToFinishLineEvent;
        _raceTimer.AutoReset = true;
        _raceTimer.Enabled = true;
    }

    private void AssignTrack()
    {
        if (!Assigned)
        {
            _trackNum = Thread.CurrentThread.ManagedThreadId;
            Assigned = true;
        }
    }

    private void RacingToFinishLineEvent(Object source, ElapsedEventArgs e)
    {
        AssignTrack();
        if (CarRacing.Winners.Count == 10)
        {
            CursorVisible = true;
        }
        if (TimeToFinishLine >= 1)
        {
            TimeToFinishLine -= 1;
        }
    }

    public static void MoveRacersToWinnersTimer()
    {
        _checkWinnerTimer.Elapsed += MoveRacersToWinnersEvent;
        _checkWinnerTimer.AutoReset = true;
        _checkWinnerTimer.Enabled = true;
    }

    private static void MoveRacersToWinnersEvent(Object source, ElapsedEventArgs e)
    {
        foreach (Car car in CarRacing.Racers.ToList())
        {
            if (car.TimeToFinishLine < 1)
            {
                CarRacing.Winners.Add(car);
                WriteLine($"\n  * ({car.Name})\tfinished on track: " +
                $"[{car._trackNum}] *");
                CarRacing.Racers.Remove(car);
            }
        }
        if (CarRacing.Winners.Count == 10)
        {
            _statusTimer.Enabled = false;
            _incidentTimer.Enabled = false;
            _checkWinnerTimer.Enabled = false;
            CarRacing.Racers.Clear();
            CarRacing.ThreadList.Clear();
            Thread.Sleep(1000);
            CursorVisible = true;
            CarRacing.PrintWinners();
        }
    }

    private static void IncidentRandomizer()
    {
        try
        {
            Thread.Sleep(600);
            foreach (Car car in CarRacing.Racers.ToList())
            {
                car.MakeObstacle();
            }
            WriteLine("\t____________________________________________________________");
        }
        catch (Exception)
        {

        }
    }

    private void MakeObstacle()
    {
        int randum = RandomNumberGenerator.GetInt32(1, 101);
        switch (randum)
        {
            case <= 2:                     // 2%
                TimeToFinishLine += 30;
                WriteLine($"\n\t\t({Name})\t is -stopping for refueling-");
                return;
            case <= 6:                     // 2 + 4 = 4%
                TimeToFinishLine += 20;
                WriteLine($"\n\t\t({Name})\t got -flat tire-");
                return;
            case <= 16:                     // 2 + 4 + 10 = 10%
                TimeToFinishLine += 10;
                WriteLine($"\n\t\t({Name})\t got -hit by a bird-");
                return;
            case <= 36:                     // 2 + 6 + 10 + 20 = 20%
                Speed -= 1;
                WriteLine($"\n\t\t({Name})\t is having an -engine failure- ");
                return;
        }
    }

    public static void IncidentRandomizerTimer()
    {
        _incidentTimer.Elapsed += IncidentEvent;
        _incidentTimer.AutoReset = true;
        _incidentTimer.Enabled = true;
    }

    private static void IncidentEvent(object source, ElapsedEventArgs e)
    {
        IncidentRandomizer();
    }

    public static void PrintStatusTimer()
    {
        _statusTimer.Elapsed += StatusIvent;
        _statusTimer.AutoReset = true;
        _statusTimer.Enabled = true;
    }

    private static void StatusIvent(object source, ElapsedEventArgs e)
    {
        PrintStatus();
    }

    public static void PrintStatus()
    {
        try
        {
            Clear();
            CursorVisible = false;
            WriteLine("\n\t\t\t\t!!  THE RACE IS ON  !!\n");
            WriteLine($"\n\n\t\t\t\t   --Race Status--\n\n\n\t\t\tCar Name\tCurrent Speed\tkm remaining\n");
            foreach (Car car in CarRacing.Racers.ToList())
            {
                WriteLine($"\t\t\t{car.Name}\t\t{car.Speed}Km/h\t\t" +
                    $"{(car.TimeToFinishLine / 60 / 60) * car.Speed:#.##}\n");
            }
            Write("\t\t_________________________________________________\n");
        }
        catch (Exception)
        {

        }
    }
}
