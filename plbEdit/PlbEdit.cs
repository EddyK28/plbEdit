using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.Serialization;
using plbEdit.Format;
using System.Runtime.Serialization.Json;
using plbEdit.Controls;

/* UI Layout
 *   TreeList
 *   
 *     Sections
 *       Section 1
 *         group Refs
 *           ...
 *         Items
 *           ...
 *         Maps
 *           ...
 *       Section 2
 *         ...
 *         
 *     Groups
 *       ???
 *       
 *     Layers
 *       Layer 1
 *       ...
 */

namespace plbEdit
{
    public partial class PlbEdit : Form
    {
        private PLB data;
        private string pathCurrent;
        private string fileCurrent;

        public PlbEdit()
        {
            InitializeComponent();
        }

        private void PlbEdit__FormClosing(object sender, FormClosingEventArgs e)
        {
            //unsaved changes check
            if (IsDirty())
            {
                DialogResult temp = MessageBox.Show(
                    "Unsaved changes exist. Do you wish to save them?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
                if (temp == DialogResult.Cancel) e.Cancel = true;
                else if (temp == DialogResult.Yes) Menu_File_Save__Click(sender, e);
            }
        }

        #region Menu Events
        private void Menu_File_Open__Click(object sender, EventArgs e)
        {
            //unsaved changes check
            if (IsDirty())
            {
                DialogResult temp = MessageBox.Show(
                    "Unsaved changes exist. Do you wish to save them?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
                if (temp == DialogResult.Cancel) return;
                else if (temp == DialogResult.Yes) Menu_File_Save__Click(sender, e);
            }

            using (OpenFileDialog OpenDlg = new OpenFileDialog())
            {
                OpenDlg.Filter = "PMD Placement Data|*.plb";

                if (OpenDlg.ShowDialog() == DialogResult.OK)
                {
                    OpenFile(OpenDlg.FileName);
                }
            }
        }

        private void Menu_File_Save__Click(object sender, EventArgs e)
        {
            if (pathCurrent == null)
            {
                Menu_File_SaveAs__Click(sender, e);
                return;
            }

            if (data != null)
            {
                Stream file = null;
                try
                {
                    MessagePrinter.ClearMsg();
                    file = new FileStream(pathCurrent, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    data.Build(new PLBWriter(file));
                    SetDirty(false);
                    MessagePrinter.ShowMsg("There were some issues while saving:", "Warning!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(            //show error message
                    "An error occured while saving:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
                finally
                {
                    file?.Close();
                }
            }
        }

        private void Menu_File_SaveAs__Click(object sender, EventArgs e)
        {
            if (pathCurrent != null) pathCurrent = "";
            if (data != null)
            {
                using (SaveFileDialog SaveDlg = new SaveFileDialog())
                {
                    SaveDlg.Filter = "PMD Placement Data|*.plb";
                    SaveDlg.FileName = Path.GetFileName(pathCurrent);

                    if (SaveDlg.ShowDialog() == DialogResult.OK)
                    {
                        Stream file = null;
                        try
                        {
                            MessagePrinter.ClearMsg();
                            pathCurrent = SaveDlg.FileName;                          //store file path
                            file = new FileStream(pathCurrent, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                            data.Build(new PLBWriter(file));
                            fileCurrent = Path.GetFileName(pathCurrent);
                            SetDirty(false);
                            MessagePrinter.ShowMsg("There were some issues while saving:", "Warning!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(            //show error message
                            "An error occured while saving:\n" + ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        }
                        finally
                        {
                            file?.Close();
                        }
                    }
                }
            }
        }

        private void Menu_File_Quit__Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void Menu_Tools_Import__Click(object sender, EventArgs e)
        {
            //unsaved changes check
            if (IsDirty())
            {
                DialogResult temp = MessageBox.Show(
                    "Unsaved changes exist. Do you wish to save them?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
                if (temp == DialogResult.Cancel) return;
                else if (temp == DialogResult.Yes) Menu_File_Save__Click(sender, e);
            }

            using (OpenFileDialog OpenDlg = new OpenFileDialog())
            {
                OpenDlg.Filter = "PMD Placement Data JSON|*.json|PMD Placement Data XML|*.xml";

                if (OpenDlg.ShowDialog() == DialogResult.OK)
                {
                    Stream file = null;
                    try
                    {
                        fileCurrent = OpenDlg.SafeFileName + " (Imported)";
                        file = new FileStream(OpenDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        DeserializePLB(file, ref data, OpenDlg.FilterIndex);

                        pathCurrent = null;
                        SetDirty();
                        SetLoaded();
                        CreateTree();
                    }
                    catch (Exception ex)
                    {
                        data = null;                //release PLB object

                        MessageBox.Show(            //show error message
                            "An error occured while opening:\n" + ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    finally
                    {
                        file?.Close();
                    }
                }
            }
        }

        private void Menu_Tools_Export__Click(object sender, EventArgs e)
        {
            if (pathCurrent != null || data != null)
            {
                using (SaveFileDialog SaveDlg = new SaveFileDialog())
                {
                    SaveDlg.Filter = "PMD Placement Data JSON|*.json|PMD Placement Data XML|*.xml";
                    SaveDlg.FileName = Path.GetFileName(pathCurrent);

                    if (SaveDlg.ShowDialog() == DialogResult.OK)
                    {
                        Stream file = null;
                        try
                        {
                            file = new FileStream(SaveDlg.FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                            SerializePLB(file, data, SaveDlg.FilterIndex);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(            //show error message
                            "An error occured while exporting:\n" + ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        }
                        finally
                        {
                            file?.Close();
                        }
                    }
                }
            }
        }


        private void Menu_Help_About__Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "An experimental PMD 3/4 placement data editor.\nBy EddyK28",
                "plbEdit- About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void Menu_Help_Cmd__Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Usage: plbEdit <option> <input file> <output file>\n" +
                    "Options:\n" +
                    "    /wj\t/writeJson\tOpen a PLB file and write as JSON\n" +
                    "    /wx\t/writeXml\t\tOpen a PLB file and write as XML\n" +
                    "    /rj\t/readJson\t\tOpen a PLB JSON and convert to PLB\n" +
                    "    /rx\t/readXml\t\tOpen a PLB XML and convert to PLB\n" +
                    "    /t\t/test\t\tOpen and re-save a PLB file for testing",
                "plbEdit- CMD Help",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        #endregion


        private void PLBTree__Select(object sender, TreeViewEventArgs e)
        {
            PLBPanel.Controls.Clear();

            PLBNode node = null;
            try { node = (PLBNode)PLBTree.SelectedNode; }
            catch (Exception ex)
            {
                PLBPanel_Button_Save.Enabled = false;
                PLBPanel_Button_Reset.Enabled = false;
                return;
            }

            PLBPanel_Button_Save.Enabled = true;
            PLBPanel_Button_Reset.Enabled = true;

            PLBControlBase plbControl = null;

            if (node.item is Section)
                plbControl = new PLBControlSection(node);
            else if (node.item is Group)
                plbControl = new PLBControlGroup(node);
            else if (node.item is Layer)
                plbControl = new PLBControlLayer(node);
            else if (node.item is GroupRef)
                plbControl = new PLBControlGroupref(node);
            else if (node.item is ItemEntry)
                plbControl = new PLBControlItem(node);
            else if (node.item is MapEntry)
                plbControl = new PLBControlMap(node);
            else if (node.item is GroupSection)
                plbControl = new PLBControlGroupSection(node);

            if (plbControl != null)
            {
                plbControl.Dock = DockStyle.Fill;
                PLBPanel.Controls.Add(plbControl);
            }
        }

        private void PLBPanel_Button_Save__Click(object sender, EventArgs e)
        {
            if (PLBPanel.Controls.Count < 1) return;
            PLBControlBase inputs = (PLBControlBase)PLBPanel.Controls[0];
            try
            {
                if (inputs != null) inputs.SaveData();
                SetDirty();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Error!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                inputs.InitData();
            }
        }

        private void PLBPanel_Button_Reset__Click(object sender, EventArgs e)
        {
            if (PLBPanel.Controls.Count < 1) return;
            PLBControlBase inputs = (PLBControlBase)PLBPanel.Controls[0];
            if (inputs != null) inputs.InitData();
        }

        private void SetDirty(bool en = true)
        {
            Menu_File_Save.Enabled = en;
            //Button_Save.Enabled = en;
            
            if (en)
                Text = $"plbEdit - {fileCurrent}*";
            else
                Text = "plbEdit - " + fileCurrent;
        }


        private bool IsDirty()
        {
            return Menu_File_Save.Enabled;
        }

        private void SetLoaded(bool ld = true)
        {
            Menu_File_SaveAs.Enabled = ld;
            Menu_Tools_Export.Enabled = ld;
        }

        private void CreateTree()
        {
            TreeNode parent = null;
            PLBNode tempNode = null;

            parent = PLBTree.Nodes["Node_Sect"];
            parent.Nodes.Clear();
            if (data.Sections != null)
                foreach (Section section in data.Sections)
                    parent.Nodes.Add(CreateTreeSection(section));

            parent = PLBTree.Nodes["Node_Group"];
            parent.Nodes.Clear();
            if (data.Groups != null)
                foreach (Group group in data.Groups)
                    parent.Nodes.Add(CreateTreeGroup(group, "Node_Group"));

            parent = PLBTree.Nodes["Node_Lyr"];
            parent.Nodes.Clear();
            if (data.Layers != null)
                foreach (Layer layer in data.Layers)
                {
                    tempNode = new PLBNode(layer.Name);
                    tempNode.Name = "Node_Lyr_" + layer.Name;
                    tempNode.ContextMenuStrip = MenuContext_Containee;
                    tempNode.item = layer;
                    parent.Nodes.Add(tempNode);
                }
        }

        private PLBNode CreateTreeSection(Section section)
        {
            PLBNode sectNode = new PLBNode(section.Name);
            PLBNode tempNode = null;
            TreeNode[] tempNodes = new TreeNode[3];

            sectNode.Name = "Node_Section_" + section.Name;
            sectNode.ContextMenuStrip = MenuContext_Containee;
            sectNode.item = section;

            TreeNode groupNode = new TreeNode(sectNode.Name + "_GroupRefs");
            groupNode.Text = "Group Refs";
            groupNode.ContextMenuStrip = MenuContext_Container;
            if (section.GroupRefs != null)
                foreach (GroupRef gref in section.GroupRefs)
                {
                    tempNode = new PLBNode(gref.gref);

                    sectNode.Name = groupNode.Name + '_' + tempNode.Name;
                    tempNode.ContextMenuStrip = MenuContext_Containee;
                    tempNode.item = gref;

                    groupNode.Nodes.Add(tempNode);
                }
                    

            TreeNode itemNode = new TreeNode(sectNode.Name + "_Items");
            itemNode.Text = "Items";
            itemNode.ContextMenuStrip = MenuContext_Container;
            if (section.Items != null)
                foreach (ItemEntry item in section.Items)
                {
                    tempNode = new PLBNode(item.Id);

                    sectNode.Name = itemNode.Name + '_' + tempNode.Name;
                    tempNode.ContextMenuStrip = MenuContext_Containee;
                    tempNode.item = item;

                    itemNode.Nodes.Add(tempNode);
                }


            TreeNode mapNode = new TreeNode(sectNode.Name + "_Maps");
            mapNode.Text = "Maps";
            mapNode.ContextMenuStrip = MenuContext_Container;
            if (section.Maps != null)
                foreach (MapEntry map in section.Maps)
                {
                    tempNode = new PLBNode(map.Id);

                    sectNode.Name = mapNode.Name + '_' + tempNode.Name;
                    tempNode.ContextMenuStrip = MenuContext_Containee;
                    tempNode.item = map;

                    mapNode.Nodes.Add(tempNode);
                }


            sectNode.Nodes.AddRange(new TreeNode[] {groupNode, itemNode, mapNode});

            return sectNode;
        }

        private PLBNode CreateTreeGroup(Group group, string prefix)
        {
            PLBNode groupNode = new PLBNode(group.Id);
            PLBNode tempNode = null;
            TreeNode[] tempNodes = new TreeNode[3];

            groupNode.Name = prefix +'_'+ group.Id;
            groupNode.ContextMenuStrip = MenuContext_Containee;
            groupNode.item = group;

            TreeNode sectNode = new TreeNode(groupNode.Name + "_Sections");
            sectNode.ContextMenuStrip = MenuContext_Container;
            sectNode.Text = "Sections";
            if (group.Sections != null)
                foreach (GroupSection section in group.Sections)
                    sectNode.Nodes.Add(CreateTreeGroupSect(section, sectNode.Name));

            TreeNode itemNode = new TreeNode(groupNode.Name + "_Items");
            itemNode.ContextMenuStrip = MenuContext_Container;
            itemNode.Text = "Items";
            if (group.Items != null)
                foreach (ItemEntry item in group.Items)
                {
                    tempNode = new PLBNode(item.Id);

                    tempNode.Name = itemNode.Name + '_' + tempNode.Name;
                    tempNode.ContextMenuStrip = MenuContext_Containee;
                    tempNode.item = item;

                    itemNode.Nodes.Add(tempNode);
                }


            //TODO: group ukns nodes?
            //TreeNode mapNode = new TreeNode(groupNode.Name + "_Maps");
            //mapNode.Text = "Maps";
            //if (section.Maps != null)
            //    foreach (MapEntry map in section.Maps)
            //    {
            //        tempNode = new PLBNode(map.Label);
            //
            //        groupNode.Name = mapNode.Name + '_' + tempNode.Name;
            //        tempNode.item = map;
            //
            //        mapNode.Nodes.Add(tempNode);
            //    }


            groupNode.Nodes.AddRange(new TreeNode[] { sectNode, itemNode });

            return groupNode;
        }

        private PLBNode CreateTreeGroupSect(GroupSection section, string prefix)
        {
            PLBNode sectNode = new PLBNode(section.Id);
            PLBNode tempNode = null;

            sectNode.Name = prefix + '_' + sectNode.Name;
            sectNode.ContextMenuStrip = MenuContext_Containee;
            sectNode.item = section;

            TreeNode groupNode = new TreeNode(sectNode.Name + "_Groups");
            groupNode.ContextMenuStrip = MenuContext_Container;
            groupNode.Text = "Groups";
            if (section.Groups != null)
                foreach (Group group in section.Groups)
                    groupNode.Nodes.Add(CreateTreeGroup(group, sectNode.Name));

            TreeNode itemNode = new TreeNode(sectNode.Name + "_Items");
            itemNode.ContextMenuStrip = MenuContext_Container;
            itemNode.Text = "Items";
            if (section.Items != null)
                foreach (ItemEntry item in section.Items)
                {
                    tempNode = new PLBNode(item.Id);

                    tempNode.Name = itemNode.Name + '_' + tempNode.Name;
                    tempNode.ContextMenuStrip = MenuContext_Containee;
                    tempNode.item = item;

                    itemNode.Nodes.Add(tempNode);
                }

            sectNode.Nodes.AddRange(new TreeNode[] { groupNode, itemNode });
            return sectNode;
        }

        public static void SerializePLB(Stream output, PLB data, int mode)
        {
            if (mode == 1)
            {
                var ser = new DataContractJsonSerializer(typeof(PLB));
                using (var jsn = JsonReaderWriterFactory.CreateJsonWriter(output, System.Text.Encoding.UTF8, true, true))
                {
                    ser.WriteObject(jsn, data);
                }
            }
            else
            {
                var ser = new DataContractSerializer(typeof(PLB));
                using (XmlWriter xw = XmlWriter.Create(output, new XmlWriterSettings { Indent = true }))
                {
                    ser.WriteObject(xw, data);
                }
            }
        }

        public static void DeserializePLB(Stream input, ref PLB data, int mode)
        {
            if (mode == 1)
            {
                var ser = new DataContractJsonSerializer(typeof(PLB));
                data = (PLB)ser.ReadObject(input);
            }
            else
            {
                var ser = new DataContractSerializer(typeof(PLB));
                data = (PLB)ser.ReadObject(input);
            }
        }

        public void OpenFile(string path)
        {
            Stream file = null;
            try
            {
                MessagePrinter.ClearMsg();
                PLBPanel.Controls.Clear();
                file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                data = new PLB(new PLBReader(file));
                pathCurrent = path;
                fileCurrent = Path.GetFileName(path);
                SetLoaded();
                SetDirty(false);
                MessagePrinter.ShowMsg("There were some issues while opening:", "Warning!");
                CreateTree();
            }
            catch (Exception ex)
            {
                data = null;

                MessageBox.Show(            //show error message
                    "An error occured while opening:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                file?.Close();
            }
        }
    }

    public class PLBNode : TreeNode
    {
        public object item;  //ref?

        public PLBNode() : base() { }
        public PLBNode(string text) : base(text) { }
        public PLBNode(string text, TreeNode[] children) : base(text, children) { }
    }
}
