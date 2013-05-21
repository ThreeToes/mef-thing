using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MefWpfExample.Extensions.MenuExtensions.Derps
{
    [Export(typeof(IMenuExtension))]
    [ExportMetadata("MenuExtensionName","The Derpinator")]
    [ExportMetadata("MenuHierarchy", new[]{"Herpa","Derpa"})]
    class DerpMenuExtension : IMenuExtension
    {
        public void Launch()
        {
            System.Windows.MessageBox.Show("Derp", "THE DERPINATOR KNOWS ALL!");
        }
    }
}
