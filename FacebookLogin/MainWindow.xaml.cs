using System;
using System.Windows;

namespace FacebookLogin
{
    public partial class MainWindow : Window
    {
        ApiResults ResultsData = new ApiResults();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            FBDialog fbd = new FBDialog(AppId.Text, Scopes.Text);
            fbd.Owner = this;
            fbd.Show();
        }

        public void SendArgumentsBack(ApiResults data)
        {
            ResultsData = data;
            if (data.ErrorFound)
            {
                MessageBox.Show(ResultsData.Error + Environment.NewLine + ResultsData.ErrorDescription + Environment.NewLine + ResultsData.ErrorReason);
            }
            else
            {
                MessageBox.Show(ResultsData.Accesstoken + Environment.NewLine + ResultsData.GrantedScopes + Environment.NewLine + ResultsData.DeniedScopes);
            }
        }
    }
}
