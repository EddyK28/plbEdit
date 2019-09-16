using plbEdit.Format;
using System;

namespace plbEdit.Controls
{
    public partial class PLBControlItem : PLBControlBase
    {
        ItemEntry item;
        public PLBControlItem(PLBNode nd) : base(nd)
        {
            item = (ItemEntry)node.item;
            InitializeComponent();
            InitData();
        }

        public override void InitData()
        {
            PLBControl_Box_Type.Text = item.Type;
            PLBControl_Box_Id.Text = item.Id;
            PLBControl_Box_Label.Text = item.Label;

            PLBControl_Box_Layer.Text = item.layerIdx.ToString();

            PLBControl_Box_PosX.Text = item.posX.ToString();
            PLBControl_Box_PosY.Text = item.posY.ToString();
            PLBControl_Box_PosZ.Text = item.posZ.ToString();
            PLBControl_Box_Ukn1.Text = "0x" + item.ukn1.ToString("X8");
            PLBControl_Box_Ukn2.Text = "0x" + item.ukn2.ToString("X8");

            if (item.GetExtraBytes() > 0)
                PLBControl_Label_Warning.Visible = true;
        }

        public override void SaveData()
        {
            item.Type = PLBControl_Box_Type.Text;
            node.Text = item.Id = PLBControl_Box_Id.Text;
            item.Label = PLBControl_Box_Label.Text;

            item.layerIdx = (int)ParseUint(PLBControl_Box_Layer.Text);

            item.posX = Convert.ToSingle(PLBControl_Box_PosX.Text);
            item.posY = Convert.ToSingle(PLBControl_Box_PosY.Text);
            item.posZ = Convert.ToSingle(PLBControl_Box_PosZ.Text);
            item.ukn1 = ParseUint(PLBControl_Box_Ukn1.Text);
            item.ukn2 = ParseUint(PLBControl_Box_Ukn2.Text);
            InitData();
        }
    }
}
