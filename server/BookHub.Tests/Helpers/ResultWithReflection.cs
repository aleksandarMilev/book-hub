namespace BookHub.Tests.Helpers;

using System.Collections;
using System.Reflection;

public static class ResultWithReflection
{
    public static bool IsSuccess(object result)
    {
        ArgumentNullException.ThrowIfNull(result);

        var typeOfResult = result.GetType();
        var propNames = new[]
        {
            "Succeeded", "IsSuccess", "Success", "IsSuccessful", "Ok"
        };

        foreach (var propName in propNames)
        {
            var property = typeOfResult.GetProperty(
                propName,
                BindingFlags.Public | BindingFlags.Instance);

            if (property?.PropertyType == typeof(bool))
            {
                return (bool)property.GetValue(result)!;
            }
        }

        var errorsProp = typeOfResult.GetProperty(
            "Errors",
            BindingFlags.Public | BindingFlags.Instance)
            ?? typeOfResult.GetProperty(
                "ErrorMessages",
                BindingFlags.Public | BindingFlags.Instance);

        if (errorsProp is not null)
        {
            var errorsValue = errorsProp.GetValue(result);
            if (errorsValue is IEnumerable enumerable)
            {
                foreach (var _ in enumerable)
                {
                    return false;
                }

                return true;
            }
        }

        throw new InvalidOperationException(
            $"Could not determine success flag for result type '{typeOfResult.FullName}'. Add a mapping in {nameof(ResultWithReflection)}.");
    }

    public static T? GetValue<T>(object result)
    {
        ArgumentNullException.ThrowIfNull(result);

        var typeOfResult = result.GetType();
        var propNames = new[]
        {
            "Value", "Data", "Result", "Payload", "Model"
        };

        foreach (var propName in propNames)
        {
            var property = typeOfResult.GetProperty(
                propName,
                BindingFlags.Public | BindingFlags.Instance);

            if (property is not null && typeof(T).IsAssignableFrom(property.PropertyType))
            {
                return (T?)property.GetValue(result);
            }
        }

        foreach (var propName in propNames)
        {
            var property = typeOfResult.GetProperty(
                propName,
                BindingFlags.Public | BindingFlags.Instance);

            if (property is not null)
            {
                var value = property.GetValue(result);
                if (value is T typedValue) 
                {
                    return typedValue;
                }
            }
        }

        return default;
    }

    public static string? GetError(object result)
    {
        ArgumentNullException.ThrowIfNull(result);

        var typeOfResult = result.GetType();
        var propNames = new[]
        {
            "Error", "ErrorMessage", "Message", "FailureMessage"
        };

        foreach (var name in propNames)
        {
            var property = typeOfResult.GetProperty(
                name,
                BindingFlags.Public | BindingFlags.Instance);

            if (property?.PropertyType == typeof(string))
            {
                return (string?)property.GetValue(result);
            }
        }

        var errorsProp = typeOfResult.GetProperty(
            "Errors",
            BindingFlags.Public | BindingFlags.Instance)
            ?? typeOfResult.GetProperty(
                "ErrorMessages",
                BindingFlags.Public | BindingFlags.Instance);

        if (errorsProp is not null)
        {
            var errorsValue = errorsProp.GetValue(result);
            if (errorsValue is IEnumerable enumerable)
            {
                var parts = enumerable.Cast<object?>()
                    .Select(x => x?.ToString())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToArray();

                if (parts.Length > 0)
                {
                    return string.Join("; ", parts);
                }
            }
        }

        return null;
    }
}
