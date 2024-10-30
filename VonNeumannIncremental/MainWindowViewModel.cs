using CommunityToolkit.Mvvm.ComponentModel;
using NLog;
using VonNeumannIncremental.Core;
using VonNeumannIncremental.Stages.Common;

namespace VonNeumannIncremental;

public partial class MainWindowViewModel(Game game) : ObservableObject, IViewModel
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public Game Game { get; private set; } = game;

    [ObservableProperty]
    private IViewModel? currentStage;

    [ObservableProperty]
    private int ticks;

    public void Reset()
    {
        var stageType = Game.Stages[Game.CurrentStage];
        var Stage = Activator.CreateInstance(stageType, Game) as IViewModel;
        Stage?.Reset();

        CurrentStage = Stage;
    }

    public void Start() 
    {
        Game.Timer.Tick += GameTimerTick;
    }

    public void Stop() 
    {
    }

    private void GameTimerTick(object? sender, EventArgs e)
    {
        Ticks = Game.Ticks;
    }

    partial void OnCurrentStageChanging(IViewModel? oldValue, IViewModel? newValue)
    {
        logger.Debug("Changing stage from {from_stage} to {to_stage}", oldValue?.GetType().Name, newValue?.GetType().Name);

        if (oldValue is not null)
            oldValue.Stop();
    }

    partial void OnCurrentStageChanged(IViewModel? value)
    {
        if (value is not null)
            value.Start();
    }
}
