﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLittleHelpers
{
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// Extension methods for safe handling of the SecureString
    /// </summary>
    public static class SecureString
    {
        static byte[] entropy = System.Text.Encoding.Unicode.GetBytes("Question everything.");

        /// <summary>
        /// Encrypts the string using DPAPI 
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.String.</returns>
        public static string EncryptString(this System.Security.SecureString input)
        {
            byte[] encryptedData = System.Security.Cryptography.ProtectedData.Protect(
                System.Text.Encoding.Unicode.GetBytes(ToInsecureString(input)),
                entropy,
                System.Security.Cryptography.DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypts the string using DPAPI
        /// </summary>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <returns>System.Security.SecureString.</returns>
        public static System.Security.SecureString DecryptString(this string encryptedData)
        {
            try
            {
                byte[] decryptedData = System.Security.Cryptography.ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedData),
                    entropy,
                    System.Security.Cryptography.DataProtectionScope.CurrentUser);
                return SecureString.ToSecureString(System.Text.Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new System.Security.SecureString();
            }
        }

        /// <summary>
        /// Determines whether [the specified secure password] is null or empty
        /// </summary>
        /// <param name="securePassword">The secure password.</param>
        /// <returns><c>true</c> if [is null or empty] [the specified secure password]; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrEmpty(this System.Security.SecureString securePassword)
        {
            if (securePassword == null)
            {
                return true;
            }

            if (securePassword.Length < 1)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// To the insecure string.
        /// </summary>
        /// <param name="securePassword">The secure password.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ArgumentNullException">securePassword</exception>
        public static string ToInsecureString(this System.Security.SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// To the secure string.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>System.Security.SecureString.</returns>
        /// <exception cref="ArgumentNullException">password</exception>
        public static System.Security.SecureString ToSecureString(this string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            unsafe
            {
                fixed (char* passwordChars = password)
                {
                    var securePassword = new System.Security.SecureString(passwordChars, password.Length);
                    securePassword.MakeReadOnly();
                    return securePassword;
                }
            }
        }
    }
}