using Microsoft.Extensions.DependencyInjection;

namespace RoeiJeRot.View.Wpf.Logic
{
    /// <summary>
    ///     Get access to the dependency injection system which can create instances of types.
    /// </summary>
    public sealed class InstanceCreator
    {
        private static InstanceCreator instance;
        private static readonly object padlock = new object();

        /// <summary>
        ///     Singleton value of the `InstanceCreator`.
        /// </summary>
        public static InstanceCreator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null) instance = new InstanceCreator();
                    return instance;
                }
            }
        }

        /// <summary>
        ///     Returns an instance of the given generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreateInstance<T>()
        {
            return App.Host.Services.GetService<T>();
        }
    }
}