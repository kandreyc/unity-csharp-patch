namespace Custom.Features;

public class ExtensionBlock : ITest
{
    public void Run()
    {
        Assert.AreEqual(42, this.Count);
    }
}

public static class TestExtensionBlockExtensions
{
    extension(ExtensionBlock block)
    {
        public int Count => 42;
    }
}