namespace Custom.Features;

public class FieldKeyword : ITest
{
    public int Property
    {
        get => field;
        set => field = Mathf.Clamp(value, 0, 100);
    }

    public void Run()
    {
        Property = 142;

        Assert.AreEqual(Property, 100);
    }
}