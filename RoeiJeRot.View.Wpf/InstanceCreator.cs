using Microsoft.Extensions.DependencyInjection;

namespace RoeiJeRot.View.Wpf
{
    public sealed class InstanceCreator
    {
        private static InstanceCreator instance = null;
        private static readonly object padlock = new object();

        public static InstanceCreator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new InstanceCreator();
                    }
                    return instance;
                }
            }
        }

        public T CreateService<T>()
        {
            return App.Host.Services.GetService<T>();
        }
    }
}