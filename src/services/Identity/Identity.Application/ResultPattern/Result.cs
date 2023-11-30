namespace Identity.Application.ResultPattern
{
    public abstract class Result<T>
    {
        public abstract ResultType ResultType { get; }
        public abstract List<string> Errors { get; }
        public abstract T Value { get; }
    }
}
