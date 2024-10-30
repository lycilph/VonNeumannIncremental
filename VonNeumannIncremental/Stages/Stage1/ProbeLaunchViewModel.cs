using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using System.Collections.ObjectModel;
using VonNeumannIncremental.Core;
using VonNeumannIncremental.Stages.Common;

namespace VonNeumannIncremental.Stages.Stage1;

public partial class ProbeLaunchViewModel(Game game, Stage1ViewModel vm) : ViewModelBase(game)
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    protected Stage1ViewModel Stage1ViewModel { get; private set; } = vm;
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LaunchProbeCommand))]
    private bool isWorking = false;

    [ObservableProperty]
    private ObservableCollection<string> messages = [];

    private List<string> text =
        ["Fuelling", ".", ".", ".",
         "Moving probe to pad", ".", ".", ".",
         "Counting down",
         "10", "9", "8", "7", "6", "5", "4", "3", "2", "1",
         "Launch"];

    private int tick = 0;
    private int tickDelay = 10;

    public override void Start()
    {
        logger.Debug("Stage 1 - Probe launch section - started");
        Game.Timer.Tick += GameTimerTick;
    }

    public override void Stop()
    {
        logger.Debug("Stage 1 - Probe launch section - stopped");
        Game.Timer.Tick -= GameTimerTick;
    }

    private void GameTimerTick(object? sender, EventArgs e)
    {
        if (IsWorking) 
        {
            tick++;
            if (tick >= 10)
            {
                tick = 0;
            }
        }
    }

    [RelayCommand(CanExecute = nameof(CanLaunchProbe))]
    private void LaunchProbe() => IsWorking = true;
    
    private bool CanLaunchProbe() => !IsWorking;

}
