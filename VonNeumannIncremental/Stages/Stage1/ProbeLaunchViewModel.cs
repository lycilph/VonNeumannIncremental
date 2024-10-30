using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using VonNeumannIncremental.Core;
using VonNeumannIncremental.Stages.Common;

namespace VonNeumannIncremental.Stages.Stage1;

public partial class ProbeLaunchViewModel(Game game, Stage1ViewModel stage1) : ViewModelBase(game)
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LaunchProbeCommand))]
    private bool isWorking = false;

    private List<string> text =
        ["Fuelling",
         "Moving probe to pad",
         "Counting down",
         "10", "9", "8", "7", "6", "5", "4", "3", "2", "1",
         "Launch"];

    private int tick = 0;
    private int tickDelay = 2;
    private int currentMessageIndex = 0;
    private Task? finishingTask = null;

    public override void Start()
    {
        logger.Debug("Stage 1 - Probe launch section - started");

        Game.Timer.Tick += GameTimerTick;
        finishingTask = null;
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

            if (tick >= tickDelay && currentMessageIndex < text.Count)
            {
                tick = 0;
                stage1.Write(text[currentMessageIndex++]);
            }

            if (currentMessageIndex == text.Count && finishingTask == null)
            {
                stage1.Write("Probe Launched");

                finishingTask =
                    Task.Delay(TimeSpan.FromSeconds(2))
                        .ContinueWith(task => stage1.Next());
            }
        }
    }

    [RelayCommand(CanExecute = nameof(CanLaunchProbe))]
    private void LaunchProbe() => IsWorking = true;
    
    private bool CanLaunchProbe() => !IsWorking;

}
