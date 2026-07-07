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
using Services.Implementations;
using Services.Contracts.Logs;

namespace UI.controlForms
{
    public partial class ctrlLogs : TranslatableUserControls
    {
        LanguageService lang = LanguageService.GetInstance;
        LogReaderService logReader = LogReaderService.Instance();

        /// <summary>
        /// Initializes the logs control, building the level filter, grid columns, initial configuration and loading logs.
        /// </summary>
        public ctrlLogs()
        {
            InitializeComponent();
            loadCheckedListBox();
            SetupGrid();
            InitialConfig();
            LoadLogs();
        }

        /// <summary>
        /// Populates the log levels checked list box with every level except Debug and checks them all by default.
        /// </summary>
        private void loadCheckedListBox()
        {
            cklbLogsLevels.Items.Clear();
            foreach (var level in Enum.GetValues(typeof(LogLevels)))
            {
                if (level.ToString() == LogLevels.Debug.ToString()) continue;
                cklbLogsLevels.Items.Add(level);
            }

            for (int i = 0; i < cklbLogsLevels.Items.Count; i++)
            {
                cklbLogsLevels.SetItemChecked(i, true);
            }
        }

        /// <summary>
        /// Defines the log grid columns (the designer grid ships with none).
        /// </summary>
        private void SetupGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.Columns.Add("colTimestamp", "Timestamp");
            dataGridView1.Columns.Add("colLevel", "Level");
            dataGridView1.Columns.Add("colMessage", "Message");

            dataGridView1.Columns["colTimestamp"].Width = 150;
            dataGridView1.Columns["colLevel"].Width = 80;
            dataGridView1.Columns["colMessage"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        /// <summary>
        /// Configures the log grid as read-only full-row selection and sets the default date range and filter handler.
        /// </summary>
        private void InitialConfig()
        {
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dateTimePicker1.Value = DateTime.Today.AddDays(-7);
            dateTimePicker2.Value = DateTime.Today;

            btnFilter.Click += btnFilter_Click;
            btnClose.Click += btnClose_Click;
        }

        /// <summary>
        /// Handles the close button click: removes this control from the main panel,
        /// restores the main panel's default size and disposes the control.
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
            frmMain.GetInstance().ResetMainPanelSize();
            this.Dispose();
        }

        /// <summary>
        /// Reloads the logs using the current date range and level filters.
        /// </summary>
        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadLogs();
        }

        /// <summary>
        /// Reads the logs for the selected date range and levels and renders them.
        /// </summary>
        private void LoadLogs()
        {
            try
            {
                DateTime from = dateTimePicker1.Value.Date;
                DateTime to = dateTimePicker2.Value.Date.AddDays(1).AddTicks(-1);

                var levels = cklbLogsLevels.CheckedItems.Cast<LogLevels>().ToList();

                var logs = logReader.GetLogs(from, to, levels);
                RenderLogs(logs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    lang.Translate("UnexpectedError") + "\n\n" + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Renders the given log entries into the grid, tagging each row and coloring it by level.
        /// </summary>
        /// <param name="logs">The log entries to display.</param>
        private void RenderLogs(List<LogEntry> logs)
        {
            dataGridView1.Rows.Clear();
            foreach (var log in logs)
            {
                int idx = dataGridView1.Rows.Add(
                    log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    log.Level.ToString(),
                    log.Message);
                dataGridView1.Rows[idx].Tag = log;
                dataGridView1.Rows[idx].DefaultCellStyle.ForeColor = ColorForLevel(log.Level);
            }
            dataGridView1.Refresh();
        }

        /// <summary>
        /// Returns the display color associated with the given log level.
        /// </summary>
        /// <param name="level">The log level to map to a color.</param>
        /// <returns>The color used to render log entries of the given level.</returns>
        private static Color ColorForLevel(LogLevels level)
        {
            switch (level)
            {
                case LogLevels.Warning: return Color.DarkGoldenrod;
                case LogLevels.Error: return Color.Firebrick;
                case LogLevels.Fatal: return Color.DarkRed;
                default: return Color.Black;
            }
        }

        /// <summary>
        /// Applies the current language translations to the labels, filter button and log grid columns.
        /// </summary>
        public override void ApplyTranslations()
        {
            label1.Text = lang.Translate("From:");
            label2.Text = lang.Translate("To:");
            btnFilter.Text = lang.Translate("Filter");

            if (dataGridView1.Columns["colTimestamp"] != null)
                dataGridView1.Columns["colTimestamp"].HeaderText = lang.Translate("Timestamp");
            if (dataGridView1.Columns["colLevel"] != null)
                dataGridView1.Columns["colLevel"].HeaderText = lang.Translate("Level");
            if (dataGridView1.Columns["colMessage"] != null)
                dataGridView1.Columns["colMessage"].HeaderText = lang.Translate("Message");

            dataGridView1.Refresh();
        }
    }
}
