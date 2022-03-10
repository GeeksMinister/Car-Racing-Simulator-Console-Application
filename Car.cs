using System.Security.Cryptography;
using System.Timers;

public class Car
{
#pragma warning disable CS8604
#pragma warning disable CS8622
    public string RegNum { get; set; }
    public string Name { get; set; }
    public double Speed { get; set; }
    System.Timers.Timer? _raceTimer;
    static System.Timers.Timer? _incidentTimer;
    static System.Timers.Timer? _statusTimer;
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
        _raceTimer = new System.Timers.Timer(500);
        _raceTimer.Elapsed += CloserToFinishLineEvent;
        _raceTimer.AutoReset = true;
        _raceTimer.Enabled = true;
    }

    void CloserToFinishLineEvent(Object source, ElapsedEventArgs e)
    {

        if (TimeToFinishLine >= 0.5)
        {
            TimeToFinishLine -= 0.5;
        }
        else
        {
            WriteLine($"\n   ({Name})\tfinished on track: " +
                $"[{Thread.CurrentThread.ManagedThreadId}]");
            try
            {
                if (CarRacing.Racers.ToList().Exists(car => car == this))
                {
                    CarRacing.Winners.Add(this);
                    if (!CarRacing.Racers.Remove(this))
                    {
                        
                    }else if (!CarRacing.Racers.Remove(this))
                    {
                        CarRacing.Racers.Remove(this);
                    }
                    
                }
            }
            catch (Exception)
            {

            }
            _raceTimer.Enabled = false;
        }
        if (CarRacing.Winners.Count == 10)
        {
            CarRacing.Racers.Clear();
            _statusTimer.Enabled = false;
            _incidentTimer.Enabled = false;
            CarRacing.ThreadList.Clear();
            CarRacing.PrintWinners();
        }
    }

    private static void IncidentRandomizer()
    {
        Thread.Sleep(750);
        try
        {
            foreach (Car car in CarRacing.Racers.ToList())
            {
                car.MakeObstacle();
            }
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
        _incidentTimer = new System.Timers.Timer(30000);
        _incidentTimer.Elapsed += IncidentEvent;
        _incidentTimer.AutoReset = true;
        _incidentTimer.Enabled = true;
    }

    private static void IncidentEvent(object source, ElapsedEventArgs e)
    {
        if (CarRacing.Winners.Count == 10 || CarRacing.Racers.Count == 0)
        {
            _incidentTimer.Enabled = false;
            _statusTimer.Enabled = false;
            _incidentTimer.AutoReset = false;
            _statusTimer.AutoReset = false;
        }
        IncidentRandomizer();
    }

    public static void PrintStatusTimer()
    {
        _statusTimer = new System.Timers.Timer(10000);
        _statusTimer.Elapsed += StatusIvent;
        _statusTimer.AutoReset = true;
        _statusTimer.Enabled = true;
    }

    private static void StatusIvent(object source, ElapsedEventArgs e)
    {
        if (CarRacing.Winners.Count == 10 || CarRacing.Racers.Count == 0)
        {
            _statusTimer.Enabled = false;
            return;
        }
        PrintStatus();
    }

    public static void PrintStatus()
    {
        try
        {
            Clear();
            WriteLine("\n\t\t\t   !!  THE RACE IS ON  !!\n\n");
            WriteLine($"\n\n\t\t    Contesters Status\n\n\tCar Name\tCurrent Speed\tkm remaining\n");
            foreach (Car car in CarRacing.Racers.ToList())
            {
                WriteLine($"\t{car.Name}\t\t{car.Speed}Km/h\t\t" +
                    $"{(car.TimeToFinishLine / 60 / 60) * car.Speed:#.##}\n");
            }
        }
        catch (Exception)
        {

        }
    }
}
