using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookLogin
{
    public class ApiResults
    {
        public string Accesstoken { get; set; }
        public DateTime Tokenexpires { get; set; }
        public string GrantedScopes { get; set; }
        public string DeniedScopes { get; set; }
        public string Error { get; set; }
        public string ErrorReason { get; set; }
        public string ErrorDescription { get; set; }
        public bool ErrorFound { get; set; } = false;
    }
}
