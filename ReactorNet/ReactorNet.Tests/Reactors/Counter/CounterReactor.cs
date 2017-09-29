using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ReactorNet.Tests.Reactors
{
  public class CounterReactor : Reactor<CounterState>
  {
    public CounterReactor(CounterState initialState, IScheduler scheduler) : base(initialState)
    {
      RegisterTrigger<Increase>((trigger) => new[] {
        Observable.Return<IReactorMutation<CounterState>>(new SetLoading(true)),
        Observable.FromAsync(DoSomethingAsync).Select<bool,IReactorMutation<CounterState>>(r => new SetLoading(r)),
        Observable.Return<IReactorMutation<CounterState>>(new IncreaseValue()).Delay(TimeSpan.FromSeconds(0.5), scheduler),
        Observable.Return<IReactorMutation<CounterState>>(new SetLoading(false))
      });
      RegisterTrigger<Decrease>((action) => new[] { Observable.Return<IReactorMutation<CounterState>>(new DecreaseValue()) });
    }

    private Task<bool> DoSomethingAsync() => Task.FromResult(true);
  }
}