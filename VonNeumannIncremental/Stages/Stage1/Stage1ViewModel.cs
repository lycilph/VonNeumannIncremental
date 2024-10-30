using CommunityToolkit.Mvvm.ComponentModel;
using NLog;
using System.Collections.ObjectModel;
using VonNeumannIncremental.Core;
using VonNeumannIncremental.Stages.Common;

namespace VonNeumannIncremental.Stages.Stage1;

public partial class Stage1ViewModel(Game game) : ViewModelBase(game)
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    [ObservableProperty]
    private IViewModel? currentSection;

    [ObservableProperty]
    private ObservableCollection<string> messages = [];

    public void Next()
    {
        if (CurrentSection is PartsConstructionViewModel)
        {
            var section = new ProbeAssemblyViewModel(Game, this);
            section.Reset();

            CurrentSection = section;
        }
        else
            CurrentSection = null;
    }

    public override void Reset()
    {
        var section = new PartsConstructionViewModel(Game, this);
        section.Reset();

        CurrentSection = section;
    }

    public override void Start()
    {
        logger.Debug("Stage 1 - started");

        Messages.Add("Stage 1 - started");
    }

    public override void Stop()
    {
        logger.Debug("Stage 1 - stopped");
    }

    partial void OnCurrentSectionChanging(IViewModel? oldValue, IViewModel? newValue)
    {
        logger.Debug("Changing section from {from} to {to}", oldValue?.GetType().Name, newValue?.GetType().Name);

        if (oldValue is not null)
            oldValue.Stop();
    }

    partial void OnCurrentSectionChanged(IViewModel? value)
    {
        if (value is not null)
            value.Start();
    }
}
