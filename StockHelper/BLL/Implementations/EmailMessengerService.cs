using System;
using System.Diagnostics;

namespace BLL.Implementations
{
    public class EmailMessengerService
    {
        private string _to;
        private string _subject;
        private string _body;

        /// <summary>
        /// Initializes a new email messenger with the recipient, subject, and body.
        /// </summary>
        public EmailMessengerService(string to, string subject, string body)
        {
            _to = to;
            _subject = subject;
            _body = body;
        }

        /// <summary>
        /// Opens the default mail client with a pre-filled email using the mailto protocol.
        /// </summary>
        public void SendEmail()
        {
            string url = $"mailto:{_to}?subject={Uri.EscapeDataString(_subject)}&body={Uri.EscapeDataString(_body)}";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}
