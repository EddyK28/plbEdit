
namespace plbEdit.Controls
{
    public class PLBControlBox : System.Windows.Forms.TextBox
    {
        static System.Drawing.Font fnt = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        public PLBControlBox() : base() { Font = fnt; }
    }
}
