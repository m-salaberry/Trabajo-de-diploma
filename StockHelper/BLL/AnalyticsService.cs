using BLL.Implementations;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class CategoryAnalyticsRow
    {
        public string CategoryName { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal Percentage { get; set; }
    }

    public class ProviderAnalyticsRow
    {
        public string ProviderName { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal Percentage { get; set; }
    }

    public class AnalyticsService
    {
        private static AnalyticsService instance;

        /// <summary>
        /// Returns the singleton instance of AnalyticsService.
        /// </summary>
        public static AnalyticsService Instance()
        {
            if (instance == null)
            {
                instance = new AnalyticsService();
            }
            return instance;
        }

        /// <summary>
        /// Private constructor to enforce singleton pattern.
        /// </summary>
        private AnalyticsService() { }

        /// <summary>
        /// Returns purchase statistics grouped by category for the specified date range, including totals and percentages.
        /// </summary>
        public List<CategoryAnalyticsRow> GetStatsByCategory(DateTime from, DateTime to)
        {
            var orders = PurchaseOrderService.Instance().GetAll()
                .Where(po => po.IssuedDate >= from && po.IssuedDate <= to
                          && po.Status != PurchaseOrderStatus.Cancelled)
                .ToList();

            decimal totalSpent = orders.Sum(po => po.TotalAmount);

            return orders
                .GroupBy(po => po.ReplacementOrder.Provider.Category.Name)
                .Select(g => new CategoryAnalyticsRow
                {
                    CategoryName = g.Key,
                    TotalOrders = g.Count(),
                    TotalSpent = g.Sum(po => po.TotalAmount),
                    Percentage = totalSpent > 0
                        ? Math.Round(g.Sum(po => po.TotalAmount) / totalSpent * 100, 2)
                        : 0
                })
                .OrderByDescending(r => r.TotalSpent)
                .ToList();
        }

        /// <summary>
        /// Returns purchase statistics grouped by provider for the specified date range, including totals and percentages.
        /// </summary>
        public List<ProviderAnalyticsRow> GetStatsByProvider(DateTime from, DateTime to)
        {
            var orders = PurchaseOrderService.Instance().GetAll()
                .Where(po => po.IssuedDate >= from && po.IssuedDate <= to
                          && po.Status != PurchaseOrderStatus.Cancelled)
                .ToList();

            decimal totalSpent = orders.Sum(po => po.TotalAmount);

            return orders
                .GroupBy(po => po.ReplacementOrder.Provider.Name)
                .Select(g => new ProviderAnalyticsRow
                {
                    ProviderName = g.Key,
                    TotalOrders = g.Count(),
                    TotalSpent = g.Sum(po => po.TotalAmount),
                    Percentage = totalSpent > 0
                        ? Math.Round(g.Sum(po => po.TotalAmount) / totalSpent * 100, 2)
                        : 0
                })
                .OrderByDescending(r => r.TotalSpent)
                .ToList();
        }
    }
}
