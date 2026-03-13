using BLL;
using BLL.Implementations;
using BLL.Templates;
using Services.Contracts.CustomsException;
using Services.Implementations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Implementations;

namespace UI.controlForms
{
    public partial class ctrlAnalytics : TranslatableUserControls
    {
        LanguageService lang = LanguageService.GetInstance;
        /// <summary>
        /// Initializes the analytics control and wires up event handlers.
        /// </summary>
        public ctrlAnalytics()
        {
            InitializeComponent();
            btnGenerate.Click += btnGenerate_Click;
        }

        /// <summary>
        /// Applies translations to all labels and grid headers.
        /// </summary>
        public override void ApplyTranslations()
        {
            lbAnalytics.Text = lang.Translate("Purchase Statistics");
            label1.Text = lang.Translate("From:");
            label2.Text = lang.Translate("To:");
            btnGenerate.Text = lang.Translate("Generate");
            btnSendToEmail.Text = lang.Translate("Send to Email");

            CategoryOfPurchase.HeaderText = lang.Translate("Category of Purchase");
            TotalOrders.HeaderText = lang.Translate("Total Orders");
            TotalSpent.HeaderText = lang.Translate("Total Spent");
            Percentage.HeaderText = lang.Translate("% of Total Spending");

            ProviderOfPurchase.HeaderText = lang.Translate("Provider");
            dataGridViewTextBoxColumn2.HeaderText = lang.Translate("Total Orders");
            dataGridViewTextBoxColumn3.HeaderText = lang.Translate("Total Spent");
            dataGridViewTextBoxColumn4.HeaderText = lang.Translate("% of Total Spending");
        }

        /// <summary>
        /// Handles the Generate button click to compute and display statistics.
        /// </summary>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime from = dtpFrom.Value.Date;
                DateTime to = dtpTo.Value.Date.AddDays(1).AddTicks(-1);

                if (from > to)
                {
                    MessageBox.Show(
                        lang.Translate("The 'From' date must be earlier than or equal to the 'To' date."),
                        lang.Translate("Validation"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var categoryStats = AnalyticsService.Instance().GetStatsByCategory(from, to);
                var providerStats = AnalyticsService.Instance().GetStatsByProvider(from, to);

                PopulateCategoryGrid(categoryStats);
                PopulateProviderGrid(providerStats);
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("System Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Populates the category statistics grid with the given data.
        /// </summary>
        private void PopulateCategoryGrid(List<CategoryAnalyticsRow> rows)
        {
            dataGridView1.Rows.Clear();
            foreach (var row in rows)
            {
                dataGridView1.Rows.Add(
                    row.CategoryName,
                    row.TotalOrders,
                    row.TotalSpent.ToString("$#,##0.00"),
                    row.Percentage.ToString("0.00") + "%"
                );
            }
        }

        /// <summary>
        /// Populates the provider statistics grid with the given data.
        /// </summary>
        private void PopulateProviderGrid(List<ProviderAnalyticsRow> rows)
        {
            dataGridView2.Rows.Clear();
            foreach (var row in rows)
            {
                dataGridView2.Rows.Add(
                    row.ProviderName,
                    row.TotalOrders,
                    row.TotalSpent.ToString("$#,##0.00"),
                    row.Percentage.ToString("0.00") + "%"
                );
            }
        }

        /// <summary>
        /// Handles the Send to Email button click to open the email client with the analytics report.
        /// </summary>
        private void btnSendToEmail_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count == 0 && dataGridView2.Rows.Count == 0)
                {
                    MessageBox.Show(
                        lang.Translate("Please generate the statistics before sending."),
                        lang.Translate("Warning"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                DateTime from = dtpFrom.Value.Date;
                DateTime to = dtpTo.Value.Date.AddDays(1).AddTicks(-1);

                var categoryStats = AnalyticsService.Instance().GetStatsByCategory(from, to);
                var providerStats = AnalyticsService.Instance().GetStatsByProvider(from, to);

                string body = EmailMessageTemplates.BuildAnalyticsReport(categoryStats, providerStats, from, to, lang);
                string subject = EmailMessageTemplates.BuildAnalyticsSubject(from, to, lang);

                string recipient = frmMain.GetInstance().CurrentUser.Email ?? "";
                var emailService = new EmailMessengerService(recipient, subject, body);
                emailService.SendEmail();
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(
                    lang.Translate("A system error occurred while sending the report: ") + ex.Message,
                    lang.Translate("System Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred while sending the report: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
