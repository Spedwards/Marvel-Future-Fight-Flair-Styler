using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvel_Future_Fight_Flair_Styler {
	public class Uniform {
		private String _name;
		private String _cssName;

		public Uniform(String name, String cssName) {
			this._name = name;
			this._cssName = cssName;
		}

		public static Uniform newUniform(String name, String cssName) {
			return new Uniform(name, cssName);
		}

		public String getName() {
			return this._name;
		}

		public String getCSSName() {
			return this._cssName;
		}

	}
}
