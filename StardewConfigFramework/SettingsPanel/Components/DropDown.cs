using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace StardewConfigFramework
{

	/// <summary>A dropdown UI component which lets the player choose from a list of values.</summary>
	internal class DropDownOption : OptionsDropDown
    {
        private ModOptionSelection Option;

		public virtual new int selectedOption
		{
            get { return Option.Selection; }
            private set {
                this.Option.MakeSelection(value);
            }
		}

        public virtual new List<string> dropDownOptions {
            get { return Option.List; }
        }

       

        public DropDownOption(ModOptionSelection option, int x = -1, int y = -1) : base(option.LabelText, option.Selection, x, y)
        {
            this.Option = option;
        }

    }


}

