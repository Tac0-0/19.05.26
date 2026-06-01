using Doner.Data;

namespace Doner.Test.Helpers;

internal static class ControllerExceptionAssertions
{
    public static DonerDBContext ThrowContextCreation() =>
        throw new InvalidOperationException("context creation failed");

    public static void AssertContextCreationFailure(params Func<Task>[] actions)
    {
        foreach (Func<Task> action in actions)
        {
            Assert.That((Func<Task>)(async () => await action()), Throws.TypeOf<InvalidOperationException>()
                .With.Message.EqualTo("context creation failed"));
        }
    }
}
