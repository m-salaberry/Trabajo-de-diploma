using System;
using System.Diagnostics;

namespace BLL.Implementations
{
    public class WhatsAppMessengerService
    {
        private string _phoneNumber;
        private string _message;

        /// <summary>
        /// Initializes a new WhatsApp messenger with the recipient phone number and message.
        /// </summary>
        public WhatsAppMessengerService(string phoneNumber, string message)
        {
            _phoneNumber = phoneNumber;
            _message = message;
        }

        /// <summary>
        /// Opens WhatsApp Web with a pre-filled message using the wa.me deep link.
        /// </summary>
        public void SendMessage()
        {
            string url = $"https://wa.me/{_phoneNumber}?text={Uri.EscapeDataString(_message)}";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}
