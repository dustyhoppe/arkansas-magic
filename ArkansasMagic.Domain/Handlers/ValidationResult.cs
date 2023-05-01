using System.Collections.Generic;
using System.Linq;

namespace ArkansasMagic.Domain.Handlers
{
    public class ValidationResult
	{
		public ValidationResult(IList<string> errors = null)
		{
			Errors = errors ?? new List<string>();
		}

		public bool IsValidResponse => !Errors.Any();

		public IList<string> Errors { get; }
	}
}
