using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewModdingAPI.Events;



namespace StardewConfigMenu.Components {
	abstract class SCMControl {
		static protected SCMControl SelectedComponent;

		internal bool IsActiveComponent => SelectedComponent == this;
		protected bool IsAvailableForSelection => (IsActiveComponent || SelectedComponent == null) && Visible;

		public virtual int Height { get; }
		public virtual int Width { get; }
		public virtual int X { get; }
		public virtual int Y { get; }

		private bool _visible = false;
		internal protected bool _enabled;
		internal protected string _label;

		virtual internal bool Visible {
			set => _visible = value;
			get => _visible;
		}

		public virtual bool Enabled {
			get => _enabled;
			protected set => _enabled = value;
		}

		public virtual string Label {
			get => _label;
			protected set => _label = value;
		}

		public SCMControl(string label, bool enabled = true) {
			_label = label;
			_enabled = enabled;
			AddListeners();
		}

		public SCMControl(string label, int x, int y, bool enabled = true) {
			_label = label;
			_enabled = enabled;
			AddListeners();
		}

		protected void RegisterAsActiveComponent() {
			SelectedComponent = this;
		}

		protected void UnregisterAsActiveComponent() {
			if (IsActiveComponent) {
				SelectedComponent = null;
			}
		}

		// For moving the component
		public virtual void draw(SpriteBatch b, int x, int y) { }

		public virtual void draw(SpriteBatch b) { }

		internal void AddListeners() {
			RemoveListeners();
		}

		internal void RemoveListeners() {
			UnregisterAsActiveComponent();
			Visible = false;
		}

		public virtual void receiveLeftClick(int x, int y, bool playSound = true) { }
		public virtual void receiveRightClick(int x, int y, bool playSound = true) { }
		public virtual void receiveKeyPress(Keys key) { }
		public virtual void leftClickHeld(int x, int y) { }
		public virtual void releaseLeftClick(int x, int y) { }
		public virtual void receiveScrollWheelAction(int direction) { }

	}
}
