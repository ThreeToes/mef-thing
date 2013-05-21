using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MefWpfExample.Extensions.MenuExtensions;

namespace MefWpfExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CompositionContainer _container;
        [ImportMany(typeof(IMenuExtension))]
        public IEnumerable<Lazy<IMenuExtension, IMenuExtensionData>> MenuExtensions;

        public ObservableCollection<MenuItem> PluginMenuItems { get; private set; }

        public MainWindow()
        {
            InitialiseMefParts();
            PluginMenuItems = new ObservableCollection<MenuItem>();
            DataContext = this;
            InitializeComponent();
            SetupPluginsMenu();
        }

        private void SetupPluginsMenu()
        {
            PluginMenuItems.Clear();
            foreach (var menuExtension in MenuExtensions)
            {
                var currentParent = PluginMenu;
                var hierarchy = menuExtension.Metadata.MenuHierarchy;
                foreach (var entry in hierarchy)
                {
                    var first = currentParent.Items.Cast<MenuItem>().FirstOrDefault(x => x.Header.ToString() == entry);
                    if(first == null)
                    {
                        var menuItem = new MenuItem(){Header = entry};
                        menuItem.ItemsSource = new ObservableCollection<MenuItem>();
                        var items = new ObservableCollection<MenuItem>();
                        currentParent.ItemsSource = items;
                        items.Add(menuItem);
                        currentParent = menuItem;
                    }
                    else
                    {
                        currentParent = first;
                    }
                }
                var pluginEntry = new MenuItem()
                                      {
                                          Header = menuExtension.Metadata.MenuExtensionName
                                      };
                pluginEntry.Click += (sender, ev) => menuExtension.Value.Launch();
                var collection = (ICollection<MenuItem>)currentParent.ItemsSource;
                collection.Add(pluginEntry);
                
            }
        }

        private void InitialiseMefParts()
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(MainWindow).Assembly));

            //Create the CompositionContainer with the parts in the catalog
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object
            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }

        private void MenuItemClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
