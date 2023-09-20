using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace myFirstProject
{
    public partial class MDIParent1 : Form
    {
        private int childFormNumber = 0;

        public MDIParent1()
        {
            InitializeComponent();
            Load += MDIParent1_Load; // Assign the Load event handler

        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void produitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.MdiParent = this;
            form1.Dock = DockStyle.Fill; // Fill the entire area of the MDI parent form


            form1.Show();
        }



        private void MDIParent1_Load(object sender, EventArgs e)
        {
            // Calculate the desired size of the MDI parent form
            int parentWidth = (int)(Screen.PrimaryScreen.WorkingArea.Width * 0.8); // 80% of the screen width
            int parentHeight = (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.8); // 80% of the screen height
            Size parentSize = new Size(parentWidth, parentHeight);

            // Set the size of the MDI parent form
            this.Size = parentSize;

            // Calculate the location to display the MDI parent form in the middle of the screen
            int parentX = (Screen.PrimaryScreen.WorkingArea.Width - parentSize.Width) / 2;
            int parentY = (Screen.PrimaryScreen.WorkingArea.Height - parentSize.Height) / 2;
            Point parentLocation = new Point(parentX, parentY);

            // Set the location of the MDI parent form
            this.Location = parentLocation;

            // Adjust the child form position
            AdjustChildFormPosition();
        }

        private void AdjustChildFormPosition()
        {
            foreach (Form childForm in this.MdiChildren)
            {
                // Calculate the center position within the MDI parent form
                int childX = (this.ClientSize.Width - childForm.Width) / 2;
                int childY = (this.ClientSize.Height - childForm.Height) / 2;
                childForm.StartPosition = FormStartPosition.Manual;
                childForm.Location = new Point(childX, childY);
            }
        }


    }
}