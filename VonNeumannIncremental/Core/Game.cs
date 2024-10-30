using System.Windows.Threading;
using VonNeumannIncremental.Stages.Stage1;

namespace VonNeumannIncremental.Core;

public class Game
{
    public List<Type> Stages { get; private set; } = [typeof(Stage1ViewModel)];

    public DispatcherTimer Timer { get; private set; }

    public int Ticks { get; set; }
    public int CurrentStage { get; set; } = 0;

    public Game()
    {
        Timer = new DispatcherTimer();
        Timer.Interval = TimeSpan.FromMilliseconds(250);
        Timer.Tick += GameTick;
        Timer.Start();
    }

    private void GameTick(object? sender, EventArgs e)
    {
        Ticks++;
    }
}
