
namespace StardewConfigFramework {
	public delegate void ModOptionToggleHandler(string identifier, bool isOn);

	public class ModOptionToggle: ModOption {

		public event ModOptionToggleHandler ValueChanged;

		public ModOptionToggle(string identifier, string labelText, bool isOn = true, bool enabled = true) : base(identifier, labelText, enabled) {
			this.IsOn = isOn;
		}

		public bool IsOn {
			get {
				return _isOn;
			}
			set {
				if (_isOn == value)
					return;

				this._isOn = value;
				this.ValueChanged?.Invoke(this.identifier, this.IsOn);
			}
		}

		private bool _isOn;

		public void Toggle() {
			this.IsOn = !this.IsOn;
		}
	}
}
