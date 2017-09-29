# ReactorNet
ReactorNet is a framework for a reactive and unidirection domain layer for your app. This repository introduces the basic concept of Reactor and contains a sample reactor with example of use via unit tests.

## Goals
* **Be Small**: ReactorNet doesn't require the whole application to follow a single architecture. Use it only when you need it.
* **Be Testable**: The purpose of ReactorNet is to separate your ViewModels from the business logic.

## Use manual
#### Create Store
Store is a POCO. It might be class or even something as simple as string.

#### Create Trigger(s)
Trigger is a class or a struct that conforms to ```IReactorTrigger``` interface, like: 

```csharp
public struct Increase : IReactorTrigger { }
```

#### Create Mutator(s)
Mutator is a class or a struct that conforms to ```IReactorMutation<T>``` interface, like:
```csharp
public struct IncreaseValue : IReactorMutation<CounterState>
{
  public CounterState Mutate(CounterState state) => new CounterState(state.Value + 1, state.IsLoading);
}
```

### Create Reactor
Reactor should inherit from ```Reactor<T>``` abstract class. Trigger(s) registration should be places here, like:
```csharp
public class CounterReactor : Reactor<CounterState>
{
  public CounterReactor(CounterState initialState, IScheduler scheduler) : base(initialState)
  {
    RegisterTrigger<Increase>((trigger) => new[] {
        Observable.Return<IReactorMutation<CounterState>>(new IncreaseValue()).Delay(TimeSpan.FromSeconds(0.5), scheduler),
    });
  }
}
```

### PROFIT
Now use your reactor like that:
```csharp
reactor.Dispatch(new Increase());
```

### Side-effects?
Whant to add logging? Or observe any States update? Just observe State change cause Reactor supports INotifyPropertyChanges.
