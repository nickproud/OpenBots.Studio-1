using System;
using System.ComponentModel;

namespace OpenBots.Core.Enums
{
    public static class EnumsHelper
    {
		public static T GetValueFromDescription<T>(string description) where T : Enum
		{
			foreach (var field in typeof(T).GetFields())
			{
				if (Attribute.GetCustomAttribute(field,
				typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
				{
					if (attribute.Description == description)
						return (T)field.GetValue(null);
				}
				else
				{
					if (field.Name == description)
						return (T)field.GetValue(null);
				}
			}

			throw new ArgumentException("Not found.", nameof(description));
		}

		public static string GetDescription(this Enum enumValue)
		{
			object[] attr = enumValue.GetType().GetField(enumValue.ToString())
				.GetCustomAttributes(typeof(DescriptionAttribute), false);

			return attr.Length > 0
			   ? ((DescriptionAttribute)attr[0]).Description
			   : enumValue.ToString();
		}
	}
}
