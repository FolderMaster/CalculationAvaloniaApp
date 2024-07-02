namespace Model.Parameters
{
    public interface IParameter<T>
    {
        public T Value { get; set; }
    }
}
