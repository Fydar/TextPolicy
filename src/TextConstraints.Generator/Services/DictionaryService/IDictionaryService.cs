using System.Collections.Generic;
using System.Threading;

namespace TextConstraints.Generator.Services.DictionaryService
{
	public interface IDictionaryService
	{
		public IAsyncEnumerable<string> GetWordsAsync(
			CancellationToken cancellationToken = default);
	}
}
