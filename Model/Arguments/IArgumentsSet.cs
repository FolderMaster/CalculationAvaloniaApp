namespace Model.Arguments
{
    public interface IArgumentsSet<T>
    {
        public IEnumerable<IArgument<double>> Arguments { get; }

        public event EventHandler<IArgument<T>>? ArgumentChanged;

        public T[] GetArray();
    }
}
