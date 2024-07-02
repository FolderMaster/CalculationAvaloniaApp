namespace Model.Parameters
{
    public interface IParametersComposite<T>
    {
        public IEnumerable<object> Parameters { get; }

        public T[] GetArguments();
    }
}
