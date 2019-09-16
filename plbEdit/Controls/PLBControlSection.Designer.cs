namespace plbEdit.Controls
{
    partial class PLBControlSection
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
            this.PLBControl_Box_Name = new PLBControlBox();
            this.PLBControl_Label_Name = new System.Windows.Forms.Label();
            this.PLBControl_Panel = new System.Windows.Forms.TableLayoutPanel();
            this.PLBControl_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // PLBControl_Box_Name
            // 
            this.PLBControl_Box_Name.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLBControl_Box_Name.Location = new System.Drawing.Point(58, 3);
            this.PLBControl_Box_Name.Name = "PLBControl_Box_Name";
            this.PLBControl_Box_Name.Size = new System.Drawing.Size(301, 22);
            this.PLBControl_Box_Name.TabIndex = 3;
            // 
            // PLBControl_Label_Name
            // 
            this.PLBControl_Label_Name.AutoSize = true;
            this.PLBControl_Label_Name.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLBControl_Label_Name.Location = new System.Drawing.Point(3, 0);
            this.PLBControl_Label_Name.Name = "PLBControl_Label_Name";
            this.PLBControl_Label_Name.Size = new System.Drawing.Size(49, 28);
            this.PLBControl_Label_Name.TabIndex = 99;
            this.PLBControl_Label_Name.Text = "Name:";
            this.PLBControl_Label_Name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PLBControl_Panel
            // 
            this.PLBControl_Panel.AutoSize = true;
            this.PLBControl_Panel.ColumnCount = 2;
            this.PLBControl_Panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.PLBControl_Panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.PLBControl_Panel.Controls.Add(this.PLBControl_Box_Name, 1, 0);
            this.PLBControl_Panel.Controls.Add(this.PLBControl_Label_Name, 0, 0);
            this.PLBControl_Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.PLBControl_Panel.Location = new System.Drawing.Point(0, 0);
            this.PLBControl_Panel.Name = "PLBControl_Panel";
            this.PLBControl_Panel.RowCount = 1;
            this.PLBControl_Panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PLBControl_Panel.Size = new System.Drawing.Size(362, 28);
            this.PLBControl_Panel.TabIndex = 1;
            // 
            // PLBControlSection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.PLBControl_Panel);
            this.Name = "PLBControlSection";
            this.Size = new System.Drawing.Size(362, 78);
            this.PLBControl_Panel.ResumeLayout(false);
            this.PLBControl_Panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PLBControl_Box_Name;
        private System.Windows.Forms.Label PLBControl_Label_Name;
        private System.Windows.Forms.TableLayoutPanel PLBControl_Panel;
    }
}
