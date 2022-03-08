using System.Security.Cryptography;
using System.Timers;

public class Car
{
#pragma warning disable CS8622
    public string RegNum { get; set; }
    public string Name { get; set; }
    public double Speed { get; set; }
    System.Timers.Timer? _raceTimer;
    static System.Timers.Timer? _obstacleTimer;
    public Car(string regNum, string name, double speed)
    {
        RegNum = regNum;
        Name = name;
        Speed = speed;
    }
    public double TimeToFinishLine;
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
        if (TimeToFinishLine > 0)
        {
            TimeToFinishLine -= 0.5;
        }
        else
        {
            WriteLine($"\n   ({Name})\tfinished on track: " +
                $"[{Thread.CurrentThread.ManagedThreadId}]");
            CarRacing.Racers.Remove(this);
            CarRacing.Winners.Add(this);
            _raceTimer.Enabled = false;
        }
        if (CarRacing.Winners.Count == 10)
        {
            CarRacing.ThreadList.Clear();
            CarRacing.PrintWinners();
        }
    }

    public static void IncidentRandomizer()
    {
        foreach (Car car in CarRacing.Racers)
        {
            car.MakeObstacle();
        }
    }

    private void MakeObstacle()
    {
        int randum = RandomNumberGenerator.GetInt32(1, 101);
        switch (randum)
        {
            case <= 2:                     // 2%
                TimeToFinishLine += 30;
                WriteLine($"\n\t({Name})\t is -stopping for refueling-");
                return;
            case <= 6:                     // 2 + 4 = 4%
                TimeToFinishLine += 20;
                WriteLine($"\n\t({Name})\t got -flat tire-");
                return;
            case <= 16:                     // 2 + 4 + 10 = 10%
                TimeToFinishLine += 10;
                WriteLine($"\n\t({Name})\t got -hit by a bird-");
                return;
            case <= 36:                     // 2 + 6 + 10 + 20 = 20%
                Speed -= 1;
                WriteLine($"\n\t({Name})\t is having an -engine failure- ");
                return;
        }
    }

    public static void IncidentRandomizerTimer()
    {
        _obstacleTimer = new System.Timers.Timer(2000);
        _obstacleTimer.Elapsed += IncidentEvent;
        _obstacleTimer.AutoReset = true;
        _obstacleTimer.Enabled = true;
    }

    private static void IncidentEvent(Object source, ElapsedEventArgs e)
    {
        if (CarRacing.Winners.Count == 10)
        {
            _obstacleTimer.Enabled = false;
        }
        IncidentRandomizer();
    }


}
