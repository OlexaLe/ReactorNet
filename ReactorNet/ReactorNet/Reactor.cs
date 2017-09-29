using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ReactorNet
{
  public abstract class Reactor<T> : INotifyPropertyChanged, IDisposable
  {
    private readonly Dictionary<Type, Func<IReactorTrigger, IEnumerable<IObservable<IReactorMutation<T>>>>> _mutations = new Dictionary<Type, Func<IReactorTrigger, IEnumerable<IObservable<IReactorMutation<T>>>>>();
    private Subject<IReactorMutation<T>> _reducerQueue;
    private T _state;

    public Reactor(T initialState)
    {
      State = initialState;
      _reducerQueue = new Subject<IReactorMutation<T>>();
      _reducerQueue.Subscribe(ProcessMutation, (exception) => OnError?.Invoke(this, exception));
    }

    public event EventHandler<Exception> OnError;

    public event PropertyChangedEventHandler PropertyChanged;

    public T State
    {
      get { return _state; }
      private set { _state = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State))); }
    }

    public IDisposable Dispatch(IReactorTrigger trigger)
    {
      try
      {
        var triggerType = trigger.GetType();
        _mutations.TryGetValue(triggerType, out Func<IReactorTrigger, IEnumerable<IObservable<IReactorMutation<T>>>> mutation);
        if (mutation == null)
          throw new TriggerNotFoundException(triggerType);
        return mutation
          .Invoke(trigger)
          .Concat()
          .Subscribe((m) => _reducerQueue.OnNext(m), (exception) => OnError?.Invoke(this, exception));
      }
      catch (Exception e)
      {
        OnError.Invoke(this, e);
      }
      return null;
    }

    protected void RegisterTrigger<TAction>(Func<IReactorTrigger, IEnumerable<IObservable<IReactorMutation<T>>>> mutations) where TAction : IReactorTrigger
          => _mutations.Add(typeof(TAction), mutations);

    private void ProcessMutation(IReactorMutation<T> mutation)
    {
      try
      {
        State = mutation.Mutate(State);
      }
      catch (Exception e)
      {
        OnError.Invoke(this, e);
      }
    }

    #region IDisposable Support

    private bool disposedValue = false;

    public void Dispose()
    {
      Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          _reducerQueue.Dispose();
        }
        disposedValue = true;
      }
    }

    #endregion IDisposable Support
  }
}