using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace StardewConfigFramework
{
    public delegate void ModOptionRangeHandler(float currentValue);

    public class ModOptionRange : ModOption
    {
        public event ModOptionRangeHandler ValueChanged;

        public ModOptionRange(string labelText, float min, float max, float defaultSelection, bool enabled = true) : base(labelText, enabled)
        {
            this.Value = defaultSelection;
        }

        public float min { get; private set; }
        public float max { get; private set; }
        public float Value { get; private set; }

        public void SetValue(float selection)
        {
            this.Value = CheckValidInput(selection);
            this.ValueChanged(this.Value);
        }

        public void SetValueByPercentage(float percent)
        {
            float newValue = ((max - min) * percent) + min;
            this.Value = CheckValidInput(newValue);
            this.ValueChanged(this.Value);
        }

        private float CheckValidInput(float input)
        {
            if (input > max)
                return max;

            if (input < min)
                return min;

            return input;
        }

    }
}