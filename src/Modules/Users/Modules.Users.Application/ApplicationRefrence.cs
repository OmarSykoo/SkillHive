using System.Reflection;

namespace Modules.Users.Application;

public static class ApplicationRefrence
{
    public static Assembly Assembly => typeof(ApplicationRefrence).Assembly;
}
