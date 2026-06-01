namespace _19._05._26
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            UiTheme.Enable();
            LoginForm loginForm = new();
            UiTheme.Apply(loginForm);
            Application.Run(loginForm);
        }
    }
}