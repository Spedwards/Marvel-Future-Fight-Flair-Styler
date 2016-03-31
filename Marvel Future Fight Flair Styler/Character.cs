using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvel_Future_Fight_Flair_Styler {
	public class Character {
		private static List<Character> _characters = new List<Character>();

		private String _name;
		private String _cssName;
		private Boolean _hasUniform;
		private List<Uniform> _uniforms;

		public Character(String characterName, String cssName) {
			this._name = characterName;
			this._cssName = cssName;
			this._hasUniform = false;
			Character._characters.Add(this);
		}

		public Character(String characterName, List<Uniform> characterUniforms) {
			this._name = characterName;
			this._hasUniform = true;
			this._uniforms = characterUniforms;
			Character._characters.Add(this);
		}

		public String getName() {
			return this._name;
		}

		public String getCSSName() {
			return this._cssName;
		}

		public Boolean hasUniform() {
			return this._hasUniform;
		}

		public List<Uniform> getUniforms() {
			return this._uniforms;
		}

		public static Character getCharacter(String characterName) {
			foreach (Character c in _characters) {
				if (characterName.Equals(c.getName())) {
					return c;
				}
			}
			return null;
		}

		public static Character getCharacter(int index) {
			return _characters[index];
		}

		public static List<String> getCharacters() {
			List<String> characters = new List<String>();
			foreach (Character c in _characters) {
				characters.Add(c.getName());
			}
			return characters;
		}
	}
}