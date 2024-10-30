using CommunityToolkit.Mvvm.ComponentModel;
using VonNeumannIncremental.Core;

namespace VonNeumannIncremental.Stages.Common;

public class ViewModelBase(Game game) : ObservableObject, IViewModel
{
    public Game Game { get; private set; } = game;

    public virtual void Reset() { }

    public virtual void Start() { }

    public virtual void Stop() { }
}
