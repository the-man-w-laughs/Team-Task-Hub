using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Result
{    
    public abstract class Result<T>
    {
        public abstract ResultType ResultType { get; }
        public abstract List<string> Errors { get; }
        public abstract T Data { get; }
    }    
}
