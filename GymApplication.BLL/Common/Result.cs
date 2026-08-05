using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.BLL.Common
{
    public sealed record Result(bool success , string? error = null , ResultKind kind = ResultKind.OK)
    {
        public static Result OK() => new(true);
        public static Result Fail(string error , ResultKind kind = ResultKind.Conflict)
        {
            return new(false, error , kind);
        }

        public static Result NotFound(string error = "Not Found")
        {
            return new(false, error, ResultKind.NotFound);
        }

        public static Result Validation(string error)
        {
            return new(false, error, ResultKind.ValidationFailed);
        }
    }

    public sealed record Result<T>(bool success , T? value ,  string? error = null , ResultKind kind = ResultKind.OK)
    {
        public static Result<T> OK(T val) => new(true,val);
        public static Result<T> Fail(string error, ResultKind kind = ResultKind.Conflict)
        {
            return new(false, default, error ,  kind);
        }

        public static Result<T> NotFound(string error = "Not Found")
        {
            return new(false , default, error, ResultKind.NotFound);
        }
    }
}
