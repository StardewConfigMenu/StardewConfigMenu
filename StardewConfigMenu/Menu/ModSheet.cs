using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewConfigFramework;
using StardewConfigMenu.UI;
using StardewValley;
using StardewValley.Menus;

namespace StardewConfigMenu {
	public class ModSheet: IClickableMenu {

		int SelectedTab = 0;
		int FirstShownTab = 0;
		IOptionsPackage Package;
		List<ModTab> Tabs = new List<ModTab>();
		List<SCMTexturedLabel> UITabs = new List<SCMTexturedLabel>();

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

			LoadTabs(package.Tabs);

			package.Tabs.ContentsDidChange += LoadTabs;
		}

		private void LoadTabs(ISCFOrderedDictionary<IOptionsTab> tabs) {
			Tabs.Clear();
			UITabs.Clear();
			foreach (IOptionsTab tab in tabs) {
				Tabs.Add(new ModTab(tab, xPositionOnScreen, yPositionOnScreen, width, height));
				UITabs.Add(new SCMTexturedLabel(tab.Label, Game1.smallFont));
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

		public override void receiveRightClick(int x, int y, bool playSound = true) {
			if (Tabs.Count < 1)
				return;

			Tabs[SelectedTab].receiveRightClick(x, y, playSound);
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true) {
			if (Tabs.Count < 1)
				return;

			Tabs[SelectedTab].receiveLeftClick(x, y, playSound);

			if (Tabs.Count < 2)
				return;

			for (int i = 0; i < UITabs.Count; i++) {
				if (UITabs[i].Bounds.Contains(x, y)) {
					Tabs[SelectedTab].Visible = false;
					SelectedTab = i;
					Tabs[i].Visible = true;
				}
			}
		}

		public override void leftClickHeld(int x, int y) {
			if (Tabs.Count < 1)
				return;

			Tabs[SelectedTab].leftClickHeld(x, y);
		}

		public override void releaseLeftClick(int x, int y) {
			if (Tabs.Count < 1)
				return;

			Tabs[SelectedTab].releaseLeftClick(x, y);
		}

		public override void receiveScrollWheelAction(int direction) {
			if (Tabs.Count < 1)
				return;

			Tabs[SelectedTab].receiveScrollWheelAction(direction);
		}

		public override void draw(SpriteBatch b) {
			if (Tabs.Count < 1)
				return;

			Tabs[SelectedTab].draw(b);

			if (Tabs.Count < 2)
				return;

			int tabPosition = 0;

			for (int i = FirstShownTab; i < Tabs.Count; i++) {
				var xOffset = Game1.pixelZoom * ((i == SelectedTab) ? 6 : 9);
				UITabs[i].DrawAt(b, xPositionOnScreen - UITabs[i].Width - xOffset, yPositionOnScreen + (UITabs[i].Height * tabPosition) + (tabPosition * Game1.pixelZoom));
				tabPosition++;
			}
		}
	}
}
