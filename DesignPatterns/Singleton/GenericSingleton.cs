namespace KuroNeko.Utilities.DesignPattern
{
    public class GenericSingleton<T> where T : class, new()
    {
        private static T _instance;
        private static readonly object _lock = new();

        protected GenericSingleton() { }

        public static T Instance
        {
            get
            {
                lock (_lock)
                    return _instance ??= new T();
            }
        }
    }
}
