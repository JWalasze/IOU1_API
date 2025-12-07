using IOU1.Application.Features.Users.Models.Endpoint;
using IOU1.Domain.Entities;
using System.Reflection;

namespace IOU1.Application.Features.Users;

public class UserMapper
{
    public T Map<T>(object source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var srcType = source.GetType();
        var destType = typeof(T);

        // Find non-generic Map(X) that returns T (or subtype), with one parameter assignable from srcType
        var method = GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => m.Name == "Map"
                        && !m.IsGenericMethodDefinition   // exclude Map<T>(object)
                        && m.GetParameters().Length == 1
                        && m.ReturnType != typeof(void)
                        && m.GetParameters()[0].ParameterType.IsAssignableFrom(srcType)
                        && destType.IsAssignableFrom(m.ReturnType))
            // prefer the most specific parameter match
            .OrderByDescending(m => m.GetParameters()[0].ParameterType == srcType)
            .FirstOrDefault()
            ?? throw new MissingMethodException(
                $"No Map({srcType.Name}) -> {destType.Name} found on {GetType().Name}.");

        return (T)method.Invoke(this, new[] { source })!;
    }

    // your specific mapper
    private AddUserResponse Map(User user)
    {
        return new()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email.EmailAddress,
            Login = user.Login,
        };
    }
}
