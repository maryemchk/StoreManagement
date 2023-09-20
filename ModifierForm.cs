using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myFirstProject
{

    public partial class ModifierForm : Form
    {
        

        private void ModifierForm_Load(object sender, EventArgs e)
        {

        }
        public ModifierForm(string designation, int quantite)
        {
            InitializeComponent();

            // Set the initial values for the text boxes
            textBoxDesignation.Text = designation;
            textBoxQuantite.Text = quantite.ToString();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string modifiedDesignation = textBoxDesignation.Text;

            if (int.TryParse(textBoxQuantite.Text, out int quantite))
            {
                textBoxQuantite.Text = quantite.ToString();
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Veuillez entrer une valeur numérique valide pour la quantité.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void ModifiedDesignation_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxCode_TextChanged(object sender, EventArgs e)
        {
           

        }
    }
}
