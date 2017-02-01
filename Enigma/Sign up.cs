using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Enigma
{
    public partial class Sign_up : Form
    {
        public Sign_up()
        {
            
            InitializeComponent();  
            CenterToScreen();
        }

        private void btnSignUp_Click(object sender, System.EventArgs e)
        {

           if (validate())
            {
                var db = DbConnection.getInstance();

                if (db.IsUsernameExist(txtusername.Text.ToLower()))
                {
                    lblErrors.Text = "This username already exists.";
                } else
                {
                    db.InsertUser(txtusername.Text.ToLower(),txtPassword.Text);
                    MessageBox.Show("Your account has created.");
                    this.Hide();
                    var loginForm = new Login();
                    loginForm.Closed += (s, args) => this.Close();
                    loginForm.Show();
                }
            }
        }


        private bool validate()
        {
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

            if (String.IsNullOrWhiteSpace(txtusername.Text))
            {
                lblErrors.Text = "Name  cannot be empty.";
                return false;
            }


            if (!regexItem.IsMatch(txtusername.Text))
            {
                lblErrors.Text = "Username must be an alphnumeric value.";
                return false;
            }

            if (String.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblErrors.Text = "Password  cannot be empty.";
                return false;
            }

            if (String.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                lblErrors.Text = "Confirm password  cannot be empty.";
                return false;
            }

        

            if (txtusername.Text.Length > 20 )
            {
                lblErrors.Text = "Username length cannot exceed 20 characters.";
                return false;
            }

            if (txtPassword.Text.Length > 20)
            {
                lblErrors.Text = "Password length cannot exceed 20 characters.";
                return false;
            }



            if (txtPassword.Text.Length < 8)
            {
                lblErrors.Text = "Password length cannot be less than 8 characters.";
                return false;
            }


            if (txtusername.Text.Length < 4)
            {
                lblErrors.Text = "Username length cannot be less than 4 characters.";
                return false;
            }


            if (txtConfirmPassword.Text != txtPassword.Text)
            {
                lblErrors.Text = "Passwords do not match.";
                return false;
            }

            return true;
        }
    }
}
