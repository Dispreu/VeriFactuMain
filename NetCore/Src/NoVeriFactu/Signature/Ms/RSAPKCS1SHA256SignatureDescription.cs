﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography;

namespace VeriFactu.NoVeriFactu.Signature.Ms
{
  internal sealed class RSAPKCS1SHA256SignatureDescription : RSAPKCS1SignatureDescription
  {
    public RSAPKCS1SHA256SignatureDescription() : base("SHA256")
        { }

    public override sealed HashAlgorithm CreateDigest()
    {
      return SHA256.Create();
    }
  }
}
