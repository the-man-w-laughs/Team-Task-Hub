namespace Identity.Application.ResultPattern.Results
{
    public class SuccessResult<T> : Result<T>
    {
        private readonly T _value;

        public SuccessResult(T value)
        {
            _value = value;
        }

        public override ResultType ResultType => ResultType.Ok;
        public override List<string> Errors => new List<string>();
        public override T Value => _value;
    }
}
