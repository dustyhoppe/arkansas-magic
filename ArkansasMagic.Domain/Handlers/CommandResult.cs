using System.Collections.Generic;

namespace ArkansasMagic.Domain.Handlers
{
    public class CommandResult : ValidationResult
    {
        public CommandResult(IList<string> errors = null)
            : base(errors)
        { }

        public string Message { get; private set; }
        public FailureReasonType FailureReason { get; private set; } = FailureReasonType.None;

        public static CommandResult Ok(string message = null)
        {
            return new CommandResult()
            {
                Message = message,
            };
        }

        public static CommandResult Fail(string message, FailureReasonType failureReason = FailureReasonType.ValidationErrors)
        {
            var errors = new List<string>();
            if (!string.IsNullOrEmpty(message))
            {
                errors.Add(message);
            }

            return Fail(errors, failureReason);
        }

        public static CommandResult Fail(List<string> errors, FailureReasonType failureReason = FailureReasonType.ValidationErrors)
        {
            return new CommandResult(errors)
            {
                FailureReason = failureReason
            };
        }
    }
}
