using System;
using System.Text;
using Domain;
using Services.Implementations;

namespace BLL.Templates
{
    public static class WhatsAppMessageTemplates
    {
        /// <summary>
        /// Builds a WhatsApp message body for sending a replacement order to a provider.
        /// </summary>
        public static string BuildOrderMessage(ReplacementOrder order, LanguageService lang)
        {
            var sb = new StringBuilder();

            sb.AppendLine(lang.Translate("Good morning,"));
            sb.AppendLine($"{lang.Translate("We send you the following replacement order")} (N° {order.ReplacementOrderNumber}):");
            sb.AppendLine();

            foreach (var row in order.OrderRows)
            {
                string unitName = row.Item.Unit != null && row.Item.Unit.ContainsKey("Name")
                    ? row.Item.Unit["Name"]?.ToString() : "";

                string formattedQuantity = row.Item.IsUnitInteger()
                    ? ((int)row.Quantity).ToString()
                    : row.Quantity.ToString("F2");

                sb.AppendLine($"- {row.Item.Name}: {formattedQuantity} {unitName}".TrimEnd());
            }

            sb.AppendLine();
            sb.AppendLine(lang.Translate("We await your confirmation."));
            sb.Append(lang.Translate("Kind regards."));

            return sb.ToString();
        }
    }
}
