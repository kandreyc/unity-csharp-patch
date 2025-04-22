namespace Custom.Features;

public class SimpleLambdaParameters : ITest
{
    private delegate bool TryParse<T>(string text, out T result);

    public void Run()
    {
        TryParse<int> parser = (text, out result) => int.TryParse(text, out result);

        Assert.AreEqual(true, parser("42", out var value));
        Assert.AreEqual(42, value);
    }
}