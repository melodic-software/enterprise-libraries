using System.Text.Json;

namespace Enterprise.Serialization.Json.Microsoft.JsonNamingPolicies;

/// <summary>
/// This was adapted from the .NET 8 JsonCamelCaseNamingPolicy.
/// The core logic hasn't changed, but the original implementation didn't accomodate for "." characters.
/// This resulted in half camel cased dictionary keys for things like translated error collections from the result pattern.
/// </summary>
public class CustomJsonCamelCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0]))
        {
            return name;
        }

        string[] parts = name.Split('.');
        IEnumerable<string> camelCasedParts = parts.Select(CamelCase);
        string result = string.Join(".", camelCasedParts);

        return result;
    }

    private static new string CamelCase(string part)
    {
        return string.Create(part.Length, part, (chars, name) =>
        {
            name.CopyTo(chars);
            FixCasing(chars);
        });
    }

    private static void FixCasing(Span<char> chars)
    {
        for (int i = 0; i < chars.Length; i++)
        {
            if (i == 1 && !char.IsUpper(chars[i]))
            {
                break;
            }

            bool hasNext = i + 1 < chars.Length;

            // Stop when next char is already lowercase.
            if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
            {
                // If the next char is a space, lowercase current char before exiting.
                if (chars[i + 1] == ' ')
                {
                    chars[i] = char.ToLowerInvariant(chars[i]);
                }
                break;
            }

            chars[i] = char.ToLowerInvariant(chars[i]);
        }
    }
}
