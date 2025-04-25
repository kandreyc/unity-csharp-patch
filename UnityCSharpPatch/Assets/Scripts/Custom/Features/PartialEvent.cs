namespace Custom.Features;

public partial class PartialEvent : ITest
{
    public partial event Action TestEvent;

    public void Run()
    {
        var calledTimes = 0;
        TestEvent += () => calledTimes++;

        InvokeTestEvent();

        Assert.AreEqual(1, calledTimes);
    }
}

public partial class PartialEvent
{
    private Action? _actions = delegate { };

    public partial event Action TestEvent
    {
        add => _actions += value;
        remove => _actions -= value;
    }

    public void InvokeTestEvent()
    {
        _actions?.Invoke();
    }
}