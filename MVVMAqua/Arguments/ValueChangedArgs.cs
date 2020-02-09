namespace MVVMAqua.Arguments
{
    public class ValueChangedArgs<T>
    {
        public T OldValue { get; }
        
        public T NewValue { get; }

        public ValueChangedArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}