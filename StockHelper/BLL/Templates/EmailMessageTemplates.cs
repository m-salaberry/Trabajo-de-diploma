using System;
using System.Collections.Generic;
using System.Text;
using Services.Implementations;

namespace BLL.Templates
{
    public static class EmailMessageTemplates
    {
        /// <summary>
        /// Builds a plain-text analytics report with category and provider statistics for the given period.
        /// </summary>
        public static string BuildAnalyticsReport(
            List<CategoryAnalyticsRow> categoryStats,
            List<ProviderAnalyticsRow> providerStats,
            DateTime from, DateTime to,
            LanguageService lang)
        {
            var sb = new StringBuilder();

            sb.AppendLine(lang.Translate("Purchase Statistics Report"));
            sb.AppendLine($"{lang.Translate("Period:")} {from:dd/MM/yyyy} - {to:dd/MM/yyyy}");
            sb.AppendLine();

            sb.AppendLine($"--- {lang.Translate("Statistics by Category")} ---");
            sb.AppendLine($"{"Category",-20} | {"Orders",-6} | {"Spent",-12} | %");
            foreach (var row in categoryStats)
            {
                sb.AppendLine($"{row.CategoryName,-20} | {row.TotalOrders,-6} | {row.TotalSpent.ToString("$#,##0.00"),-12} | {row.Percentage.ToString("0.00")}%");
            }

            sb.AppendLine();

            sb.AppendLine($"--- {lang.Translate("Statistics by Provider")} ---");
            sb.AppendLine($"{"Provider",-20} | {"Orders",-6} | {"Spent",-12} | %");
            foreach (var row in providerStats)
            {
                sb.AppendLine($"{row.ProviderName,-20} | {row.TotalOrders,-6} | {row.TotalSpent.ToString("$#,##0.00"),-12} | {row.Percentage.ToString("0.00")}%");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Builds the email subject line for the analytics report with the date range.
        /// </summary>
        public static string BuildAnalyticsSubject(DateTime from, DateTime to, LanguageService lang)
        {
            return $"{lang.Translate("Purchase Statistics Report")} - {from:dd/MM/yyyy} to {to:dd/MM/yyyy}";
        }
    }
}
