namespace Loggerlibrary
{
    /// <summary>
    /// configuration interface
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Log folder path
        /// </summary>
        string LoggerDir { get; }

        /// <summary>
        /// Max log file size
        /// </summary>
        long MaxFileSize { get; }

        /// <summary>
        /// QueueLogger timer period
        /// </summary>
        int QueueTimerPeriodMs { get; }
    }
}
