using CommunityToolkit.Mvvm.ComponentModel;
using NLog;
using VonNeumannIncremental.Core;
using VonNeumannIncremental.Stages.Common;

namespace VonNeumannIncremental.Stages.Stage1;

public partial class Stage1ViewModel(Game game) : ObservableObject, IViewModel
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public Game Game { get; private set; } = game;

    [ObservableProperty]
    public int _ticks;

    public void Reset()
    {
    }

    public void Start()
    {
        logger.Debug("Stage 1 - started");
        Game.GameTimer.Tick += GameTimerTick;
    }

    public void Stop()
    {
        logger.Debug("Stage 1 - stopped");
        Game.GameTimer.Tick -= GameTimerTick;
    }

    private void GameTimerTick(object? sender, EventArgs e)
    {
        Ticks = Game.Ticks * 10;
    }
}
