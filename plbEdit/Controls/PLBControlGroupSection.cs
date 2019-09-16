using plbEdit.Format;

namespace plbEdit.Controls
{
    public partial class PLBControlGroupSection : PLBControlBase
    {
        GroupSection section;
        public PLBControlGroupSection(PLBNode nd) : base(nd)
        {
            section = (GroupSection)node.item;
            InitializeComponent();
            InitData();
        }

        public override void InitData()
        {
            PLBControl_Box_Type.Text = section.Type;
            PLBControl_Box_Id.Text = section.Id;
        }

        public override void SaveData()
        {
            section.Type = PLBControl_Box_Type.Text;
            node.Text = section.Id = PLBControl_Box_Id.Text;
        }
    }
}
