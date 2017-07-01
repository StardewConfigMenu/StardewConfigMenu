using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewConfigMenu
{
    internal class OptionComponent
    {
        static protected OptionComponent selected;

        protected bool IsAvailableForSelection()
        {
            if (OptionComponent.selected == this || OptionComponent.selected == null)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
