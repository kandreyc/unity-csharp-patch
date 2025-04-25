namespace Custom.Features;

public class Constructor : ITest
{
    public void Run()
    {
        var test = new Test(42, "42");

        Assert.AreEqual(42, test.A);
        Assert.AreEqual("42", test.B);
    }

    private class Test(int a, string b)
    {
        public int A => a;
        public string B => b;
    }
}

