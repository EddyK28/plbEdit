using System;
using System.Windows.Forms;

namespace plbEdit.Controls
{
    public class PLBControlBase : UserControl
    {
        protected PLBNode node;

        public PLBControlBase() {}
        public PLBControlBase(PLBNode nd) { node = nd; }

        public virtual void InitData() { }
        public virtual void SaveData() { }

        public uint ParseUint(string str)
        {
            if (str.Length == 0) return 0;
            if (str.Length > 2 && str.Substring(0,2).Equals("0x",StringComparison.OrdinalIgnoreCase)) return Convert.ToUInt32(str,16);
            if (str[0] == '0') return Convert.ToUInt32(str, 8);

            return (uint)Convert.ToInt32(str);
        }
    }
}
