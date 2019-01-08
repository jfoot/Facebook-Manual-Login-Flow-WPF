using System;
using System.Net;
using System.Windows;
using System.Windows.Navigation;

namespace FacebookLogin
{
    /// <summary>
    /// Interaction logic for FBDialog.xaml
    /// </summary>
    public partial class FBDialog : Window
    {
        private string p_appID;
        private string p_scopes;

        public ApiResults data = new ApiResults();

        public FBDialog(string inpAppID, string inpScopes)
        {
            p_appID = inpAppID;
            p_scopes = inpScopes;
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            string returnURL = WebUtility.UrlEncode("https://www.facebook.com/connect/login_success.html");
            string scopes = WebUtility.UrlEncode(p_scopes);
            FBwebBrowser.Source = new Uri(string.Format("https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&response_type=token%2Cgranted_scopes&scope={2}&display=popup", new object[] { p_appID, returnURL, scopes }));
            FBwebBrowser.Navigated += FBwebBrowser_Navigated;

        }

        private void FBwebBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            // Check to see if we hit return url
            if (FBwebBrowser.Source.AbsolutePath == "/connect/login_success.html")
            {
                // Check for error
                if (FBwebBrowser.Source.Query.Contains("error"))
                {
                    // Error detected
                    data.ErrorFound = true;
                    ExtractURLInfo("?", FBwebBrowser.Source.Query);
                }
                else
                {
                    data.ErrorFound = false;
                    ExtractURLInfo("#", FBwebBrowser.Source.Fragment);
                }
                // Close the dialog
                this.Close();
            }
        }


        private void ExtractURLInfo(string inpTrimChar, string urlInfo)
        {
            string fragments = urlInfo.Trim(char.Parse(inpTrimChar)); // Trim the hash or the ? mark
            string[] parameters = fragments.Split(char.Parse("&")); // Split the url fragments / query string 

            // Extract info from url
            foreach (string parameter in parameters)
            {
                string[] name_value = parameter.Split(char.Parse("=")); // Split the input

                switch (name_value[0])
                {
                    case "access_token":
                        data.Accesstoken = name_value[1];
                        break;
                    case "expires_in":
                        double expires = 0;
                        if (double.TryParse(name_value[1], out expires))
                            data.Tokenexpires = DateTime.Now.AddSeconds(expires);
                        else
                            data.Tokenexpires = DateTime.Now;
                        break;
                    case "granted_scopes":
                        data.GrantedScopes = WebUtility.UrlDecode(name_value[1]);
                        break;
                    case "denied_scopes":
                        data.DeniedScopes = WebUtility.UrlDecode(name_value[1]);
                        break;
                    case "error":
                        data.Error = WebUtility.UrlDecode(name_value[1]);
                        break;
                    case "error_reason":
                        data.ErrorReason = WebUtility.UrlDecode(name_value[1]);
                        break;
                    case "error_description":
                        data.ErrorDescription = WebUtility.UrlDecode(name_value[1]);
                        break;
                    default:
                        break;
                }
            }
        }

        private void ClosingWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainWindow)(Owner)).SendArgumentsBack(data);
        }
    }
}
