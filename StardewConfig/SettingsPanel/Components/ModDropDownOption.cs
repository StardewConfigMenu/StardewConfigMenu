using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewConfigFramework;


namespace StardewConfigMenu
{
    internal class ModDropDownOption : DropDownComponent
    {
        private ModOptionSelection ModData;

        protected new List<string> dropDownOptions => ModData.List;

        public new int selectedOption
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

        public ModDropDownOption(ModOptionSelection option, int width) : base(option.LabelText, width)
        {
            this.ModData = option;

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
            var selected = dropDownDisplayOptions[DisplayedSelection];
            ModData.Selection = dropDownOptions.IndexOf(selected);
            base.SelectDisplayedOption(DisplayedSelection);
        }

    }
}
