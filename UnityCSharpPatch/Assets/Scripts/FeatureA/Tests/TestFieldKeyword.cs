namespace FeatureA.Tests;

public class TestFieldKeyword
{
    public int NewProperty
    {
        get => field;
        set => field = Mathf.Clamp(value, 0, 100);
    }
}