namespace Enterprise.Library.Core.Streaming;

public static class MemoryStreamExtensions
{
    public static void JumpToBeginning(this MemoryStream stream)
    {
        if (stream.Position == 0)
            return;

        stream.Seek(0, SeekOrigin.Begin);

        // This is an alternative.
        //stream.Position = 0;
    }
}