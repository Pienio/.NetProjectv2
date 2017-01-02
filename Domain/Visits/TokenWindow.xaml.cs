using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace Visits
{
    /// <summary>
    /// Interaction logic for TokenWindow.xaml
    /// </summary>
    public partial class TokenWindow : MetroWindow
    {
        private string mailAddress;
        private string token;
        private bool RorE;

        public TokenWindow(string mailAddress,bool tr)
        {
            InitializeComponent();

            this.mailAddress = mailAddress;
            RorE = tr;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                bool numSign = r.Next(2) == 1;
                sb.Append(numSign ? (char)r.Next(49, 58) : (char)r.Next(65, 91));
            }
            token = sb.ToString();
            MailService.MailServices mail = new MailService.MailServices();
            if (RorE)
                mail.SendRegistrationConfirmation(mailAddress, token);
            else
            {
                mail.SendEditConfirmation(mailAddress, token);
            }
        }

        private void accept_Click(object sender, RoutedEventArgs e)
        {
            if (tokenBox.Text == token)
            {
                DialogResult = true;
                Close();
                return;
            }
            MessageBox.Show("Podany token nie jest prawidłowy.", App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void activationMail_Click(object sender, RoutedEventArgs e)
        {
            MailService.MailServices mail = new MailService.MailServices();
            if(RorE)
                mail.SendRegistrationConfirmation(mailAddress, token);
            else
            {
                mail.SendEditConfirmation(mailAddress, token);
            }
        }

    }
}
