namespace Custom.Features;

public class ImplicitSpanConversions : ITest
{
    public void Run()
    {
        int[] arr = [42];

        Assert.AreEqual(42, arr.First());
    }
}

public static class ImplicitSpanConversionsExtensions
{
    public static T First<T>(this ReadOnlySpan<T> span)
    {
        return span[0];
    }
}