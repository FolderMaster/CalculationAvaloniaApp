using System.Reflection;

namespace Model
{
    public class SingletonCollection<I>
    {
        public static IDictionary<Type, I> Instances { get; private set; }

        static SingletonCollection()
        {
            Instances = new Dictionary<Type, I>();
            UpdateInstances();
        }

        public static void UpdateInstances()
        {
            Instances.Clear();
            var types = Assembly.GetExecutingAssembly()
                .GetTypes().Where(t => t.IsAssignableTo(typeof(I)) && !t.IsAbstract);
            foreach (var type in types)
            {
                if (!Instances.ContainsKey(type))
                {
                    Instances.Add(type, (I)Activator.CreateInstance(type));
                }
            }
        }
    }
}
