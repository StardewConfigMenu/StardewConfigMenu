using Newtonsoft.Json;

namespace StardewConfigFramework
{
	public delegate void ModOptionTriggerHandler(string identifier);

	public class ModOptionTrigger: ModOption
	{

		public event ModOptionTriggerHandler ActionTriggered;

		[JsonConstructor]
		public ModOptionTrigger(string identifier, string labelText, OptionActionType type, bool enabled = true) : base(identifier, labelText, enabled)
		{
			this.type = type;
		}

		public void Trigger()
		{
			this.ActionTriggered?.Invoke(this.identifier);
		}

		public OptionActionType type;

	}

	public enum OptionActionType
	{
		OK, SET, CLEAR, DONE, GIFT
	}

}
