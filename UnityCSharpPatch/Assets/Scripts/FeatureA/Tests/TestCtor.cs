namespace FeatureA.Tests;

public class TestCtor(ILogger a, string b)
{
    public void Method()
    {
        a.Log(b);
    }
}