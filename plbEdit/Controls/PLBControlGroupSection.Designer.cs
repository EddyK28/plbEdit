namespace plbEdit.Controls
{
    partial class PLBControlGroupSection
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
            this.PLBControl_Box_Id = new PLBControlBox();
            this.PLBControl_Box_Type = new PLBControlBox();
            this.PLBControl_Label_Id = new System.Windows.Forms.Label();
            this.PLBControl_Label_Type = new System.Windows.Forms.Label();
            this.PLBControl_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // PLBControl_Panel
            // 
            this.PLBControl_Panel.AutoSize = true;
            this.PLBControl_Panel.ColumnCount = 2;
            this.PLBControl_Panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.PLBControl_Panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.PLBControl_Panel.Controls.Add(this.PLBControl_Box_Id, 1, 1);
            this.PLBControl_Panel.Controls.Add(this.PLBControl_Box_Type, 1, 0);
            this.PLBControl_Panel.Controls.Add(this.PLBControl_Label_Id, 0, 1);
            this.PLBControl_Panel.Controls.Add(this.PLBControl_Label_Type, 0, 0);
            this.PLBControl_Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.PLBControl_Panel.Location = new System.Drawing.Point(0, 0);
            this.PLBControl_Panel.Name = "PLBControl_Panel";
            this.PLBControl_Panel.RowCount = 2;
            this.PLBControl_Panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.PLBControl_Panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.PLBControl_Panel.Size = new System.Drawing.Size(372, 56);
            this.PLBControl_Panel.TabIndex = 7;
            // 
            // PLBControl_Box_Id
            // 
            this.PLBControl_Box_Id.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLBControl_Box_Id.Location = new System.Drawing.Point(53, 31);
            this.PLBControl_Box_Id.Name = "PLBControl_Box_Id";
            this.PLBControl_Box_Id.Size = new System.Drawing.Size(317, 22);
            this.PLBControl_Box_Id.TabIndex = 2;
            // 
            // PLBControl_Box_Type
            // 
            this.PLBControl_Box_Type.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLBControl_Box_Type.Location = new System.Drawing.Point(53, 3);
            this.PLBControl_Box_Type.Name = "PLBControl_Box_Type";
            this.PLBControl_Box_Type.Size = new System.Drawing.Size(317, 22);
            this.PLBControl_Box_Type.TabIndex = 1;
            // 
            // PLBControl_Label_Id
            // 
            this.PLBControl_Label_Id.AutoSize = true;
            this.PLBControl_Label_Id.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLBControl_Label_Id.Location = new System.Drawing.Point(3, 28);
            this.PLBControl_Label_Id.Name = "PLBControl_Label_Id";
            this.PLBControl_Label_Id.Size = new System.Drawing.Size(44, 28);
            this.PLBControl_Label_Id.TabIndex = 99;
            this.PLBControl_Label_Id.Text = "Id:";
            this.PLBControl_Label_Id.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PLBControl_Label_Type
            // 
            this.PLBControl_Label_Type.AutoSize = true;
            this.PLBControl_Label_Type.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLBControl_Label_Type.Location = new System.Drawing.Point(3, 0);
            this.PLBControl_Label_Type.Name = "PLBControl_Label_Type";
            this.PLBControl_Label_Type.Size = new System.Drawing.Size(44, 28);
            this.PLBControl_Label_Type.TabIndex = 99;
            this.PLBControl_Label_Type.Text = "Type:";
            this.PLBControl_Label_Type.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PLBControlGroupSection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PLBControl_Panel);
            this.Name = "PLBControlGroupSection";
            this.Size = new System.Drawing.Size(372, 87);
            this.PLBControl_Panel.ResumeLayout(false);
            this.PLBControl_Panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel PLBControl_Panel;
        private System.Windows.Forms.TextBox PLBControl_Box_Id;
        private System.Windows.Forms.TextBox PLBControl_Box_Type;
        private System.Windows.Forms.Label PLBControl_Label_Id;
        private System.Windows.Forms.Label PLBControl_Label_Type;
    }
}
