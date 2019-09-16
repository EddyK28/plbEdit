namespace plbEdit
{
    partial class PlbEdit
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Sections");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Groups");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Layers");
            this.MenuContext_Container = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuContext_Container_Add = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip_Main = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_Quit = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools_Import = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools_Export = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Help_About = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Help_Cmd = new System.Windows.Forms.ToolStripMenuItem();
            this.PLBTree = new System.Windows.Forms.TreeView();
            this.MainContainer = new System.Windows.Forms.SplitContainer();
            this.PLBPanel = new System.Windows.Forms.Panel();
            this.PLBPanel_Buttons = new System.Windows.Forms.Panel();
            this.PLBPanel_Button_Reset = new System.Windows.Forms.Button();
            this.PLBPanel_Button_Save = new System.Windows.Forms.Button();
            this.MenuContext_Containee = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuContext_Containee_Remove = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuContext_Containee_Add = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuContext_Container.SuspendLayout();
            this.MenuStrip_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).BeginInit();
            this.MainContainer.Panel1.SuspendLayout();
            this.MainContainer.Panel2.SuspendLayout();
            this.MainContainer.SuspendLayout();
            this.PLBPanel_Buttons.SuspendLayout();
            this.MenuContext_Containee.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuContext_Container
            // 
            this.MenuContext_Container.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuContext_Container.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuContext_Container_Add});
            this.MenuContext_Container.Name = "MenuContext_Container";
            this.MenuContext_Container.Size = new System.Drawing.Size(263, 60);
            // 
            // MenuContext_Container_Add
            // 
            this.MenuContext_Container_Add.Enabled = false;
            this.MenuContext_Container_Add.Name = "MenuContext_Container_Add";
            this.MenuContext_Container_Add.Size = new System.Drawing.Size(262, 28);
            this.MenuContext_Container_Add.Text = "&Add (Not Implemented)";
            // 
            // MenuStrip_Main
            // 
            this.MenuStrip_Main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Tools,
            this.Menu_Help});
            this.MenuStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip_Main.Name = "MenuStrip_Main";
            this.MenuStrip_Main.Size = new System.Drawing.Size(800, 31);
            this.MenuStrip_Main.TabIndex = 0;
            this.MenuStrip_Main.Text = "menuStrip1";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File_Open,
            this.Menu_File_Save,
            this.Menu_File_SaveAs,
            this.Menu_File_Quit});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(47, 27);
            this.Menu_File.Text = "&File";
            // 
            // Menu_File_Open
            // 
            this.Menu_File_Open.Name = "Menu_File_Open";
            this.Menu_File_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.Menu_File_Open.Size = new System.Drawing.Size(235, 28);
            this.Menu_File_Open.Text = "&Open";
            this.Menu_File_Open.Click += new System.EventHandler(this.Menu_File_Open__Click);
            // 
            // Menu_File_Save
            // 
            this.Menu_File_Save.Enabled = false;
            this.Menu_File_Save.Name = "Menu_File_Save";
            this.Menu_File_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.Menu_File_Save.Size = new System.Drawing.Size(235, 28);
            this.Menu_File_Save.Text = "&Save";
            this.Menu_File_Save.Click += new System.EventHandler(this.Menu_File_Save__Click);
            // 
            // Menu_File_SaveAs
            // 
            this.Menu_File_SaveAs.Enabled = false;
            this.Menu_File_SaveAs.Name = "Menu_File_SaveAs";
            this.Menu_File_SaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.Menu_File_SaveAs.Size = new System.Drawing.Size(235, 28);
            this.Menu_File_SaveAs.Text = "Save &As";
            this.Menu_File_SaveAs.Click += new System.EventHandler(this.Menu_File_SaveAs__Click);
            // 
            // Menu_File_Quit
            // 
            this.Menu_File_Quit.Name = "Menu_File_Quit";
            this.Menu_File_Quit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.Menu_File_Quit.Size = new System.Drawing.Size(235, 28);
            this.Menu_File_Quit.Text = "&Quit";
            this.Menu_File_Quit.Click += new System.EventHandler(this.Menu_File_Quit__Click);
            // 
            // Menu_Tools
            // 
            this.Menu_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Tools_Import,
            this.Menu_Tools_Export});
            this.Menu_Tools.Name = "Menu_Tools";
            this.Menu_Tools.Size = new System.Drawing.Size(62, 27);
            this.Menu_Tools.Text = "&Tools";
            // 
            // Menu_Tools_Import
            // 
            this.Menu_Tools_Import.Name = "Menu_Tools_Import";
            this.Menu_Tools_Import.Size = new System.Drawing.Size(138, 28);
            this.Menu_Tools_Import.Text = "&Import";
            this.Menu_Tools_Import.Click += new System.EventHandler(this.Menu_Tools_Import__Click);
            // 
            // Menu_Tools_Export
            // 
            this.Menu_Tools_Export.Enabled = false;
            this.Menu_Tools_Export.Name = "Menu_Tools_Export";
            this.Menu_Tools_Export.Size = new System.Drawing.Size(138, 28);
            this.Menu_Tools_Export.Text = "&Export";
            this.Menu_Tools_Export.Click += new System.EventHandler(this.Menu_Tools_Export__Click);
            // 
            // Menu_Help
            // 
            this.Menu_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Help_About,
            this.Menu_Help_Cmd});
            this.Menu_Help.Name = "Menu_Help";
            this.Menu_Help.Size = new System.Drawing.Size(57, 27);
            this.Menu_Help.Text = "&Help";
            // 
            // Menu_Help_About
            // 
            this.Menu_Help_About.Name = "Menu_Help_About";
            this.Menu_Help_About.Size = new System.Drawing.Size(242, 28);
            this.Menu_Help_About.Text = "&About";
            this.Menu_Help_About.Click += new System.EventHandler(this.Menu_Help_About__Click);
            // 
            // Menu_Help_Cmd
            // 
            this.Menu_Help_Cmd.Name = "Menu_Help_Cmd";
            this.Menu_Help_Cmd.Size = new System.Drawing.Size(242, 28);
            this.Menu_Help_Cmd.Text = "&Command Line Help";
            this.Menu_Help_Cmd.Click += new System.EventHandler(this.Menu_Help_Cmd__Click);
            // 
            // PLBTree
            // 
            this.PLBTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLBTree.Location = new System.Drawing.Point(0, 0);
            this.PLBTree.Name = "PLBTree";
            treeNode1.ContextMenuStrip = this.MenuContext_Container;
            treeNode1.Name = "Node_Sect";
            treeNode1.Text = "Sections";
            treeNode2.ContextMenuStrip = this.MenuContext_Container;
            treeNode2.Name = "Node_Group";
            treeNode2.Text = "Groups";
            treeNode3.ContextMenuStrip = this.MenuContext_Container;
            treeNode3.Name = "Node_Lyr";
            treeNode3.Text = "Layers";
            this.PLBTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.PLBTree.RightToLeftLayout = true;
            this.PLBTree.Size = new System.Drawing.Size(188, 419);
            this.PLBTree.TabIndex = 0;
            this.PLBTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.PLBTree__Select);
            // 
            // MainContainer
            // 
            this.MainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainContainer.Location = new System.Drawing.Point(0, 31);
            this.MainContainer.Name = "MainContainer";
            // 
            // MainContainer.Panel1
            // 
            this.MainContainer.Panel1.Controls.Add(this.PLBTree);
            // 
            // MainContainer.Panel2
            // 
            this.MainContainer.Panel2.Controls.Add(this.PLBPanel);
            this.MainContainer.Panel2.Controls.Add(this.PLBPanel_Buttons);
            this.MainContainer.Size = new System.Drawing.Size(800, 419);
            this.MainContainer.SplitterDistance = 188;
            this.MainContainer.TabIndex = 1;
            // 
            // PLBPanel
            // 
            this.PLBPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLBPanel.Location = new System.Drawing.Point(0, 0);
            this.PLBPanel.Name = "PLBPanel";
            this.PLBPanel.Size = new System.Drawing.Size(608, 391);
            this.PLBPanel.TabIndex = 2;
            // 
            // PLBPanel_Buttons
            // 
            this.PLBPanel_Buttons.Controls.Add(this.PLBPanel_Button_Reset);
            this.PLBPanel_Buttons.Controls.Add(this.PLBPanel_Button_Save);
            this.PLBPanel_Buttons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PLBPanel_Buttons.Location = new System.Drawing.Point(0, 391);
            this.PLBPanel_Buttons.Name = "PLBPanel_Buttons";
            this.PLBPanel_Buttons.Size = new System.Drawing.Size(608, 28);
            this.PLBPanel_Buttons.TabIndex = 3;
            // 
            // PLBPanel_Button_Reset
            // 
            this.PLBPanel_Button_Reset.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.PLBPanel_Button_Reset.Dock = System.Windows.Forms.DockStyle.Right;
            this.PLBPanel_Button_Reset.Enabled = false;
            this.PLBPanel_Button_Reset.Location = new System.Drawing.Point(458, 0);
            this.PLBPanel_Button_Reset.Name = "PLBPanel_Button_Reset";
            this.PLBPanel_Button_Reset.Size = new System.Drawing.Size(75, 28);
            this.PLBPanel_Button_Reset.TabIndex = 4;
            this.PLBPanel_Button_Reset.Text = "Reset";
            this.PLBPanel_Button_Reset.UseVisualStyleBackColor = true;
            this.PLBPanel_Button_Reset.Click += new System.EventHandler(this.PLBPanel_Button_Reset__Click);
            // 
            // PLBPanel_Button_Save
            // 
            this.PLBPanel_Button_Save.Dock = System.Windows.Forms.DockStyle.Right;
            this.PLBPanel_Button_Save.Enabled = false;
            this.PLBPanel_Button_Save.Location = new System.Drawing.Point(533, 0);
            this.PLBPanel_Button_Save.Name = "PLBPanel_Button_Save";
            this.PLBPanel_Button_Save.Size = new System.Drawing.Size(75, 28);
            this.PLBPanel_Button_Save.TabIndex = 5;
            this.PLBPanel_Button_Save.Text = "Apply";
            this.PLBPanel_Button_Save.UseVisualStyleBackColor = true;
            this.PLBPanel_Button_Save.Click += new System.EventHandler(this.PLBPanel_Button_Save__Click);
            // 
            // MenuContext_Containee
            // 
            this.MenuContext_Containee.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuContext_Containee.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuContext_Containee_Remove,
            this.MenuContext_Containee_Add});
            this.MenuContext_Containee.Name = "MenuContext_Containee";
            this.MenuContext_Containee.Size = new System.Drawing.Size(293, 60);
            // 
            // MenuContext_Containee_Remove
            // 
            this.MenuContext_Containee_Remove.Enabled = false;
            this.MenuContext_Containee_Remove.Name = "MenuContext_Containee_Remove";
            this.MenuContext_Containee_Remove.Size = new System.Drawing.Size(292, 28);
            this.MenuContext_Containee_Remove.Text = "&Remove (Not Implemented)";
            // 
            // MenuContext_Containee_Add
            // 
            this.MenuContext_Containee_Add.Enabled = false;
            this.MenuContext_Containee_Add.Name = "MenuContext_Containee_Add";
            this.MenuContext_Containee_Add.Size = new System.Drawing.Size(292, 28);
            this.MenuContext_Containee_Add.Text = "&Add (Not Implemented)";
            // 
            // PlbEdit
            // 
            this.AcceptButton = this.PLBPanel_Button_Save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.PLBPanel_Button_Reset;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MainContainer);
            this.Controls.Add(this.MenuStrip_Main);
            this.MainMenuStrip = this.MenuStrip_Main;
            this.Name = "PlbEdit";
            this.Text = "plbEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlbEdit__FormClosing);
            this.MenuContext_Container.ResumeLayout(false);
            this.MenuStrip_Main.ResumeLayout(false);
            this.MenuStrip_Main.PerformLayout();
            this.MainContainer.Panel1.ResumeLayout(false);
            this.MainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).EndInit();
            this.MainContainer.ResumeLayout(false);
            this.PLBPanel_Buttons.ResumeLayout(false);
            this.MenuContext_Containee.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip_Main;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Open;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Save;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_SaveAs;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Quit;
        private System.Windows.Forms.TreeView PLBTree;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tools;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tools_Import;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tools_Export;
        private System.Windows.Forms.ToolStripMenuItem Menu_Help;
        private System.Windows.Forms.ToolStripMenuItem Menu_Help_About;
        private System.Windows.Forms.ToolStripMenuItem Menu_Help_Cmd;
        private System.Windows.Forms.SplitContainer MainContainer;
        private System.Windows.Forms.Panel PLBPanel_Buttons;
        private System.Windows.Forms.Button PLBPanel_Button_Reset;
        private System.Windows.Forms.Button PLBPanel_Button_Save;
        private System.Windows.Forms.Panel PLBPanel;
        private System.Windows.Forms.ContextMenuStrip MenuContext_Container;
        private System.Windows.Forms.ToolStripMenuItem MenuContext_Container_Add;
        private System.Windows.Forms.ContextMenuStrip MenuContext_Containee;
        private System.Windows.Forms.ToolStripMenuItem MenuContext_Containee_Remove;
        private System.Windows.Forms.ToolStripMenuItem MenuContext_Containee_Add;
    }
}

