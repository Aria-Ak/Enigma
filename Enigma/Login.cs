using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enigma
{
    public partial class Login : Form
    {

      

        public Login()
        {
            
            InitializeComponent();
            CenterToScreen();
        }


        private void Login_Load(object sender, EventArgs e)
        {
            
        }

        private void lblNewUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            var signupForm = new Sign_up();
            signupForm.Closed += (s, args) => this.Close();
            signupForm.Show();

        }


        private bool validate()
        {
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            int userid = DbConnection.getInstance().GetUserIdByName(txtUsername.Text);

            if (String.IsNullOrWhiteSpace(txtUsername.Text))
            {
                lblErrors.Text = "Username cannot be empty.";
                return false;
            }

            if (!regexItem.IsMatch(txtUsername.Text))
            {
                lblErrors.Text = "Username must be an alphnumeric value.";
                return false;
            }

            if (String.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblErrors.Text = "Password  cannot be empty.";
                return false;
            }

            if (txtUsername.Text.Length > 20)
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


            if (txtUsername.Text.Length < 4)
            {
                lblErrors.Text = "Username length cannot be less than 4 characters.";
                return false;
            }


            if (userid == 0)
            {
                lblErrors.Text = "This username does not exists.";
                return false;
            }


            if (!DbConnection.getInstance().verifyPassword(userid,txtPassword.Text))
            {
                lblErrors.Text = "Password is wrong!";
                return false;
            }



            return true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (validate()){

                int userid = DbConnection.getInstance().GetUserIdByName(txtUsername.Text);
                var config = new Config();
                config.LastUsername = txtUsername.Text;
                config.LastPassword = txtPassword.Text;

                if (chbRemember.Checked)
                    config.RememberMe = true;

                if (chbKeepLoggedIn.Checked)
                    config.StayLoggedIn = true;

                DbConnection.getInstance().InsertConfig(config);

                this.Hide();
                var enimga = new Enigma(userid, txtUsername.Text);
                enimga.Closed += (s, args) => this.Close();
                enimga.Show();
            }
        }

        private void Login_Shown(object sender, EventArgs e)
        {
            var config = DbConnection.getInstance().GetConfig();

            if (config.RememberMe)
            {
                txtUsername.Text = config.LastUsername;
                txtPassword.Text = config.LastPassword;
                chbRemember.Checked = true;

            }

            if (config.StayLoggedIn)
            {
                var id = DbConnection.getInstance().GetUserIdByName(config.LastUsername);

                this.Visible = false;
                var enimga = new Enigma(id, txtUsername.Text);
                enimga.Closed += (s, args) => this.Close();
                enimga.Show();

            }
        }
    }
}
