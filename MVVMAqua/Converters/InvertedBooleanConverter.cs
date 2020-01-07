namespace MVVMAqua.Converters
{
	public sealed class InvertedBoolConverter : BooleanConverter<bool>
	{
		public InvertedBoolConverter()
			: base(false, true)
		{
		}
	}
}