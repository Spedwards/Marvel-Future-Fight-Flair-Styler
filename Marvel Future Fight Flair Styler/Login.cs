using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Marvel_Future_Fight_Flair_Styler {
	public partial class Login : Form {
		private String Username;
		private String Password;

		public Login() {
			InitializeComponent();
		}

		private void btnLogin_Click(object sender, EventArgs e) {
			Username = txtUsername.Text;
			Password = txtPassword.Text;
			this.Close();
		}

		public String getUsername() {
			return Username;
		}

		public String getPassword() {
			return Password;
		}
	}
}
