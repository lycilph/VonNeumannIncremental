using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VonNeumannIncremental.Core;
using VonNeumannIncremental.Stages.Common;
using VonNeumannIncremental.Stages.Stage1;

namespace VonNeumannIncremental;

public partial class MainWindowViewModel(Game game) : ObservableObject, IViewModel
{
    public Game Game { get; private set; } = game;

    [ObservableProperty]
    public IViewModel? _currentStage;

    [ObservableProperty]
    public int _ticks;

    public void Reset()
    {
        CurrentStage = new Stage1ViewModel(Game);
        CurrentStage.Reset();
    }

    public void Start() 
    {
        Game.GameTimer.Tick += GameTimerTick;
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
        if (oldValue is not null)
            oldValue.Stop();
    }

    partial void OnCurrentStageChanged(IViewModel? value)
    {
        if (value is not null)
            value.Start();
    }

    [RelayCommand]
    private void StopTimer()
    {
        CurrentStage?.Stop();
    }
}
