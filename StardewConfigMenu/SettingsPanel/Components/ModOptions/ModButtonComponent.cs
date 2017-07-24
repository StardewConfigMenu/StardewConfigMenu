using StardewConfigFramework;
using StardewValley;
using Microsoft.Xna.Framework;

namespace StardewConfigMenu.Panel.Components.ModOptions
{
	internal class ModButtonComponent: ButtonComponent
	{
		readonly ModOptionTrigger Option;

		public override bool enabled
		{
			get {
				return Option.enabled;
			}
		}

		public override string label
		{
			get {
				return Option.LabelText;
			}
		}

		internal ModButtonComponent(ModOptionTrigger option, int x, int y) : base(option.LabelText, option.type, x, y, option.enabled)
		{
			this.Option = option;
		}

		internal ModButtonComponent(ModOptionTrigger option) : base(option.LabelText, option.type, option.enabled)
		{
			this.Option = option;
		}

		public override OptionActionType ActionType
		{
			get {
				if (Option == null)
					return _ActionType;
				if (this.Option.type != _ActionType)
				{
					_ActionType = this.Option.type;
					this.bounds.Width = (int) buttonScale * this.buttonSource.Width;
					this.bounds.Height = (int) buttonScale * this.buttonSource.Height;
				}
				return this.Option.type;
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true) {
			//throw new NotImplementedException();
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
