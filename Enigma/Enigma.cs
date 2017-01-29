using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Enigma
{
    public partial class frmMain : Form
    {
        private DbConnection db = new DbConnection();

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (validate())
            {

             
                var secretItem = new Secret();

                secretItem.Name = txtName.Text;
                secretItem.Username = txtUsername.Text;
                secretItem.Password = txtPassword.Text;
                secretItem.Remarks= txtRemarks.Text;
                
                

                lstSecrets.Items.Add(secretItem);
            }
        }

        public bool validate()
        {
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                lblErrors.Text = "Name  cannot be empty.";
                return false;
            }

            return true;
        }

    }
}
