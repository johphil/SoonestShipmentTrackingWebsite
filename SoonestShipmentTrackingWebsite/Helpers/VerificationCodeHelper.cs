using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace SoonestShipmentTrackingWebsite.Helpers
{
    public static class VerificationCodeHelper
    {
        public static readonly TimeSpan CodeLifetime = TimeSpan.FromMinutes(15);

        // A 6-digit numeric code, zero-padded (e.g. "042917"). Uses a
        // cryptographic RNG rather than System.Random: Random isn't
        // thread-safe under concurrent web requests, and a verification
        // code should be hard to guess/predict anyway.
        public static string GenerateCode()
        {
            byte[] bytes = new byte[4];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            uint value = BitConverter.ToUInt32(bytes, 0);
            int code = (int)(value % 1000000);
            return code.ToString("D6");
        }
    }
}