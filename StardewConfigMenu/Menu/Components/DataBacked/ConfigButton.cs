using StardewConfigFramework.Options;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewValley.Menus;

namespace StardewConfigMenu.Components.DataBacked {
	using ActionType = Action.ActionType;

	internal class ConfigButton: SCMButton {
		readonly Action ModData;

		public override string Label { get => ModData.Label; protected set => ModData.Label = value; }
		public override bool Enabled { get => ModData.Enabled; protected set => ModData.Enabled = value; }

		public override ActionType ActionType { get => ModData.Type; }

		internal ConfigButton(Action option) : this(option, 0, 0) { }

		internal ConfigButton(Action option, int x, int y) : base(option.Label, option.Type, x, y, option.Enabled) {
			ModData = option;
		}
	}
}
