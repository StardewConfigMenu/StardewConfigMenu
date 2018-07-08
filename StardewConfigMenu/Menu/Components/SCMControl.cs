using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewConfigMenu.Components {

	abstract class SCMControl {
		static SCMControl ActiveComponent;

		internal bool IsActiveComponent => ActiveComponent == this;
		protected bool IsAvailableForSelection => (IsActiveComponent || ActiveComponent == null) && Visible;

		public virtual int Height { get; set; }
		public virtual int Width { get; set; }
		public virtual int X { get; set; }
		public virtual int Y { get; set; }

		private bool _visible = false;
		internal protected bool _enabled;
		internal protected string _label;

		virtual internal bool Visible {
			set {
				_visible = value;
				UnregisterAsActiveComponent();
			}
			get => _visible;
		}

		public virtual bool Enabled => _enabled;

		public virtual string Label => _label;

		public SCMControl(string label, bool enabled = true) {
			_label = label;
			_enabled = enabled;
		}

		protected void RegisterAsActiveComponent() {
			ActiveComponent = this;
		}

		protected void UnregisterAsActiveComponent() {
			if (IsActiveComponent) {
				ActiveComponent = null;
			}
		}

		// For moving the component
		public virtual void Draw(SpriteBatch b, int x, int y) {
			X = x;
			Y = y;
			Draw(b);
		}
		public virtual void Draw(SpriteBatch b) { }

		public virtual void ReceiveLeftClick(int x, int y, bool playSound = true) { }
		public virtual void ReceiveRightClick(int x, int y, bool playSound = true) { }
		public virtual void ReceiveKeyPress(Keys key) { }
		public virtual void LeftClickHeld(int x, int y) { }
		public virtual void ReleaseLeftClick(int x, int y) { }
		public virtual void ReceiveScrollWheelAction(int direction) { }

	}
}
