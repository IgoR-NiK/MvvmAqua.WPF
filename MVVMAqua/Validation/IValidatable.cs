using System;
using System.Collections.ObjectModel;

namespace MVVMAqua.Validation
{
	public interface IValidatable<T, TResult>
		where TResult : IValidationResult
	{
		bool IsValid { get; }

		ObservableCollection<TResult> Errors { get; }

		T Value { get; set; }


		void AddRule(IValidationRule<T, TResult> rule);

		void AddRule(Func<T, TResult> rule);

		bool Validate();
	}
}