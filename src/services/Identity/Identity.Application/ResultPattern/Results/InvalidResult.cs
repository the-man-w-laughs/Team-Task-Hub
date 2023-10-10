using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Application.Result;

namespace Identity.Application.ResultPattern.Results
{
    public class InvalidResult<T> : Result<T>
    {
        private string _error;
        public InvalidResult(string error)
        {
            _error = error;
        }
        public override ResultType ResultType => ResultType.Invalid;

        public override List<string> Errors => new List<string> { _error };

        public override T Data => default;
    }
}
