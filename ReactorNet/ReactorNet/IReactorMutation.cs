namespace ReactorNet
{
  public interface IReactorMutation<T>
  {
    T Mutate(T state);
  }
}