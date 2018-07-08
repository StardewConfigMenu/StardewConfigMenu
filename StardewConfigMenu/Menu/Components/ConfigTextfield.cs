using System;
using StardewConfigFramework.Options;

namespace StardewConfigMenu.Components {
	sealed class ConfigTextfield: SCMControl {
		readonly IConfigString ModData;

		public ConfigTextfield(IConfigString option) : base(option.Label, option.Enabled) {
			ModData = option;
		}
	}
}
