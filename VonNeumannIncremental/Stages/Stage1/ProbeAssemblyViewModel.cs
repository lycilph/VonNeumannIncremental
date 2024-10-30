using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using VonNeumannIncremental.Core;
using VonNeumannIncremental.Stages.Common;

namespace VonNeumannIncremental.Stages.Stage1;

public partial class ProbeAssemblyViewModel(Game game, Stage1ViewModel stage1) : ViewModelBase(game)
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    [ObservableProperty]
    private int workingTicks = 0;

    [ObservableProperty]
    private int workIncrement = 25;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AssembleProbeCommand))]
    private bool isWorking = false;

    private Task? finishingTask = null;

    public override void Start()
    {
        logger.Debug("Stage 1 - Probe assembly section - started");

        Game.Timer.Tick += GameTimerTick;
        finishingTask = null;
    }

    public override void Stop()
    {
        logger.Debug("Stage 1 - Probe assembly section - stopped");
        Game.Timer.Tick -= GameTimerTick;
    }
    
    private void GameTimerTick(object? sender, EventArgs e)
    {
        if (IsWorking)
        {
            if (WorkingTicks < 100)
                WorkingTicks += WorkIncrement;

            // Target is always 100 since it is a percentage
            if (WorkingTicks >= 100 && finishingTask == null)
            {
                stage1.Write("Probe assembly done");
                stage1.Write("Heading to probe launch facility");

                finishingTask =
                    Task.Delay(TimeSpan.FromSeconds(2))
                        .ContinueWith(task => stage1.Next());
            }
        }
    }

    [RelayCommand(CanExecute = nameof(CanAssembleProbe))]
    private void AssembleProbe() => IsWorking = true;

    private bool CanAssembleProbe() => !IsWorking;
}
