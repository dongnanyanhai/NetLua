using System;
using System.ComponentModel;

namespace Smart.FormDesigner.Internal
{
    internal class PropertyGridSite : ISite, IServiceProvider
    {
        private IServiceProvider _serviceProvider;

        public IComponent Component => null;

        public IContainer Container => null;

        public bool DesignMode => false;

        public string Name
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public PropertyGridSite(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider?.GetService(serviceType);
        }
    }

}