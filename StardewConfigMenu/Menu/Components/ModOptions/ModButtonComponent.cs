using StardewConfigFramework.Options;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewValley.Menus;

namespace StardewConfigMenu.Components.ModOptions {
	using ActionType = Action.ActionType;

	internal class ModButtonComponent: ButtonComponent {
		readonly Action ModData;

		public override bool Enabled => ModData.Enabled;
		public override string Label => ModData.Label;

		internal ModButtonComponent(Action option, int x, int y) : base(option.Label, option.Type, x, y, option.Enabled) {
			this.ModData = option;
		}

		internal ModButtonComponent(Action option) : base(option.Label, option.Type, option.Enabled) {
			this.ModData = option;
		}

		public override ActionType ActionType {
			get {
				if (ModData == null)
					return ActionType.OK;

				return this.ModData.Type;
			}
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true) {
			base.receiveLeftClick(x, y, playSound);

			if (this.Button.containsPoint(x, y) && Enabled && this.IsAvailableForSelection()) {
				if (playSound)
					Game1.playSound("breathin");
				this.ModData.Trigger();
			}
		}
	}
}
