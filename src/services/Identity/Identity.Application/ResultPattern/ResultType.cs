namespace Identity.Application.ResultPattern
{
    public enum ResultType
    {
        Ok,
        Invalid,
        Unauthorized,
        PartialOk,
        NotFound,
        PermissionDenied,
        Unexpected
    }
}
