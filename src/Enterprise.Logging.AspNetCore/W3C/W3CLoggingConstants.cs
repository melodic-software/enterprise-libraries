namespace Enterprise.Logging.AspNetCore.W3C
{
    public static class W3CLoggingConstants
    {
        /// <summary>
        /// Example directory for Windows:
        /// C:\Users\{username}\AppData\Local\logs
        /// </summary>
        public const string LogDirectoryName = "logs";

        /// <summary>
        /// The resulting file will look like this: {ApplicationName}-W3C20230906.0000.txt
        /// </summary>
        /// <param name="applicationName"></param>
        /// <returns></returns>
        public static string GetFileNamePrefix(string applicationName) => $"{applicationName}-W3C";

        /// <summary>
        /// This is the file size limitations.
        /// 5,242,880 bytes = 5.24288 megabytes
        /// </summary>
        public const int FileSizeLimitInBytes = 5 * 1024 * 1024;

        /// <summary>
        /// This is the interval that logs should be flushed / written.
        /// </summary>
        public const int FlushIntervalInSeconds = 2;
    }
}