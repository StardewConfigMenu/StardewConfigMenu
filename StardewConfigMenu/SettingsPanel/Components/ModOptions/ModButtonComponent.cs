using StardewConfigFramework.Options;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewValley.Menus;

namespace StardewConfigMenu.Components.ModOptions {
	using ActionType = Action.ActionType;
	internal class ModButtonComponent: ButtonComponent {
		readonly Action Option;

		public override bool enabled {
			get {
				return Option.Enabled;
			}
		}

		public override string label {
			get {
				return Option.Label;
			}
		}

		internal ModButtonComponent(Action option, int x, int y) : base(option.Label, option.Type, x, y, option.Enabled) {
			this.Option = option;
		}

		internal ModButtonComponent(Action option) : base(option.Label, option.Type, option.Enabled) {
			this.Option = option;
		}

		public override ActionType ActionType {
			get {
				if (Option == null)
					return _ActionType;

				return this.Option.Type;
			}
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true) {
			base.receiveLeftClick(x, y, playSound);

			if (this.button.containsPoint(x, y) && enabled && this.IsAvailableForSelection()) {
				if (playSound)
					Game1.playSound("breathin");
				this.Option.Trigger();
			}
		}

		/*
		protected override void leftClicked(int x, int y)
		{
			base.leftClicked(x, y);

			if (this.bounds.Contains(x, y) && enabled && this.IsAvailableForSelection())
			{
				Game1.playSound("breathin");
				this.Option.Trigger();
			}
		} */
	}
}
