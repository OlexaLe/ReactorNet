using NUnit.Framework;
using ReactorNet.Tests.Reactors;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;

namespace ReactorNet.Tests
{
  [TestFixture]
  public class CounterReactorTests
  {
    private CounterReactor reactor;

    [Test]
    public void Decrease()
    {
      // Arrange
      // Act
      reactor.Dispatch(new Decrease());

      // Assert
      Assert.AreEqual(-1, reactor.State.Value);
    }

    [Test]
    public void Increase()
    {
      // Arrange
      // Act
      reactor.Dispatch(new Increase());

      // Assert
      Assert.AreEqual(1, reactor.State.Value);
    }

    [Test]
    public void IsLoadingTriggeredOnIncrease()
    {
      // Arrange
      var actions = new List<CounterState>();
      reactor.PropertyChanged += (s, e) => actions.Add(reactor.State);

      // Act
      reactor.Dispatch(new Increase());

      // Assert
      Assert.True(actions.First().IsLoading);
      Assert.False(actions.Last().IsLoading);
    }

    [SetUp]
    public void SetUp()
    {
      var initialState = new CounterState(0, false);
      reactor = new CounterReactor(initialState, Scheduler.Immediate);
      reactor.PropertyChanged += (s, p) => Debug.WriteLine(reactor.State.ToString());
    }
  }
}