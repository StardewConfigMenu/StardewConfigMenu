using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using StardewConfigFramework.Options;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework.Input;

namespace StardewConfigMenu.Components.ModOptions {

	internal class ModCheckBoxComponent: CheckBoxComponent {
		//
		// Static Fields
		//
		//public const int pixelsHigh = 11;

		//
		// Fields
		//
		readonly private Toggle Option;

		public override bool IsChecked {
			get { return Option.IsOn; }
			protected set {
				if (Option == null)
					return; // used to ignore base class assignment
				this.Option.IsOn = value;
			}
		}

		public override bool Enabled {
			get {
				return Option.Enabled;
			}
		}

		public override string Label {
			get {
				return Option.Label;
			}
		}

		internal ModCheckBoxComponent(Toggle option, int x, int y) : base(option.Label, option.IsOn, x, y, option.Enabled) {
			this.Option = option;
		}

		internal ModCheckBoxComponent(Toggle option) : base(option.Label, option.IsOn, option.Enabled) {
			this.Option = option;
		}

	}

}

