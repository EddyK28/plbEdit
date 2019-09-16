using plbEdit.Format;

namespace plbEdit.Controls
{
    public partial class PLBControlGroupref : PLBControlBase
    {
        GroupRef groupRef;
        public PLBControlGroupref(PLBNode nd) : base(nd)
        {
            groupRef = (GroupRef)node.item;
            InitializeComponent();
            InitData();
        }

        public override void InitData()
        {
            PLBControl_Box_Group.Text = groupRef.gref;
            PLBControl_Box_Ukn.Text = "0x" + groupRef.ukn.ToString("X8");
        }

        public override void SaveData()
        {
            node.Text = groupRef.gref = PLBControl_Box_Group.Text;
            groupRef.ukn = ParseUint(PLBControl_Box_Ukn.Text);
            InitData();
        }
    }
}
