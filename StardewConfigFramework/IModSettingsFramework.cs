
namespace StardewConfigFramework {

	public delegate void ModAddedSettings();

	public abstract class IModSettingsFramework {
		public static IModSettingsFramework Instance { get; protected set; }
		public abstract void AddModOptions(ModOptions modOptions);
		/* Not Yet
		public abstract void SaveModOptions(ModOptions options);
		public abstract ModOptions LoadModOptions(Mod mod);
		*/
	}

}
