using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity;
using msit116apexLayout.Models;
using System.Threading.Tasks;
using OtpSharp;
using Base32;

namespace msit116apexLayout
{
    public class GoogleAuthenticatorTokenProvider: 
        IUserTokenProvider<ApplicationUser, string>
    {
        public Task<string> GenerateAsync(
            string purpose, 
            UserManager<ApplicationUser, string> manager, 
            ApplicationUser user)
        {
            return System.Threading.Tasks.Task.FromResult((string)null);
        }

        public Task<bool> ValidateAsync(
            string purpose, 
            string token, 
            UserManager<ApplicationUser, string> manager, 
            ApplicationUser user)
        {
            long timeStepMatched = 0;

            byte[] decodedKey = Base32Encoder.Decode(user.GoogleAuthenticatorSecretKey);
            var otp = new Totp(decodedKey);
            bool valid = otp.VerifyTotp(
                token, out timeStepMatched, new VerificationWindow(2, 2));

            return System.Threading.Tasks.Task.FromResult(valid);
        }

        public System.Threading.Tasks.Task NotifyAsync(
            string token, 
            UserManager<ApplicationUser, string> manager, 
            ApplicationUser user)
        {
            return System.Threading.Tasks.Task.FromResult(true);
        }

        public Task<bool> IsValidProviderForUserAsync(
            UserManager<ApplicationUser, string> manager, 
            ApplicationUser user)
        {
            return System.Threading.Tasks.Task.FromResult(user.IsGoogleAuthenticatorEnabled);
        }
    }
}