using System.Collections.Generic;

namespace MefWpfExample.Extensions.MenuExtensions
{
    public interface IMenuExtensionData
    {
        string MenuExtensionName { get; }
        IEnumerable<string> MenuHierarchy { get; }
    }
}