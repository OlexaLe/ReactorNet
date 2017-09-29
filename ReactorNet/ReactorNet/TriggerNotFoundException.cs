using System;

namespace ReactorNet
{
  public class TriggerNotFoundException : Exception
  {
    public TriggerNotFoundException(Type triggerType) : base($"{triggerType.ToString()} is not registered in Reactor")
    {
    }
  }
}