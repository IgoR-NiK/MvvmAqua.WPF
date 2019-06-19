namespace MVVMAqua.Validation
{
	public interface IValidationResult
	{
		bool IsValid { get; }

		string Message { get; }
	}
}
