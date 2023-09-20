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
    public partial class AjouterForm : Form
    {
        public int Code { get; private set; }
        public string Designation { get; private set; }
        public int Quantite { get; private set; }

        public AjouterForm()
        {
            InitializeComponent();
        }

        private void AjouterForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxCode.Text, out int code) &&
           int.TryParse(textBoxQuantite.Text, out int quantite))
            {
                Code = code;
                Designation = textBoxDesignation.Text;
                Quantite = quantite;

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Veuillez entrer des valeurs numériques valides pour le code et la quantité.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
    
}
