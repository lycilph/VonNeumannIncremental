using System.Windows.Threading;

namespace VonNeumannIncremental.Core;

public class Game
{
    public DispatcherTimer GameTimer { get; private set; }

    public int Ticks { get; set; }
    public int CurrentStage { get; set; } = 1;

    public Game()
    {
        GameTimer = new DispatcherTimer();
        GameTimer.Interval = TimeSpan.FromMilliseconds(250);
        GameTimer.Tick += GameTick;
        GameTimer.Start();
    }

    private void GameTick(object? sender, EventArgs e)
    {
        Ticks++;
    }
}
