using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewConfigFramework;
using Microsoft.Xna.Framework.Graphics;


namespace StardewConfigMenu.Panel.Components.ModOptions
{
    internal class ModDropDownComponent : DropDownComponent
    {
        private ModOptionSelection ModData;

        public override bool enabled
        {
            get
            {
                if (!ModData.enabled)
                    return ModData.enabled;
                else
                    return dropDownOptions.Count != 0;
            }
        }

        public override int selectedOption
        {
            get { return dropDownOptions.FindIndex(x => x == this.dropDownDisplayOptions[0]); }
            set
            {
                if (selectedOption == value || value + 1 > dropDownOptions.Count || value < 0)
                    return;
                dropDownDisplayOptions.Remove(dropDownOptions[value]);
                dropDownDisplayOptions.Insert(0, dropDownOptions[value]);
                this.ModData.Selection = value;
            }
        }

        public ModDropDownComponent(ModOptionSelection option, int width) : base(option.LabelText, width)
        {
            this.ModData = option;
            base.dropDownOptions = ModData.List;

            ReloadData();
            this.SelectDisplayedOption(option.Selection);
            // LongestString = option.List.GroupBy(str => str == null ? 0 : str.Length).OrderByDescending(g => g.Key).First();
        }

        public void ReloadData()
        {
            this.dropDownDisplayOptions.Clear();
            for (int i = 0; i < ModData.List.Count; i++)
            {                   
                this.AddOption(ModData.List[i]);
            }


            
        }

        protected override void SelectDisplayedOption(int DisplayedSelection)
        {
            if (dropDownOptions.Count == 0)
                return;

            var selected = dropDownDisplayOptions[DisplayedSelection];
            ModData.Selection = dropDownOptions.IndexOf(selected);
            base.SelectDisplayedOption(DisplayedSelection);
        }

    }
}
