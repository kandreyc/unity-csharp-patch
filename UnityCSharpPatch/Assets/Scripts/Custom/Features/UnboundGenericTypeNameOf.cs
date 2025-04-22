using System.Collections.Generic;

namespace Custom.Features;

public class UnboundGenericTypeNameOf : ITest
{
    public void Run()
    {
        Assert.AreEqual("List", nameof(List<>));
    }
}