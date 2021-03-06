using System.IO;
using System.Reflection;

namespace Base.Helper;

public static class DllHelper
{
    public static Assembly[] GetHotfixAssembly(GameServer game)
    {
        Assembly[] assembly =
        {
            typeof(GameServer).Assembly,
            game.GetType().Assembly,
            Assembly.Load(File.ReadAllBytes($"./{game.Role}.Hotfix.dll"),
                File.ReadAllBytes($"./{game.Role}.Hotfix.pdb"))
        };

        return assembly;
    }
}