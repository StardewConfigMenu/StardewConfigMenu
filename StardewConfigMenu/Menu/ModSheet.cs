using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StardewConfigFramework;
using StardewValley.Menus;

namespace StardewConfigMenu {
	public class ModSheet: IClickableMenu {

		int SelectedTab = 0;
		IOptionsPackage Package;
		List<ModTab> Tabs = new List<ModTab>();

		private bool _visible = false;

		internal bool Visible {
			set {
				if (!value)
					Tabs.ForEach(x => {
						x.Visible = value;
					});
				else
					setVisibleTabs();

				_visible = value;
			}
			get => _visible;
		}

		private void setVisibleTabs() {
			for (int i = 0; i < Tabs.Count; i++) {
				Tabs[i].Visible = (i == SelectedTab);
			}
		}

		public ModSheet(IOptionsPackage package, int x, int y, int width, int height) : base(x, y, width, height) {
			Package = package;

			foreach (IOptionsTab tab in package.Tabs) {
				Tabs.Add(new ModTab(tab, x, y, width, height));
			}
		}

		public void AddListeners() {
			RemoveListeners();
		}

		public void RemoveListeners(bool children = false) {
			if (children) {
				Tabs.ForEach(x => { x.RemoveListeners(children); });
			}
		}

		public override void draw(SpriteBatch b) {
			if (Tabs.Count < 1)
				return;

			Tabs[SelectedTab].draw(b);
		}
	}
}
