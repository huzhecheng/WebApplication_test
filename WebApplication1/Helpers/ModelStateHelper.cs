using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace WebApplication1.Helpers
{
	public static class ModelStateHelper
	{
		public static string GetModelStateErrors(this ModelStateDictionary modelState)
		{
			IEnumerable<KeyValuePair<string, string[]>> errors = modelState.IsValid
				? null
				: modelState
					.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray())
					.Where(m => m.Value.Any());

			string output = "";

			if (errors != null)
			{
				foreach (KeyValuePair<string, string[]> kvp in errors)
				{
					output += "\"" + kvp.Key + "\":\"" + String.Join(", ", kvp.Value) + "\",";
				}
			}
			output = output.TrimEnd(',');
			output = string.Format("{{{0}}}", output);

			return output;
		}
	}
}