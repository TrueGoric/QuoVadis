namespace QuoVadis.Common.Extensions
{
    public static class TaskExtensions
    {
        public static Task SilenceCancellation(this Task task)
            => task.ContinueWith(t => { }, TaskContinuationOptions.OnlyOnCanceled);

        public static Task<T?> SilenceCancellation<T>(this Task<T> task)
            where T : class
            => task.ContinueWith(t => (T?)default, TaskContinuationOptions.OnlyOnCanceled);
    }
}
