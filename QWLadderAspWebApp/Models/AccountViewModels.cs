using System;
using System.Collections.Generic;

// sourced from: https://github.com/bumblebeeman/angularmvctutorial/blob/3f0c3be95ebebfe30e0ca8e280a9e7443ee72acf/Part%20Three/Awesome%20Angular%20Web%20App%202.0/Awesome%20Angular%20Web%20App%202.0/Models/AccountViewModels.cs
namespace QWLadderAspWebApp.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}