using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using StardewModdingAPI;

namespace StardewConfigFramework.Serialization {
	internal class ModOptionsConverter: JsonConverter {
		/*********
        ** Accessors
        *********/
		/// <summary>Whether this converter can write JSON.</summary>
		public override bool CanWrite => false;


		/*********
		** Public methods
		*********/
		/// <summary>Get whether this instance can convert the specified object type.</summary>
		/// <param name="objectType">The object type.</param>
		public override bool CanConvert(Type objectType) {
			return objectType == typeof(ModOptions);
		}

		/// <summary>Reads the JSON representation of the object.</summary>
		/// <param name="reader">The JSON reader.</param>
		/// <param name="objectType">The object type.</param>
		/// <param name="existingValue">The object being read.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			// semantic version
			if (objectType == typeof(ModOptions)) {

				JToken token = JToken.Load(reader);

				if (token.Type == JTokenType.Object) {
					JObject jsonModOptions = (JObject) token;
					JObject jsonVersion = jsonModOptions.Value<JObject>(nameof(ModOptions.OptionsVersion));

					int major = jsonVersion.Value<int>(nameof(Version.Major));
					int minor = jsonVersion.Value<int>(nameof(Version.Minor));
					int revision = jsonVersion.Value<int>(nameof(Version.Revision));
					string build = jsonVersion.Value<string>(nameof(Version.Build));

					SemanticVersion version = new SemanticVersion(major, minor, revision, build);

					var jsonList = jsonModOptions.Value<JArray>(nameof(ModOptions.List));

					List<ModOption> optionList = new List<ModOption>();

					foreach (JObject option in jsonList) {
						string identifier = option.Value<string>(nameof(ModOption.identifier));
						string labelText = option.Value<string>(nameof(ModOption.LabelText));
						bool enabled = option.Value<bool>(nameof(ModOption.enabled));

						object checker;

						if ((checker = option.Value<bool?>(nameof(ModOptionToggle.IsOn))) != null) {

							optionList.Add(new ModOptionToggle(identifier, labelText, (bool) checker, enabled));

						} else if ((checker = option.Value<bool?>(nameof(ModOptionRange.showValue))) != null) {

							decimal min = option.Value<decimal>(nameof(ModOptionRange.min));
							decimal max = option.Value<decimal>(nameof(ModOptionRange.max));
							decimal selection = option.Value<decimal>(nameof(ModOptionRange.Value));
							decimal stepSize = option.Value<decimal>(nameof(ModOptionRange.stepSize));


							optionList.Add(new ModOptionRange(identifier, labelText, min, max, stepSize, selection, (bool) checker, enabled));

						} else if ((checker = option.Value<decimal?>(nameof(ModOptionStepper.stepSize))) != null) {

							decimal min = option.Value<decimal>(nameof(ModOptionStepper.min));
							decimal max = option.Value<decimal>(nameof(ModOptionStepper.max));
							decimal defaultValue = option.Value<decimal>(nameof(ModOptionStepper.Value));
							int type = option.Value<int>(nameof(ModOptionStepper.type));
							optionList.Add(new ModOptionStepper(identifier, labelText, min, max, (decimal) checker, defaultValue, (DisplayType) type, enabled));

						} else if ((checker = option.Value<JObject>(nameof(ModOptionSelection.Choices))) != null) {

							ModSelectionOptionChoices choices = new ModSelectionOptionChoices();
							Dictionary<string, string> choicesDict = (checker as JObject).ToObject<Dictionary<string, string>>();

							foreach (KeyValuePair<string, string> pair in choicesDict) {
								choices.Add(pair.Key, pair.Value);
							}

							string selection = option.Value<string>(nameof(ModOptionSelection.Selection));

							optionList.Add(new ModOptionSelection(identifier, labelText, choices, selection, enabled));

						} else if ((checker = option.Value<int?>(nameof(ModOptionTrigger.type))) != null) {

							optionList.Add(new ModOptionTrigger(identifier, labelText, (OptionActionType) checker, enabled));

						} else {
							optionList.Add(new ModOptionCategoryLabel(identifier, labelText));
						}

					}

					return new ModOptions(version, optionList);
				}
			}

			// unknown
			throw new NotSupportedException($"Unknown type '{objectType?.FullName}'.");
		}

		/// <summary>Writes the JSON representation of the object.</summary>
		/// <param name="writer">The JSON writer.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			throw new InvalidOperationException("This converter does not write JSON.");
		}
	}
}
