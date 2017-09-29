namespace ReactorNet.Tests.Reactors
{
  public struct SetLoading : IReactorMutation<CounterState>
  {
    private bool isLoading;

    public SetLoading(bool isLoading)
    {
      this.isLoading = isLoading;
    }

    public CounterState Mutate(CounterState state) => new CounterState(state.Value, isLoading);
  }
}