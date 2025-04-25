namespace Custom.Features;

public class EmptyTypeDeclaration : ITest
{
    public void Run()
    {
        Assert.AreNotEqual(null, new Test());
    }

    private class Test;
}