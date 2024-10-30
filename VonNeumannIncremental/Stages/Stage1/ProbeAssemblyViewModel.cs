using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using VonNeumannIncremental.Core;
using VonNeumannIncremental.Stages.Common;

namespace VonNeumannIncremental.Stages.Stage1;

public partial class ProbeAssemblyViewModel(Game game, Stage1ViewModel vm) : ViewModelBase(game)
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    protected Stage1ViewModel Stage1ViewModel { get; private set; } = vm;

    [ObservableProperty]
    private int workingTicks = 0;

    [ObservableProperty]
    private int ticksToComplete = 2;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AssembleProbeCommand))]
    private bool isWorking = false;

    public override void Start()
    {
        logger.Debug("Stage 1 - Probe assembly section - started");
        Game.Timer.Tick += GameTimerTick;
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
            WorkingTicks += 5;

            if (WorkingTicks >= 100)
            {
                logger.Debug("Stage 1 - Probe assembly section - Next");

                Stage1ViewModel.Next();
            }
        }
    }

    [RelayCommand(CanExecute = nameof(CanAssembleProbe))]
    private void AssembleProbe() => IsWorking = true;

    private bool CanAssembleProbe() => !IsWorking;
}
