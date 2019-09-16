using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using plbEdit.Format;

namespace plbEdit.Controls
{
    public partial class PLBControlSection : PLBControlBase
    {
        Section section;
        public PLBControlSection(PLBNode nd) : base(nd)
        {
            section = (Section)node.item;
            InitializeComponent();
            InitData();
        }

        public override void InitData()
        {
            PLBControl_Box_Name.Text = section.Name;
        }

        public override void SaveData()
        {
            node.Text = section.Name = PLBControl_Box_Name.Text;
        }
    }
}
