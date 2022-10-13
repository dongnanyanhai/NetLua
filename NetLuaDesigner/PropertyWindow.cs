using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace NetLuaDesigner
{
    public partial class PropertyWindow : DockContent
    {
        public PropertyWindow()
        {
            InitializeComponent();
            this.Propertybox.ShowEventTab = true;
        }

    }
}
