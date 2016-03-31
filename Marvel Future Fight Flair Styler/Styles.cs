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
	public partial class Styles : Form {
		private String CSS;

		public Styles() {
			InitializeComponent();
		}

		private void btnSubmitStyles_Click(object sender, EventArgs e) {
			CSS = txtStyles.Text;
			this.Close();
		}

		public String getCSS() {
			return CSS;
		}
	}
}
