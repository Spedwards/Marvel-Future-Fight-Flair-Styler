using RedditSharp;
using RedditSharp.Things;

using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using dotless.Core;
using dotless.Core.configuration;

namespace Marvel_Future_Fight_Flair_Styler {
	public partial class frmMain : Form {

		public Reddit r = new Reddit();
		Subreddit sub = null;
		String subredditCSS = "";
		public Boolean loggedIn = false;

		public frmMain() {
			InitializeComponent();


			populateCharacters();
			populateLists();
		}

		private void cboLeftChar_SelectedIndexChanged(object sender, EventArgs e) {
			cboLeftCharUniform.SelectedIndex = -1;
			cboLeftCharUniform.Items.Clear();
			Character selected = Character.getCharacter(cboLeftChar.SelectedIndex);
			if (selected.hasUniform()) {
				lblLeftCharUniform.Visible = true;
				cboLeftCharUniform.Visible = true;
				foreach (Uniform uni in selected.getUniforms()) {
					cboLeftCharUniform.Items.Add(uni.getName());
				}
			} else {
				lblLeftCharUniform.Visible = false;
				cboLeftCharUniform.Visible = false;
			}
		}

		private void cboCenterChar_SelectedIndexChanged(object sender, EventArgs e) {
			cboCenterCharUniform.SelectedIndex = -1;
			cboCenterCharUniform.Items.Clear();
			Character selected = Character.getCharacter(cboCenterChar.SelectedIndex);
			if (selected.hasUniform()) {
				lblCenterCharUniform.Visible = true;
				cboCenterCharUniform.Visible = true;
				foreach (Uniform uni in selected.getUniforms()) {
					cboCenterCharUniform.Items.Add(uni.getName());
				}
			} else {
				lblCenterCharUniform.Visible = false;
				cboCenterCharUniform.Visible = false;
			}
		}

		private void cboRightChar_SelectedIndexChanged(object sender, EventArgs e) {
			cboRightCharUniform.SelectedIndex = -1;
			cboRightCharUniform.Items.Clear();
			Character selected = Character.getCharacter(cboRightChar.SelectedIndex);
			if (selected.hasUniform()) {
				lblRightCharUniform.Visible = true;
				cboRightCharUniform.Visible = true;
				foreach (Uniform uni in selected.getUniforms()) {
					cboRightCharUniform.Items.Add(uni.getName());
				}
			} else {
				lblRightCharUniform.Visible = false;
				cboRightCharUniform.Visible = false;
			}
		}

		private void mnitLogin_Click(object sender, EventArgs e) {
			Login login = new Login();
			if (login.ShowDialog() == DialogResult.OK) {
				try {
					r.LogIn(login.getUsername(), login.getPassword());
					loggedIn = true;

					sub = r.GetSubreddit("FutureFight");
					subredditCSS = sub.Stylesheet.CSS;

					mnitLogin.Enabled = false;
					mnitUpdate.Enabled = false;

					btnGenerateCSS.Text = "Update CSS";
				} catch (AuthenticationException ex) {
					MessageBox.Show("Invalid Login!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void mnitUpdate_Click(object sender, EventArgs e) {
			Styles styles = new Styles();
			if (styles.ShowDialog() == DialogResult.OK) {
				subredditCSS = styles.getCSS();
			}
		}

		private void mnitAbout_Click(object sender, EventArgs e) {
			About about = new About();
			about.ShowDialog();
		}

		private void btnGenerateCSS_Click(object sender, EventArgs e) {
			/* Check for blank character fields */
			if (txtUsername.Text == "" || cboLeftChar.SelectedIndex == -1 || cboCenterChar.SelectedIndex == -1 || cboRightChar.SelectedIndex == -1) {
				MessageBox.Show("Please assure all fields have been entered!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			String username = txtUsername.Text;
			Character leftChar = Character.getCharacter(cboLeftChar.SelectedIndex);
			Character centerChar = Character.getCharacter(cboCenterChar.SelectedIndex);
			Character rightChar = Character.getCharacter(cboRightChar.SelectedIndex);

			/* Check for blank uniform fields */
			if ((leftChar.hasUniform() && cboLeftCharUniform.SelectedIndex == -1) || (centerChar.hasUniform() && cboCenterCharUniform.SelectedIndex == -1) || (rightChar.hasUniform() && cboRightCharUniform.SelectedIndex == -1)) {
				MessageBox.Show("Please assure all fields have been entered!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			int leftUniformIndex = cboLeftCharUniform.SelectedIndex;
			int centerUniformIndex = cboCenterCharUniform.SelectedIndex;
			int rightUniformIndex = cboRightCharUniform.SelectedIndex;

			/* Get CSS names */
			String usernameCSS = Regex.Replace(username, "[-_]", "");
			String leftCharCSS;
			String centerCharCSS;
			String rightCharCSS;

			if (leftUniformIndex == -1) {
				leftCharCSS = leftChar.getCSSName();
			} else {
				leftCharCSS = leftChar.getUniforms()[leftUniformIndex].getCSSName();
			}

			if (centerUniformIndex == -1) {
				centerCharCSS = centerChar.getCSSName();
			} else {
				centerCharCSS = centerChar.getUniforms()[centerUniformIndex].getCSSName();
			}

			if (rightUniformIndex == -1) {
				rightCharCSS = rightChar.getCSSName();
			} else {
				rightCharCSS = rightChar.getUniforms()[rightUniformIndex].getCSSName();
			}
			
			String CSS = ".flair.flair-" + usernameCSS + " {" + Environment.NewLine + "	!flairgroup(" + leftCharCSS + "," + centerCharCSS + "," + rightCharCSS + ");" + Environment.NewLine + "}";
			String _LESS = Regex.Replace(CSS, @"(?:!flairgroup\((.+?),\s*(.+?),\s*(.+?)\);)", Callback);

			if (loggedIn) {
				subredditCSS = sub.Stylesheet.CSS;
			}

			if (subredditCSS == "") {
				MessageBox.Show("Missing subreddit CSS! Please update!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			String p1 = @"(\.flair\.flair-USERNAME(::(before|after))?,)?\.flair\.flair-USERNAME(::(before|after))?{.+?}";
			String p2 = @"\.flair\.flair-USERNAME(::(before|after))?,";

			p1 = p1.Replace("USERNAME", usernameCSS);
			p2 = p2.Replace("USERNAME", usernameCSS);

			subredditCSS = Regex.Replace(subredditCSS, p1, "");
			subredditCSS = Regex.Replace(subredditCSS, p2, "");

			subredditCSS += _LESS;

			DotlessConfiguration config = new DotlessConfiguration();
			config.MinifyOutput = true;
			config.StrictMath = true;
			subredditCSS = Less.Parse(subredditCSS, config);

			if (loggedIn) {
				sub.Stylesheet.CSS = subredditCSS;
				sub.Stylesheet.UpdateCSS("add flair " + username);
				Clipboard.SetText(subredditCSS); // Temporary fix.
				sub.SetUserFlair(username, usernameCSS, "");
			} else {
				// txtCSSOutput.Text = subredditCSS;
				Clipboard.SetText(subredditCSS);
				MessageBox.Show("New subreddit CSS is on your clipboard.", "Updated!", MessageBoxButtons.OK);
				MessageBox.Show("Set CSS class for " + username + "'s flair to \"" + usernameCSS + "\" (without quotes)", "Updated!", MessageBoxButtons.OK);
			}
		}

		static string Callback(Match match) {
			String groupFunc = ".flairthree;\n\t.flair.flair-@b;\n\t&::before { .flair.flair-@a; }\n\t&::after  { .flair.flair-@c; }";
			String p1 = Regex.Replace(groupFunc, "@a", match.Groups[1].Value);
			String p2 = Regex.Replace(p1, "@b", match.Groups[2].Value);
			String p3 = Regex.Replace(p2, "@c", match.Groups[3].Value);
			return p3;
		}

		private static void populateCharacters() {
			new Character("Angela", new List<Uniform> {
				new Uniform("Modern", "angela"),
				new Uniform("Secret Wars: 1602 Witch Hunter", "angela-sw"),
				new Uniform("All-New, All-Different", "angela-ac")
			});
			new Character("Ant-Man", new List<Uniform> {
				new Uniform("Modern", "antman"),
				new Uniform("Marvel's Ant-Man", "antman-am")
			});
			new Character("Black Bolt", new List<Uniform> {
				new Uniform("Modern", "blackbolt"),
				new Uniform("All-New, All-Different", "blackbolt-sv")
			});
			new Character("Black Cat", new List<Uniform> {
				new Uniform("Modern", "blackcat"),
				new Uniform("Claws", "blackcat-hw")
			});
			new Character("Black Dwarf", "blackdwarf");
			new Character("Black Panther", "blackpanther");
			new Character("Black Widow", new List<Uniform> {
				new Uniform("The Avengers", "blackwidow"),
				new Uniform("Avengers: Age of Ultron", "blackwidow-aou"),
				new Uniform("Secret Wars: 2099", "blackwidow-sw")
			});
			new Character("Blade", new List<Uniform> {
				new Uniform("Modern", "blade"),
				new Uniform("70's Classic", "blade-hw")
			});
			new Character("Bullseye", new List<Uniform> {
				new Uniform("Modern", "bullseye"),
				new Uniform("Secret Wars: 1872", "bullseye-sw")
			});
			new Character("Captain America", new List<Uniform> {
				new Uniform("The Avengers", "captainamerica"),
				new Uniform("Avengers: Age of Ultron", "captainamerica-aou"),
				new Uniform("Secret Wars: 2099", "captainamerica-sw"),
				new Uniform("Captain America: The Winter Soldier", "captainamerica-sv")
			});
			new Character("Captain Marvel", new List<Uniform> {
				new Uniform("Modern", "captainmarvel"),
				new Uniform("Secret Wars: Captain Marvel & The Carol Corps", "captainmarvel-sw"),
				new Uniform("Ms. Marvel", "captainmarvel-sv")
			});
			new Character("Carnage", "carnage");
			new Character("Corvus Glaive", "corvusglaive");
			new Character("Daisy Johnson", new List<Uniform> {
				new Uniform("Marvel's Agents of S.H.I.E.L.D.", "daisyjohnson"),
				new Uniform("Modern", "daisyjohnson-aos"),
				new Uniform("Marvel's Agents of S.H.I.E.L.D. (Quake)", "daisyjohnson-jj")
			});
			new Character("Daredevil", new List<Uniform> {
				new Uniform("Modern", "daredevil"),
				new Uniform("Devil of Hell's Kitchen", "daredevil-jj")
			});
			new Character("Deathlok", new List<Uniform> {
				new Uniform("Marvel's Agents of S.H.I.E.L.D.", "deathlok"),
				new Uniform("Modern", "deathlok-aos")
			});
			new Character("Destroyer", new List<Uniform> {
				new Uniform("Classic", "destroyer"),
				new Uniform("Prometheus", "destroyer-bo")
			});
			new Character("Doctor Octopus", new List<Uniform> {
				new Uniform("Classic", "droc"),
				new Uniform("Superior Spider-Man", "droc-jj")
			});
			new Character("Drax", new List<Uniform> {
				new Uniform("Guardians of the Galaxy", "drax"),
				new Uniform("All-New, All-Different", "drax-ac")
			});
			new Character("Elektra", "elektra");
			new Character("Elsa Bloodstone", new List<Uniform> {
				new Uniform("Modern", "elsabloodstone"),
				new Uniform("Secret Wars: Marvel Zombies", "elsabloodstone-bo")
			});
			new Character("Falcon", new List<Uniform> {
				new Uniform("Captain America: Winter Soldier", "falcon"),
				new Uniform("All-New: Captain America", "falcon-allnew")
			});
			new Character("Gamora", new List<Uniform> {
				new Uniform("Guardians of the Galaxy", "gamora"),
				new Uniform("All-New, All-Different", "gamora-sv")
			});
			new Character("Ghost Rider", new List<Uniform> {
				new Uniform("Classic", "ghostrider"),
				new Uniform("70's Classic", "ghostrider-hw")
			});
			new Character("Giant-Man", new List<Uniform> {
				new Uniform("Modern", "giantman"),
				new Uniform("Modern (Goliath)", "giantman-am")
			});
			new Character("Green Goblin", new List<Uniform> {
				new Uniform("Classic", "greengoblin"),
				new Uniform("Ultimate", "greengoblin-jf")
			});
			new Character("Groot", new List<Uniform> {
				new Uniform("Guardians of the Galaxy", "groot"),
				new Uniform("Secret Wars: Thors", "groot-sw")
			});
			new Character("Hawkeye", new List<Uniform> {
				new Uniform("The Avengers", "hawkeye"),
				new Uniform("Avengers: Age of Ultron", "hawkeye-aou")
			});
			new Character("Hulk", new List<Uniform> {
				new Uniform("The Avengers", "hulk"),
				new Uniform("Secret Wars: Future Imperfect", "hulk-sw"),
				new Uniform("World War Hulk", "hulk-ac")
			});
			new Character("Hulk (Amadeus Cho)", "hulkamadeuscho");
			new Character("Iron Fist", new List<Uniform> {
				new Uniform("Classic", "ironfist"),
				new Uniform("New Avengers", "ironfist-jj")
			});
			new Character("Iron Man", new List<Uniform> {
				new Uniform("Classic", "ironman"),
				new Uniform("Avengers: Age of Ultron", "ironman-aou"),
				new Uniform("Secret Wars: 2099", "ironman-sw")
			});
			new Character("Hulkbuster (Iron Man Mark 43", new List<Uniform> {
				new Uniform("Avengers: Age of Ultron", "hulkbuster"),
				new Uniform("Heavy Duty Armour", "hulkbuster-bo")
			});
			new Character("Jessica Jones", "jessicajones");
			new Character("Ms. Marvel (Kamala Khan)", "kamalakhan");
			new Character("Kingpin", new List<Uniform> {
				new Uniform("Modern", "kingpin"),
				new Uniform("Secret Wars: Armour Wars", "kingpin-sw")
			});
			new Character("Lash", new List<Uniform> {
				new Uniform("Marvel's Agents of S.H.I.E.L.D.", "lash"),
				new Uniform("Modern", "lash-hw")
			});
			new Character("Lincoln Campbell", "lincoln");
			new Character("Loki", new List<Uniform> {
				new Uniform("The Avengers", "loki"),
				new Uniform("Lady Loki", "loki-ac")
			});
			new Character("Luke Cage", new List<Uniform> {
				new Uniform("Modern", "lukecage"),
				new Uniform("All-New, All-Different", "lukecage-jf")
			});
			new Character("Malekith", "malekith");
			new Character("Mockingbird", new List<Uniform> {
				new Uniform("Heroic Age", "mockingbird"),
				new Uniform("Marvel's Agents of S.H.I.E.L.D.", "mockingbird-aos")
			});
			new Character("M.O.D.O.K.", new List<Uniform> {
				new Uniform("Classic", "modok"),
				new Uniform("SPIDOC", "modok-hw")
			});
			new Character("Nebula", "nebula");
			new Character("Phil Coulson", "philcoulson");
			new Character("Proxima Midnight", "proximamidnight");
			new Character("Punisher", new List<Uniform> {
				new Uniform("Modern", "punisher"),
				new Uniform("Noir", "punisher-hw")
			});
			new Character("Red Hulk", "redhulk");
			new Character("Red Skull", "redskull");
			new Character("Rocket Raccoon", new List<Uniform> {
				new Uniform("Guardians of the Galaxy", "rocketraccoon"),
				new Uniform("All-New, All-Different", "rocketraccoon-ac")
			});
			new Character("Ronan", "ronan");
			new Character("Sharon Carter", "sharoncarter");
            new Character("She-Hulk", new List<Uniform> {
				new Uniform("Secret Wars: A-Force", "shehulk"),
				new Uniform("All-New", "shehulk-sw")
			});
			new Character("Sif", new List<Uniform> {
				new Uniform("Original", "sif"),
				new Uniform("Portrait Change", "sif2")
			});
			new Character("Silk", "silk");
			new Character("Singularity", "singularity");
			new Character("Sister Grimm", new List<Uniform> {
				new Uniform("Secret Wars: A-Force", "sistergrimm"),
				new Uniform("All-New, All-Different", "sistergrimm-sv")
			});
			new Character("Spider-Gwen", "spidergwn");
			new Character("Spider-Man", new List<Uniform> {
				new Uniform("Classic", "spiderman"),
				new Uniform("Secret Wars: Renew Your Vows", "spiderman-sw"),
				new Uniform("All-New, All-Different", "spiderman-sv")
			});
			new Character("Spider-Man (Miles Morales)", "milesmorales");
			new Character("Star-Lord", new List<Uniform> {
				new Uniform("Guardians of the Galaxy", "starlord"),
				new Uniform("Space Armour", "starlord-jf")
			});
			new Character("Thor", new List<Uniform> {
				new Uniform("The Avengers", "thor"),
				new Uniform("Avengers: Age of Ultron", "thor-aou")
			});
			new Character("Thor (Jane Foster)", "janefoster");
			new Character("Ultron", new List<Uniform> {
				new Uniform("Modern", "ultron"),
				new Uniform("Avengers: Age of Ultron (Prime)", "ultron-aou"),
				new Uniform("Avengers: Age of Ultron (Mark 1)", "ultron-mk1"),
				new Uniform("Avengers: Age of Ultron (Mark 3)", "ultron-mk3")
			});
			new Character("Venom", new List<Uniform> {
				new Uniform("Classic", "venom"),
				new Uniform("Secret Wars: Marvel Zombies", "venom-sw")
			});
			new Character("Vision", new List<Uniform> {
				new Uniform("Modern", "vision"),
				new Uniform("Avengers: Age of Ultron", "vision-aou")
			});
			new Character("War Machine", new List<Uniform> {
				new Uniform("Modern", "warmachine"),
				new Uniform("Iron Patriot", "warmachine-ip"),
				new Uniform("Avengers: The Initiative", "warmachine-sv")
			});
			new Character("Warwolf", "warwolf");
			new Character("Winter Soldier", "wintersoldier");
			new Character("Yellowjacket", "yellowjacket");
			new Character("Yondu", "yondu");
		}

		public void populateLists() {
			foreach (String str in Character.getCharacters()) {
				cboLeftChar.Items.Add(str);
				cboCenterChar.Items.Add(str);
				cboRightChar.Items.Add(str);
			}
		}
	}
}