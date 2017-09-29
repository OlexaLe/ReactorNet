namespace ReactorNet.Tests.Reactors
{
  public struct DecreaseValue : IReactorMutation<CounterState>
  {
    public CounterState Mutate(CounterState state) => new CounterState(state.Value - 1, state.IsLoading);
  }
}