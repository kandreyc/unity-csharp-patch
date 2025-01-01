namespace FeatureA.Tests;

public partial class TestPartialPropertyField
{
    partial int Property { get; set; }
}

public partial class TestPartialPropertyField
{
    partial int Property
    {
        get => 0;
        set { }
    }
}