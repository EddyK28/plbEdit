namespace plbEdit.Controls
{
    partial class PLBControlGroupref
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PLBControl_Panel = new System.Windows.Forms.TableLayoutPanel();
            this.PLBControl_Box_Ukn = new PLBControlBox();
            this.PLBControl_Box_Group = new PLBControlBox();
            this.PLBControl_Label_Ukn = new System.Windows.Forms.Label();
            this.PLBControl_Label_Group = new System.Windows.Forms.Label();
            this.PLBControl_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // PLBControl_Panel
            // 
            this.PLBControl_Panel.AutoSize = true;
            this.PLBControl_Panel.ColumnCount = 2;
            this.PLBControl_Panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.PLBControl_Panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.PLBControl_Panel.Controls.Add(this.PLBControl_Box_Ukn, 1, 1);
            this.PLBControl_Panel.Controls.Add(this.PLBControl_Box_Group, 1, 0);
            this.PLBControl_Panel.Controls.Add(this.PLBControl_Label_Ukn, 0, 1);
            this.PLBControl_Panel.Controls.Add(this.PLBControl_Label_Group, 0, 0);
            this.PLBControl_Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.PLBControl_Panel.Location = new System.Drawing.Point(0, 0);
            this.PLBControl_Panel.Name = "PLBControl_Panel";
            this.PLBControl_Panel.RowCount = 2;
            this.PLBControl_Panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.PLBControl_Panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.PLBControl_Panel.Size = new System.Drawing.Size(387, 56);
            this.PLBControl_Panel.TabIndex = 7;
            // 
            // PLBControl_Box_Ukn
            // 
            this.PLBControl_Box_Ukn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLBControl_Box_Ukn.Location = new System.Drawing.Point(61, 31);
            this.PLBControl_Box_Ukn.Name = "PLBControl_Box_Ukn";
            this.PLBControl_Box_Ukn.Size = new System.Drawing.Size(323, 22);
            this.PLBControl_Box_Ukn.TabIndex = 2;
            // 
            // PLBControl_Box_Group
            // 
            this.PLBControl_Box_Group.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLBControl_Box_Group.Location = new System.Drawing.Point(61, 3);
            this.PLBControl_Box_Group.Name = "PLBControl_Box_Group";
            this.PLBControl_Box_Group.Size = new System.Drawing.Size(323, 22);
            this.PLBControl_Box_Group.TabIndex = 1;
            // 
            // PLBControl_Label_Ukn
            // 
            this.PLBControl_Label_Ukn.AutoSize = true;
            this.PLBControl_Label_Ukn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLBControl_Label_Ukn.Location = new System.Drawing.Point(3, 28);
            this.PLBControl_Label_Ukn.Name = "PLBControl_Label_Ukn";
            this.PLBControl_Label_Ukn.Size = new System.Drawing.Size(52, 28);
            this.PLBControl_Label_Ukn.TabIndex = 99;
            this.PLBControl_Label_Ukn.Text = "Ukn:";
            this.PLBControl_Label_Ukn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PLBControl_Label_Group
            // 
            this.PLBControl_Label_Group.AutoSize = true;
            this.PLBControl_Label_Group.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLBControl_Label_Group.Location = new System.Drawing.Point(3, 0);
            this.PLBControl_Label_Group.Name = "PLBControl_Label_Group";
            this.PLBControl_Label_Group.Size = new System.Drawing.Size(52, 28);
            this.PLBControl_Label_Group.TabIndex = 99;
            this.PLBControl_Label_Group.Text = "Group:";
            this.PLBControl_Label_Group.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PLBControlGroupref
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.PLBControl_Panel);
            this.Name = "PLBControlGroupref";
            this.Size = new System.Drawing.Size(387, 79);
            this.PLBControl_Panel.ResumeLayout(false);
            this.PLBControl_Panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel PLBControl_Panel;
        private System.Windows.Forms.TextBox PLBControl_Box_Ukn;
        private System.Windows.Forms.TextBox PLBControl_Box_Group;
        private System.Windows.Forms.Label PLBControl_Label_Ukn;
        private System.Windows.Forms.Label PLBControl_Label_Group;
    }
}
