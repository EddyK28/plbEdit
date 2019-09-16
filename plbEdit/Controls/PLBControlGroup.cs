using plbEdit.Format;

namespace plbEdit.Controls
{
    public partial class PLBControlGroup : PLBControlBase
    {
        Group group;
        public PLBControlGroup(PLBNode nd) : base(nd)
        {
            group = (Group)node.item;
            InitializeComponent();
            InitData();
        }

        public override void InitData()
        {
            PLBControl_Box_Type.Text = group.Type;
            PLBControl_Box_Id.Text = group.Id;
        }

        public override void SaveData()
        {
            group.Type = PLBControl_Box_Type.Text;
            node.Text = group.Id = PLBControl_Box_Id.Text;
        }
    }
}
