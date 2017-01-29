using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
    }
}
