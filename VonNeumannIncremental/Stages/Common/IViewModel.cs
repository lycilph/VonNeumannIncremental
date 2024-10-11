using VonNeumannIncremental.Core;

namespace VonNeumannIncremental.Stages.Common;

public interface IViewModel 
{
    public Game Game { get; }

    public void Reset();
    public void Start();
    public void Stop();
}
