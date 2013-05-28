using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MefWpfExample.Extensions.MenuExtensions;

namespace MefWpfExample.Extensions
{
    public class ExtensionManager
    {
        private readonly CompositionContainer _container;

        [ImportMany(typeof(IMenuExtension))]
        public IEnumerable<Lazy<IMenuExtension, IMenuExtensionData>> MenuExtensions;

        public ExtensionManager(string pluginFolder)
        {
            var directoryCatalog = new DirectoryCatalog(pluginFolder);
            var domainCatalog = new ApplicationCatalog();
            var aggregateCatalog = new AggregateCatalog(directoryCatalog, domainCatalog);
            _container = new CompositionContainer(aggregateCatalog);

            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }
    }
}
