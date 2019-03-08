// <copyright file="SecurityHelper.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using Umbraco.Core;

namespace Fluidity.Helpers
{
    internal static class SecurityHelper
    {
        internal static string Encrypt(string input)
        {
            return input.EncryptWithMachineKey();
        }

        internal static string Decrypt(string input)
        {
            try
            {
                return input.DecryptWithMachineKey();
            }
            catch (ArgumentException ex)
            {
                if (!ex.Message.Contains("encryptedTicket"))
                    throw ex;

                return input;
            }
        }
    }
}
