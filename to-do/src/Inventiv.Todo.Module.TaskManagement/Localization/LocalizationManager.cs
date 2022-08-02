using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Gazel;
using Gazel.Localization;
using Gazel.Service;

namespace Inventiv.Todo.Module.TaskManagement.Localization
{
	public class LocalizationManager : ILocalizer //Gazel talks to your localization logic through ILocalizer
	{
		private readonly IModuleContext context;

		public LocalizationManager(IModuleContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// This service calculates a result code. e.g. passing (TaskManagement, ERROR, 0) will give you (20701)
		/// Use this code to create error messages in Messages.resx file.
		/// Localization key for error codes are in 'ERR-{CODE}' format. e.g. ERR-20701
		/// </summary>
		public int CalculateResultCode(string module, ResultCodeType type, int code)
		{
			var dummy = ResultCodes.TaskManagement; //this is a dummy call to load static 
			var block = ResultCodes.GetAll().SingleOrDefault(rcb => rcb.ModuleName == module);

			if (block == null) { return -1; }

			if (type == ResultCodeType.Error) { return block.Err(code); }
			if (type == ResultCodeType.Warning) { return block.Err(code); }
			if (type == ResultCodeType.Info) { return block.Err(code); }
			if (type == ResultCodeType.Success) { return ResultCodes.Gazel.Success(); }
			if (type == ResultCodeType.Fatal) { return ResultCodes.Gazel.Fatal(); }

			return -1;
		}

		private string GetLocalizedText(string key, params object[] args)
		{
			var message = Messages.ResourceManager.GetString(key);
			if (message.IsNullOrWhiteSpace())
			{
				return $"[{key}]";
			}

			return message.FormatWith(args);
		}

		private IEnumerable<CultureInfo> GetLanguages()
		{
			yield return new CultureInfo("en-US");
		}

		private IEnumerable<KeyValuePair<string, string>> GetLocalizedTexts()
		{
			foreach (DictionaryEntry entry in Messages.ResourceManager.GetResourceSet(context.System.CurrentCulture, false, false))
			{
				yield return new KeyValuePair<string, string>(
					entry.Key.ToString(),
					entry.Value.ToString()
				);
			}
		}

		#region Api Mappings

		#region Localization

		string ILocalizer.GetLocalizedText(string key, params object[] args) => GetLocalizedText(key, args);
		List<CultureInfo> ILocalizer.GetLanguages() => GetLanguages().ToList();
		Dictionary<string, string> ILocalizer.GetLocalizedTexts() => GetLocalizedTexts().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		#endregion

		#endregion
	}
}
