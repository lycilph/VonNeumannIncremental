using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using VonNeumannIncremental.Core;
using VonNeumannIncremental.Stages.Common;

namespace VonNeumannIncremental.Stages.Stage1;

public partial class PartsConstructionViewModel(Game game, Stage1ViewModel vm) : ViewModelBase(game)
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    protected Stage1ViewModel Stage1ViewModel { get; private set; } = vm;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(PrintPartCommand))]
    private int parts = 0;

    [ObservableProperty]
    private int partsToComplete = 10;

    [ObservableProperty]
    private int workingTicks = 0;

    [ObservableProperty]
    private int ticksToComplete = 2;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(PrintPartCommand))]
    private bool isWorking = false;

    public override void Start()
    {
        logger.Debug("Stage 1 - Parts construction section - started");
        Game.Timer.Tick += GameTimerTick;
    }

    public override void Stop()
    {
        logger.Debug("Stage 1 - Parts construction section - stopped");
        Game.Timer.Tick -= GameTimerTick;
    }

    private void GameTimerTick(object? sender, EventArgs e)
    {
        if (IsWorking)
        {
            WorkingTicks++;

            if (WorkingTicks >= TicksToComplete)
            {
                IsWorking = false;
                WorkingTicks = 0;
                Parts++;
            }
        }

        if (Parts >= PartsToComplete)
        {
            logger.Debug("Stage 1 - Parts construction section - Next");

            Stage1ViewModel.Next();
        }
    }

    [RelayCommand(CanExecute = nameof(CanPrintPart))]
    private void PrintPart()
    {
        IsWorking = true;
        vm.Messages.Add($"Printing Part... {Parts+1}");
    }

    private bool CanPrintPart() => !IsWorking && Parts < PartsToComplete;
}
