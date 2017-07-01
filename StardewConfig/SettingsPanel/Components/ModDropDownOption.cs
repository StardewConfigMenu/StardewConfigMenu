using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewConfigFramework;


namespace StardewConfigMenu.SettingsPanel.Components
{
    internal class ModDropDownOption : DropDownComponent
    {
        private ModOptionSelection Option;

        public ModDropDownOption(ModOptionSelection option, int width) : base(option.LabelText, width)
        {
            this.Option = option;
            // LongestString = option.List.GroupBy(str => str == null ? 0 : str.Length).OrderByDescending(g => g.Key).First();
        }

    }
}
