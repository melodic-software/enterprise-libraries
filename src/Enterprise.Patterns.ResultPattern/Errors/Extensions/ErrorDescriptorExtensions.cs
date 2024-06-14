using Enterprise.Patterns.ResultPattern.Errors.Model;

namespace Enterprise.Patterns.ResultPattern.Errors.Extensions;

public static class ErrorDescriptorExtensions
{
    public static bool ContainTrueError(this IEnumerable<ErrorDescriptor> errorDescriptors)
    {
        // Filter out the descriptor used for the "null object" instances.
        var trueErrorDescriptors = errorDescriptors
            .Where(d => d != ErrorDescriptor.NoError)
            .ToList();

        return trueErrorDescriptors.Any();
    }
}
