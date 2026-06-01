using Doner.Data;

namespace Doner.Controller;

public abstract class DbController
{
    private readonly Func<DonerDBContext> createContext;

    protected DbController(Func<DonerDBContext>? createContext = null)
    {
        this.createContext = createContext ?? (() => new DonerDBContext());
    }

    protected DonerDBContext CreateContext()
    {
        return createContext();
    }
}
