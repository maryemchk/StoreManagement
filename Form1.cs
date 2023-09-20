using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Linq.Expressions;
using System.IO;
using System.Xml.Linq;
using System.Drawing.Printing;




namespace myFirstProject
{
    public partial class Form1 : Form
    {
        public System.Windows.Forms.TextBox ModifiedDesignation;
        public System.Windows.Forms.TextBox ModifiedQuantite;
        private OleDbConnection connection = new OleDbConnection();
        public Form1() //constructeur 
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""C:\Users\21655\Desktop\Database1.accdb"";Persist Security Info=False";
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                checkConnection.Text = "la connexion est établie !";
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            string query = "SELECT * FROM PRODUIT";
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);

            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            dataGridView1.DataSource = dataTable;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }


        //**************************************************************************************************************

        private void button3_Click_1(object sender, EventArgs e)//supprimer
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                string code = dataGridView1.SelectedRows[0].Cells["Code"].Value.ToString();

                DialogResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet enregistrement ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        using (OleDbCommand command = new OleDbCommand("DELETE FROM PRODUIT WHERE Code = @Code", connection))
                        {
                            command.Parameters.AddWithValue("@Code", code);
                            command.ExecuteNonQuery();
                        }
                        connection.Close();

                        dataGridView1.Rows.RemoveAt(selectedIndex);

                        MessageBox.Show("Enregistrement supprimé avec succès.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur : " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une ligne à supprimer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //*************************************************************************************************************

        private void button1_Click(object sender, EventArgs e) // ajouter
        {
            this.Enabled = false; // Disable the current form

            AjouterForm ajouterForm = new AjouterForm();
            ajouterForm.StartPosition = FormStartPosition.CenterParent; // Set the child form to open in the center of the parent form
            ajouterForm.ShowDialog(this); // Pass the current form as the owner

            this.Enabled = true; // Enable the current form after the child form is closed or dismissed

            if (ajouterForm.DialogResult == DialogResult.OK)
            {
                int code = ajouterForm.Code;
                string designation = ajouterForm.Designation;
                int quantite = ajouterForm.Quantite;

                try
                {
                    connection.Open();

                    // Check if the product already exists
                    string checkQuery = "SELECT COUNT(*) FROM PRODUIT WHERE Code = @Code";
                    using (OleDbCommand checkCommand = new OleDbCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Code", code);// adds the value melekher 
                        int count = (int)checkCommand.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("Le produit avec le même code existe déjà.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            // Insert the product
                            string insertQuery = "INSERT INTO PRODUIT (Code, Designation, Quantite) VALUES (@Code, @Designation, @Quantite)";
                            using (OleDbCommand insertCommand = new OleDbCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@Code", code);
                                insertCommand.Parameters.AddWithValue("@Designation", designation);
                                insertCommand.Parameters.AddWithValue("@Quantite", quantite);
                                insertCommand.ExecuteNonQuery();
                            }

                            

                            MessageBox.Show("Produit ajouté avec succès.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur : " + ex.Message);
                }
                RefreshDataGridView();
            }
        }

        //*************************************************************************************************************
        private void RefreshDataGridView()
        {
            try
            {
                string query = "SELECT * FROM PRODUIT";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }
        //*************************************************************************************************************
        private void button2_Click(object sender, EventArgs e) // modifier
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\21655\Desktop\Database1.accdb;Persist Security Info=False";

            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int code = Convert.ToInt32(selectedRow.Cells["Code"].Value);

                // Find the product to modify
                string currentDesignation = selectedRow.Cells["Designation"].Value.ToString();
                int currentQuantite = Convert.ToInt32(selectedRow.Cells["Quantite"].Value);

                using (ModifierForm modifierForm = new ModifierForm(currentDesignation, currentQuantite))
                {
                    modifierForm.textBoxCode.Text = code.ToString(); // Set the code in the text box
                    modifierForm.textBoxCode.Enabled = false; // Disable the code textbox

                    modifierForm.textBoxDesignation.Text = currentDesignation;
                    modifierForm.textBoxQuantite.Text = currentQuantite.ToString();

                    // display the modifier form in the middle of the screen
                    int modifierX = (Screen.PrimaryScreen.WorkingArea.Width - modifierForm.Width) / 2;
                    int modifierY = (Screen.PrimaryScreen.WorkingArea.Height - modifierForm.Height) / 2;
                    modifierForm.StartPosition = FormStartPosition.Manual;
                    modifierForm.Location = new Point(modifierX, modifierY);

                    this.Enabled = false; // Disable the current form

                    DialogResult result = modifierForm.ShowDialog(this); // Pass the current form as the owner

                    this.Enabled = true; // Enable the current form after the modifier form is closed or dismissed

                    if (result == DialogResult.OK)
                    {
                        // Get the modified details from the modifier form
                        string modifiedDesignation = modifierForm.textBoxDesignation.Text;
                        int modifiedQuantite = int.Parse(modifierForm.textBoxQuantite.Text);

                        try
                        {
                            using (OleDbConnection connection = new OleDbConnection(connectionString))
                            {
                                connection.Open();
                                using (OleDbCommand command = new OleDbCommand("UPDATE PRODUIT SET Designation = @Designation, Quantite = @Quantite WHERE Code = @Code", connection))
                                {
                                    command.Parameters.AddWithValue("@Designation", modifiedDesignation);
                                    command.Parameters.AddWithValue("@Quantite", modifiedQuantite);
                                    command.Parameters.AddWithValue("@Code", code);

                                    command.ExecuteNonQuery();
                                }
                            }

                            RefreshDataGridView();

                            MessageBox.Show("Produit modifié avec succès.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur : " + ex.Message);
                        }
                    }
                }
            }
        }

        //****************************************************************************************************************************************
        private void button4_Click(object sender, EventArgs e) //imprimer
        {
            // Retrieve data from the database
            string query = "SELECT * FROM PRODUIT";
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            // Generate the PDF file
            using (PrintDocument printDocument = new PrintDocument())
            {
                int pageNumber = 1; // Variable to track the current page number

                printDocument.PrintPage += (s, ev) =>
                {
                    float x = ev.MarginBounds.Left;
                    float y = ev.MarginBounds.Top;

                    // Draw the society header on each page
                    string societyHeader = "Society: MY STORE ";
                    ev.Graphics.DrawString(societyHeader, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, x, y);
                    y += 30;

                    // Add space between the header and the table
                    y += 20;

                    // Draw a line under the society header
                    ev.Graphics.DrawLine(Pens.Black, x, y, ev.MarginBounds.Right, y);
                    y += 10;

                    // Draw the table headers
                    float columnHeaderX = x;
                    float columnHeaderY = y;

                    // Draw the "Code" header
                    string codeHeader = "Code";
                    ev.Graphics.DrawString(codeHeader, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, columnHeaderX, columnHeaderY);
                    columnHeaderX += 100;

                    // Draw a line under the "Code" header
                    float lineY = columnHeaderY + 20;
                    ev.Graphics.DrawLine(Pens.Black, x, lineY, ev.MarginBounds.Right, lineY);

                    // Draw the "Designation" header
                    string designationHeader = "Designation";
                    ev.Graphics.DrawString(designationHeader, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, columnHeaderX, columnHeaderY);
                    columnHeaderX += 300;

                    // Draw a line under the "Designation" header
                    ev.Graphics.DrawLine(Pens.Black, x, lineY, ev.MarginBounds.Right, lineY);

                    // Draw the "Quantite" header
                    string quantiteHeader = "Quantite";
                    ev.Graphics.DrawString(quantiteHeader, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, columnHeaderX, columnHeaderY);

                    y += 40;

                    // Draw a line under the table headers
                    float headerLineY = y;
                    ev.Graphics.DrawLine(Pens.Black, x, headerLineY, ev.MarginBounds.Right, headerLineY);

                    // Draw the table data
                    foreach (DataRow row in dataTable.Rows)
                    {
                        x = ev.MarginBounds.Left;

                        // Draw the "Code" value
                        string codeValue = row["Code"].ToString();
                        ev.Graphics.DrawString(codeValue, new Font("Arial", 12), Brushes.Black, x, y);
                        x += 100;

                        // Draw the vertical line between "Code" and "Designation"
                        float verticalLineX = x;
                        ev.Graphics.DrawLine(Pens.Black, verticalLineX, headerLineY, verticalLineX, ev.MarginBounds.Bottom);

                        // Draw the "Designation" value
                        string designationValue = row["Designation"].ToString();
                        ev.Graphics.DrawString(designationValue, new Font("Arial", 12), Brushes.Black, x, y);
                        x += 300;

                        // Draw the vertical line between "Designation" and "Quantite"
                        verticalLineX = x;
                        ev.Graphics.DrawLine(Pens.Black, verticalLineX, headerLineY, verticalLineX, ev.MarginBounds.Bottom);

                        // Draw the "Quantite" value
                        string quantiteValue = row["Quantite"].ToString();
                        ev.Graphics.DrawString(quantiteValue, new Font("Arial", 12), Brushes.Black, x, y);

                        y += 20;

                        // Check if the current row exceeds the page bounds
                        if (y + 20 > ev.MarginBounds.Bottom)
                        {
                            // Start a new page
                            ev.HasMorePages = true;
                            pageNumber++; // Increment the page number
                            return;
                        }
                    }

                    // Draw the page number
                    string pageNumberText = "Page " + pageNumber;
                    ev.Graphics.DrawString(pageNumberText, new Font("Arial", 10), Brushes.Black, ev.MarginBounds.Left, ev.MarginBounds.Bottom + 10);
                };

                PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                printPreviewDialog.Document = printDocument;

                // Center the print preview dialog on the MDI parent form
                printPreviewDialog.StartPosition = FormStartPosition.CenterParent;

                // Show the print preview dialog
                printPreviewDialog.ShowDialog();
            }
        }

       
    }
}