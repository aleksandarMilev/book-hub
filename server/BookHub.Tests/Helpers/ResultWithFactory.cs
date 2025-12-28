namespace BookHub.Tests.Helpers;

using System.Reflection;
using FluentAssertions;
using Infrastructure.Services.Result;

public static class ResultWithFactory
{
    public static ResultWith<T> Success<T>(T value)
        => InvokeFactory<T>("Success", value);

    public static ResultWith<T> Failure<T>(string error)
        => InvokeFactory<T>("Failure", error);

    private static ResultWith<T> InvokeFactory<T>(
        string methodName,
        object? arg)
    {
        var typeOfResult = typeof(ResultWith<T>);
        var methods = typeOfResult.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => string.Equals(m.Name, methodName, StringComparison.Ordinal))
            .ToArray();

        foreach (var method in methods)
        {
            var parameters = method.GetParameters();
            if (parameters.Length == 1)
            {
                try
                {
                    var result = method.Invoke(null, [arg]);
                    if (result is ResultWith<T> typed) 
                    {
                        return typed;
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message);
                }
            }
        }

        var constructors = typeOfResult.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        foreach (var constructor in constructors)
        {
            var parameter = constructor.GetParameters();
            if (methodName == "Success")
            {
                if (parameter.Length == 1 && parameter[0].ParameterType.IsAssignableFrom(typeof(T)))
                {
                    return (ResultWith<T>)constructor.Invoke([(T)arg!]);
                }
            }
            else
            {
                if (parameter.Length == 1 && parameter[0].ParameterType == typeof(string))
                {
                    return (ResultWith<T>)constructor.Invoke([(string)arg!]);
                }
            }
        }

        throw new InvalidOperationException(
            $"Could not create a '{typeOfResult.FullName}' via static '{methodName}' or known constructors. Update {nameof(ResultWithFactory)}.");
    }
}
