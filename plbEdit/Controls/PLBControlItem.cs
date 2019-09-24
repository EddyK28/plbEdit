using plbEdit.Format;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
            {
                PLBControl_Label_Extra.Visible = true;
                PLBControl_Box_Extra.Visible = true;

                StringBuilder temp = new StringBuilder();
                ref uint[] data = ref item.extraData;
                if (item.Type == "SplineType")
                {
                    foreach (uint val in item.splineData)
                        temp.Append($"0x{val.ToString("X8")}\r\n");
                }
                else
                {
                    foreach (uint val in item.extraData)
                        temp.Append($"0x{val.ToString("X8")}\r\n");
                }
                temp.Length -= 2;
                PLBControl_Box_Extra.Text = temp.ToString();
            }
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

            //TODO: parse extra data

            InitData();
        }

        private void Box_Extra__TextChanged(object sender, EventArgs e)
        {
             Size sz = new Size(PLBControl_Box_Extra.ClientSize.Width, int.MaxValue);
             int borders = PLBControl_Box_Extra.Height - PLBControl_Box_Extra.ClientSize.Height;
             sz = TextRenderer.MeasureText(PLBControl_Box_Extra.Text, PLBControl_Box_Extra.Font, sz);
             int h = sz.Height + borders + 4;
             PLBControl_Box_Extra.Height = h;
        }
    }
}
