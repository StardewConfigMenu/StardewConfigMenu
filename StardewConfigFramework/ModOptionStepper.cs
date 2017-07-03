using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace StardewConfigFramework
{
    public delegate void ModOptionStepperHandler(decimal currentValue);

    public class ModOptionStepper : ModOption
    {
        public event ModOptionStepperHandler ValueChanged;

        public ModOptionStepper(string identifier, string labelText, decimal min, decimal max, decimal stepsize, decimal defaultSelection, bool enabled = true) : base(identifier, labelText, enabled)
        {
            this.min = Math.Round(min, 3);
            this.max = Math.Round(max, 3);
            this.stepSize = Math.Round(stepsize, 3);

            var valid = CheckValidInput(Math.Round(defaultSelection, 3));
            this.Value = valid - ((valid - min) % stepSize);

        }

        public decimal min { get; private set; }
        public decimal max { get; private set; }
        public decimal Value { get; private set; }
        public decimal stepSize { get; private set; }

        public void SetValue(decimal selection)
        {
            var valid = CheckValidInput(Math.Round((decimal)selection, 3));
            var newVal = valid - ((valid - min) % stepSize);
            if (newVal != this.Value)
            {
                this.Value = newVal;
                this.ValueChanged(this.Value);
            }
            
        }

        public void StepUp()
        {
            this.SetValue(this.Value + this.stepSize);
        }

        public void StepDown()
        {
            this.SetValue(this.Value - this.stepSize);
        }

        private decimal CheckValidInput(decimal input)
        {
            if (input > max)
                return max;

            if (input < min)
                return min;

            return input;
        }

    }
}