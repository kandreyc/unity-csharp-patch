namespace Custom.Features;

public partial class PartialProperty : ITest
{
    partial int Property { get; set; }

    public void Run()
    {
        Property = 142;

        Assert.AreEqual(100, Property);
    }
}

public partial class PartialProperty
{
    partial int Property
    {
        get;
        set => field = Mathf.Clamp(value, 0, 100);
    }
}