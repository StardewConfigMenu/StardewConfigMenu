
namespace StardewConfigFramework
{
    public delegate void ModOptionRangeHandler(string identifier, float currentValue);

    public class ModOptionRange : ModOption
    {
        public event ModOptionRangeHandler ValueChanged;

        public ModOptionRange(string identifier, string label, float min, float max, float defaultSelection, bool enabled = true) : base(identifier, label, enabled)
        {
            this.Value = defaultSelection;
        }

        public float min { get; private set; }
        public float max { get; private set; }
        public float Value { get; private set; }

        public void SetValue(float selection)
        {
            this.Value = CheckValidInput(selection);
            this.ValueChanged(this.identifier, this.Value);
        }

        public void SetValueByPercentage(float percent)
        {
            float newValue = ((max - min) * percent) + min;
            this.Value = CheckValidInput(newValue);
            this.ValueChanged(this.identifier, this.Value);
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