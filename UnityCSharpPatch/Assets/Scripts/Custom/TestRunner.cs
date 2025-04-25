using Custom.Features;
using UnityEngine.UI;

namespace Custom;

public class TestRunner : MonoBehaviour
{
    [SerializeField] private Text? _text;

    private readonly Type[] _tests =
    [
        typeof(Constructor),
        typeof(EmptyTypeDeclaration),
        typeof(ExtensionBlock),
        typeof(FieldKeyword),
        typeof(ImplicitSpanConversions),
        typeof(NullConditionalAssignment),
        typeof(PartialConstructor),
        typeof(PartialEvent),
        typeof(PartialProperty),
        typeof(SimpleLambdaParameters),
        typeof(UnboundGenericTypeNameOf)
    ];

    private IEnumerator Start()
    {
        _text!.text = string.Empty;

        for (var index = 0; index < _tests.Length; index++)
        {
            var testType = _tests[index];
            _text.text += $"[{index + 1}/{_tests.Length}] Start test {testType.Name}\n";

            var test = (ITest)Activator.CreateInstance(testType);
            test.Run();

            var str = $"[{index + 1}/{_tests.Length}] Test {testType.Name} passed";

            _text.text += $"{str}\n\n";
            Debug.Log(str);

            yield return null;
        }

        _text.text += "All tests passed";
    }
}