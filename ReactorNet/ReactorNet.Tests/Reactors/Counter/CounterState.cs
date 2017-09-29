using System.Diagnostics;

namespace ReactorNet.Tests.Reactors
{
  public class CounterState
  {
    public CounterState(int value, bool isLoading)
    {
      Value = value;
      IsLoading = isLoading;
    }

    public bool IsLoading { get; }
    public int Value { get; }
  }
}