using System;
using System.Collections;

namespace PIPS.PAGIS.Db.HotLists
{
	/// <summary>
	/// Summary description for FuzzyLogic.
	/// </summary>
	public class FuzzyLogic : IVrmFilter
	{
		private bool enabled;
		private Hashtable fuzzyHash;
		private IniFile ini;
		private string alphabet;
			
		public FuzzyLogic()
		{
			this.fuzzyHash = new Hashtable();
			ini = new IniFile(System.Environment.CurrentDirectory + @"\FuzzyLogic.ini");
			alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

			foreach(char c in alphabet) {
				this.fuzzyHash[c] = ini.ReadString("Mappings", c.ToString(), string.Empty);
			}
			this.enabled = ini.ReadBoolean("FuzzyLogic", "Enabled", true);
			
		}

		private void SetFuzzyMapping(char c, string mapping) {
			ini.WriteString("Mappings", c.ToString(), mapping);
			this.fuzzyHash[c] = mapping;
		}
		private void ResetFuzzyMappings() {
			ini.DeleteSection("Mappings");
			foreach(char c in alphabet) {
				this.fuzzyHash[c] = string.Empty;
			}
		}

		private void BuildArrayList(ArrayList vrms, string vrm , string cvrm) {
			if(cvrm.Length == vrm.Length) {
				vrms.Add(cvrm);
				return;
			}
			char next = vrm[cvrm.Length];
			string rule = (string)this.fuzzyHash[next];
			if((rule != null) && (rule != string.Empty)) {
				foreach(char c in rule)
					this.BuildArrayList(vrms, vrm, cvrm + c);
			} else {
				this.BuildArrayList(vrms, vrm, cvrm + next);
			}

		}

		public string Filter(string vrm) {
			if(this.IsEnabled) {
				//if(vrm.IndexOfAny(new char[] {'%', '_', '*', '?'}) < 0) {
					ArrayList vrms = new ArrayList();
					this.BuildArrayList(vrms, vrm, string.Empty);
					if(vrms.Count > 1) {
						string filter = "('" + (string)vrms[0] + "'";
						for(int i = 1; i < vrms.Count; i++)
							filter += ",'" + (string)vrms[i] + "'";
						//PIPS.Logger.WriteLine("FuzzyLogic.Filter({0}, {1}))", vrm, filter);
						return filter + ")";
					}
				//}
			}
			return vrm;
		}

		/*public string Filter(string vrm) {
			if(this.IsEnabled) {
				vrm = vrm.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
				string filter = string.Empty;
				foreach(char c in vrm) {
					string map = (string)this.fuzzyHash[c];
					if(map != null) {
						if(map == string.Empty) {
							filter += c;
						} else {
							filter += "[" + map + "]";
						}
					}
				}
				PIPS.Logger.WriteLine("FuzzyLogic.Filter({0}, {1})", vrm, filter);
				return filter;
			}
			return vrm;
		}*/

		public bool IsEnabled {
			get {
				return this.enabled;
			}
			set {
				ini.WriteBoolean("FuzzyLogic", "Enabled", value);
				this.enabled = value;
			}
		}

		public string[] FuzzyMappings {
			get {
				string alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
				ArrayList fs = new ArrayList();
				foreach(char c in alphabet) {
					string map = (string)this.fuzzyHash[c];
					if(map != string.Empty) {
						if(!fs.Contains(map)) {
							fs.Add(map);
						}
					}
				}
				return (string[])fs.ToArray(typeof(string));
			}
			set {
				this.ResetFuzzyMappings();
				if(value != null) {
					foreach(string mapping in value) {
						//PIPS.Logger.WriteLine("FuzzyLogic.SetMapping({0})", mapping);
						string fix = string.Empty;
						foreach(char c in mapping) {
							if(Char.IsLetterOrDigit(c)) {
								fix += c.ToString().ToUpper(System.Globalization.CultureInfo.InvariantCulture);
							}
						}
						//PIPS.Logger.WriteLine("FuzzyLogic.SetMapping({0}, {1})", mapping, fix);
						if(fix != string.Empty) {
							foreach(char c in fix) {
								this.SetFuzzyMapping(c, fix);
							}
						}
					}
				}
			}
		}
		}
}
