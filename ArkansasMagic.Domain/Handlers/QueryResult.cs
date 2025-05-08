using System.Collections.Generic;

namespace ArkansasMagic.Domain.Handlers
{
    public class QueryResult<TResult> : ValidationResult
    {
        public QueryResult(IList<string> errors = null)
            : base(errors)
        { }

        public string Message { get; private set; }

        public TResult Result { get; private set; }

        public FailureReasonType FailureReason { get; private set; } = FailureReasonType.None;


        public static QueryResult<TResult> Ok(TResult result, string message = null)
            => new QueryResult<TResult>
            {
                Message = message,
                Result = result
            };

        public static QueryResult<TResult> Fail(string message
            , FailureReasonType failureReason = FailureReasonType.ValidationErrors
            , TResult result = default)
        {
            var errors = new List<string>();
            if (!string.IsNullOrEmpty(message))
            {
                errors.Add(message);
            }

            return Fail(errors, failureReason, result);
        }

        public static QueryResult<TResult> Fail(List<string> errors
            , FailureReasonType failureReason = FailureReasonType.ValidationErrors
            , TResult result = default)
        {
            return new QueryResult<TResult>(errors)
            {
                Result = result,
                FailureReason = failureReason
            };
        }
    }
}
