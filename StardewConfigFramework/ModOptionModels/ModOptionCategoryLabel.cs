using Newtonsoft.Json;

namespace StardewConfigFramework {
	public class ModOptionCategoryLabel: ModOption {
		[JsonConstructor]
		public ModOptionCategoryLabel(string identifier, string label) : base(identifier, label) {

		}
	}
}
