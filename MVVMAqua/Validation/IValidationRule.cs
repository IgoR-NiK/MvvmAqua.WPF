namespace MVVMAqua.Validation
{
	public interface IValidationRule<in T, out TResult> 
		where TResult : IValidationResult
	{
		TResult Check(T value);
	}
}