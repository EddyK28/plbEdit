using plbEdit.Format;

namespace plbEdit.Controls
{
    public partial class PLBControlLayer : PLBControlBase
    {
        Layer layer;
        public PLBControlLayer(PLBNode nd) : base(nd)
        {
            layer = (Layer)node.item;
            InitializeComponent();
            InitData();
        }

        public override void InitData()
        {
            PLBControl_Box_Name.Text = layer.Name;
            PLBControl_Box_Data1.Text = "0x" + layer.data1.ToString("X8");
            PLBControl_Box_Data2.Text = "0x" + layer.data2.ToString("X8");
            PLBControl_Box_Data3.Text = "0x" + layer.data3.ToString("X8");
            PLBControl_Box_Data4.Text = "0x" + layer.data4.ToString("X8");
            PLBControl_Box_Ukn1.Text = "0x" + layer.ukn1.ToString("X8");
            PLBControl_Box_Ukn2.Text = "0x" + layer.ukn2.ToString("X8");
        }

        public override void SaveData()
        {
            node.Text = layer.Name = PLBControl_Box_Name.Text;
            layer.data1 = ParseUint(PLBControl_Box_Data1.Text);
            layer.data2 = ParseUint(PLBControl_Box_Data2.Text);
            layer.data3 = ParseUint(PLBControl_Box_Data3.Text);
            layer.data4 = ParseUint(PLBControl_Box_Data4.Text);
            layer.ukn1 = ParseUint(PLBControl_Box_Ukn1.Text);
            layer.ukn2 = ParseUint(PLBControl_Box_Ukn2.Text);
            InitData();
        }
    }
}
