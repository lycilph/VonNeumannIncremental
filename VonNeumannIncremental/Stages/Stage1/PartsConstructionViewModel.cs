using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using VonNeumannIncremental.Core;
using VonNeumannIncremental.Stages.Common;

namespace VonNeumannIncremental.Stages.Stage1;

public partial class PartsConstructionViewModel(Game game, Stage1ViewModel stage1) : ViewModelBase(game)
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(PrintPartCommand))]
    private int parts = 0;

    [ObservableProperty]
    private int partsToComplete = 2;

    [ObservableProperty]
    private int workingTicks = 0;

    [ObservableProperty]
    private int ticksToComplete = 2;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(PrintPartCommand))]
    private bool isWorking = false;

    private Task? finishingTask = null;

    public override void Start()
    {
        logger.Debug("Stage 1 - Parts construction section - started");

        Game.Timer.Tick += GameTimerTick;
        finishingTask = null;
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

        if (Parts >= PartsToComplete && finishingTask == null)
        {
            stage1.Write("Probe parts construction done");
            stage1.Write("Heading over to probe assembly");

            finishingTask = 
                Task.Delay(TimeSpan.FromSeconds(2))
                    .ContinueWith(task => stage1.Next());
        }
    }

    [RelayCommand(CanExecute = nameof(CanPrintPart))]
    private void PrintPart()
    {
        IsWorking = true;
        stage1.Write($"Printing Part... {Parts+1}");
    }

    private bool CanPrintPart() => !IsWorking && Parts < PartsToComplete;
}
