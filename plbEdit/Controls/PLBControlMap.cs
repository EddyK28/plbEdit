using plbEdit.Format;

namespace plbEdit.Controls
{
    public partial class PLBControlMap : PLBControlBase
    {
        MapEntry map;
        public PLBControlMap(PLBNode nd) : base(nd)
        {
            map = (MapEntry)node.item;
            InitializeComponent();
            InitData();
        }

        public override void InitData()
        {
            PLBControl_Box_Type.Text = map.Type;
            PLBControl_Box_Id.Text = map.Id;
            PLBControl_Box_Label.Text = map.Label;

            PLBControl_Box_Ukn1.Text = "0x" + map.ukn1.ToString("X8");
            PLBControl_Box_Ukn2.Text = "0x" + map.ukn2.ToString("X8");
            PLBControl_Box_Ukn3.Text = "0x" + map.ukn3.ToString("X8");
            PLBControl_Box_Ukn4.Text = "0x" + map.ukn4.ToString("X8");
        }

        public override void SaveData()
        {
            map.Type = PLBControl_Box_Type.Text;
            node.Text = map.Id = PLBControl_Box_Id.Text;
            map.Label = PLBControl_Box_Label.Text;

            map.ukn1 = ParseUint(PLBControl_Box_Ukn1.Text);
            map.ukn2 = ParseUint(PLBControl_Box_Ukn2.Text);
            map.ukn3 = ParseUint(PLBControl_Box_Ukn3.Text);
            map.ukn4 = ParseUint(PLBControl_Box_Ukn4.Text);
            InitData();
        }
    }
}
