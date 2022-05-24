
namespace Game2048.Library;

public class Unsubscriber : IDisposable
{
    private readonly List<IObserver<IGameCore>> _observers;
    private readonly IObserver<IGameCore> _observer;

    public Unsubscriber(List<IObserver<IGameCore>> observers, IObserver<IGameCore> observer)
    {
        _observers = observers;
        _observer = observer;
    }

    public void Dispose()
    {
        if (_observers.Contains(_observer))
        {
            _observers.Remove(_observer);
        }
    }
}
