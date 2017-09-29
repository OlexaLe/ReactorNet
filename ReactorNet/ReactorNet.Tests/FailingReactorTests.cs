using NUnit.Framework;
using System;
using System.Reactive.Linq;

namespace ReactorNet.Tests
{
  public class FailingMutation : IReactorMutation<object>
  {
    public object Mutate(object state) => throw new Exception();
  }

  public class FailingMutationTrigger : IReactorTrigger { }

  [TestFixture]
  public class FailingReactorTests
  {
    private Exception expectedException;
    private FooReactor reactor;

    [Test]
    public void DisposedReactor_DoNotHandle()
    {
      // Arrange

      // Act
      reactor.Dispose();
      reactor.Dispatch(new FailingMutationTrigger());

      // Assert
      Assert.IsInstanceOf<ObjectDisposedException>(expectedException);
    }

    [Test]
    public void FailingMutation_Fails()
    {
      // Arrange
      // Act
      reactor.Dispatch(new FailingMutationTrigger());

      // Assert
      Assert.NotNull(expectedException);
    }

    [Test]
    public void NonExistingTrigger_Fails()
    {
      // Arrange
      // Act
      reactor.Dispatch(new UnknownTrigger());

      // Act
      Assert.IsInstanceOf<TriggerNotFoundException>(expectedException);
    }

    [SetUp]
    public void SetUp()
    {
      reactor = new FooReactor(new object());
      reactor.OnError += (s, e) => expectedException = e;
    }
  }

  public class FooReactor : Reactor<object>
  {
    public FooReactor(object initialState) : base(initialState)
    {
      RegisterTrigger<FailingMutationTrigger>((t) => new[] { Observable.Return(new FailingMutation()) });
    }
  }

  public class UnknownTrigger : IReactorTrigger { }
}