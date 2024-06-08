namespace Enterprise.Applications.DI.Registration.Sorting;

public class SegmentComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        if (x == null && y == null)
        {
            return 0;
        }

        if (x == null)
        {
            return -1;
        }

        if (y == null)
        {
            return 1;
        }

        string[] xSegments = x.Split('.');
        string[] ySegments = y.Split('.');

        int minLength = Math.Min(xSegments.Length, ySegments.Length);

        for (int i = 0; i < minLength; i++)
        {
            int comparison = string.Compare(xSegments[i], ySegments[i], StringComparison.Ordinal);

            if (comparison != 0)
            {
                return comparison;
            }
        }

        return xSegments.Length.CompareTo(ySegments.Length);
    }
}
