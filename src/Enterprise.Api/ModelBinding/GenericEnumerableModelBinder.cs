using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Reflection;

namespace Enterprise.Api.ModelBinding;

/// <summary>
/// A custom model binder for binding comma-separated string values to enumerable types such as IEnumerable&lt;T&gt; and List&lt;T&gt;.
/// </summary>
public class GenericEnumerableModelBinder : IModelBinder
{
    /// <summary>
    /// Asynchronously binds an incoming value to an enumerable model.
    /// </summary>
    /// <param name="bindingContext">The context for the model binding.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        // Check if the model metadata represents an enumerable type.
        ModelMetadata modelMetadata = bindingContext.ModelMetadata;

        if (!modelMetadata.IsEnumerableType)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        // Retrieve the value to be bound from the value provider using the model name.
        ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        // Determine the element type of the enumerable and get the corresponding type converter.
        Type elementType = modelMetadata.ElementType ?? modelMetadata.ModelType.GenericTypeArguments[0];
        TypeConverter converter = TypeDescriptor.GetConverter(elementType);
        
        if (!converter.CanConvertFrom(typeof(string)))
        {
            // Fail the binding process if no suitable converter is found.
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        try
        {
            // Determine if the target model is a collection type like List<T>.
            Type listType = typeof(List<>).MakeGenericType(elementType);

            if (bindingContext.ModelMetadata.IsCollectionType)
            {
                BindCollectionType(bindingContext, listType, valueProviderResult, converter);
            }
            else
            {
                bindingContext.Model = BindNonCollectionType(valueProviderResult, elementType, converter);
            }

            // Successfully bind the model.
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
        }
        catch (Exception)
        {
            // Log or handle the exception and fail the binding process.
            bindingContext.Result = ModelBindingResult.Failed();
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Binds a collection enumerable type by splitting comma-separated values and converting them to the element type.
    /// </summary>
    /// <param name="bindingContext"></param>
    /// <param name="listType"></param>
    /// <param name="valueProviderResult"></param>
    /// <param name="converter"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private void BindCollectionType(ModelBindingContext bindingContext, Type listType, ValueProviderResult valueProviderResult,
        TypeConverter converter)
    {
        bindingContext.Model = listType.IsAssignableFrom(bindingContext.ModelType)
            ? BindListType(valueProviderResult, listType, converter)
            : throw new InvalidOperationException("Unsupported collection type");
    }

    /// <summary>
    /// Binds a non-collection enumerable type by splitting comma-separated values and converting them to the element type.
    /// </summary>
    /// <param name="valueProviderResult">The value provider result containing the comma-separated values.</param>
    /// <param name="elementType">The type of the elements in the enumerable.</param>
    /// <param name="converter">The type converter for the element type.</param>
    /// <returns>An array of the bound elements.</returns>
    private Array BindNonCollectionType(ValueProviderResult valueProviderResult, Type elementType, TypeConverter converter)
    {
        object?[] values = valueProviderResult.Values
            .SelectMany(SplitString)
            .Select(v => converter.ConvertFromString(v.Trim()))
            .ToArray();

        var typedValues = Array.CreateInstance(elementType, values.Length);
        values.CopyTo(typedValues, 0);

        return typedValues;
    }

    /// <summary>
    /// Binds a collection type like List&lt;T&gt; by splitting comma-separated values and converting them to the element type.
    /// </summary>
    /// <param name="valueProviderResult">The value provider result containing the comma-separated values.</param>
    /// <param name="listType">The type of the list to be bound.</param>
    /// <param name="converter">The type converter for the element type.</param>
    /// <returns>A list of the bound elements.</returns>
    private object BindListType(ValueProviderResult valueProviderResult, Type listType, TypeConverter converter)
    {
        object list = Activator.CreateInstance(listType)
                      ?? throw new InvalidOperationException("Failed to create list instance.");

        MethodInfo? addMethod = listType.GetMethod(nameof(List<object>.Add));

        foreach (string? value in valueProviderResult.Values)
        {
            string[] splitValues = SplitString(value);

            foreach (string splitValue in splitValues)
            {
                if (string.IsNullOrWhiteSpace(splitValue))
                {
                    continue;
                }

                object? convertedValue = converter.ConvertFromString(splitValue.Trim());
                addMethod?.Invoke(list, [convertedValue]);
            }
        }

        return list;
    }

    /// <summary>
    /// Splits a string by commas into an array of strings, removing empty entries.
    /// </summary>
    /// <param name="input">The string to be split.</param>
    /// <returns>An array of split strings.</returns>
    private string[] SplitString(string? input)
    {
        return string.IsNullOrEmpty(input) ? [] : input.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
    }

    private static readonly char[] Separator = [','];
}
