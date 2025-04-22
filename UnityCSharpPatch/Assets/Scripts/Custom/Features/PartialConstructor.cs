namespace Custom.Features;

public class PartialConstructor : ITest
{
    public void Run()
    {
        Assert.AreEqual(42, new Test(42).Value);
    }

    private partial class Test
    {
        public partial Test(int value);
    }

    private partial class Test
    {
        public int Value { get; }

        public partial Test(int value)
        {
            Value = value;
        }
    }
}