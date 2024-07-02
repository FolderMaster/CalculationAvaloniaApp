namespace Model.Parameters
{
    public interface IParametersSet<T>
    {
        public int Count { get; }

        public T this[int index] { get; set; }
    }
}
