namespace Marvel_Future_Fight_Flair_Styler {
	partial class Styles {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.txtStyles = new System.Windows.Forms.TextBox();
			this.btnSubmitStyles = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtStyles
			// 
			this.txtStyles.Location = new System.Drawing.Point(11, 12);
			this.txtStyles.MaxLength = 0;
			this.txtStyles.Multiline = true;
			this.txtStyles.Name = "txtStyles";
			this.txtStyles.Size = new System.Drawing.Size(326, 306);
			this.txtStyles.TabIndex = 0;
			// 
			// btnSubmitStyles
			// 
			this.btnSubmitStyles.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSubmitStyles.Location = new System.Drawing.Point(102, 324);
			this.btnSubmitStyles.Name = "btnSubmitStyles";
			this.btnSubmitStyles.Size = new System.Drawing.Size(144, 44);
			this.btnSubmitStyles.TabIndex = 1;
			this.btnSubmitStyles.Text = "Submit Stylesheet";
			this.btnSubmitStyles.UseVisualStyleBackColor = true;
			this.btnSubmitStyles.Click += new System.EventHandler(this.btnSubmitStyles_Click);
			// 
			// Styles
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(349, 375);
			this.ControlBox = false;
			this.Controls.Add(this.btnSubmitStyles);
			this.Controls.Add(this.txtStyles);
			this.Name = "Styles";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Submit Stylesheet";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtStyles;
		private System.Windows.Forms.Button btnSubmitStyles;
	}
}