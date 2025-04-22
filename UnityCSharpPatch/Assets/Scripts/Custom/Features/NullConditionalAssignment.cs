namespace Custom.Features;

public class NullConditionalAssignment : ITest
{
    private Test? _test;

    public void Run()
    {
        _test?.Property = 100;
        _test = new Test { Property = 0 };
        _test.Property += 25;

        Assert.AreEqual(25, _test.Property);
    }

    private class Test
    {
        public int Property { get; set; }
    }
}