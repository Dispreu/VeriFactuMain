﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography;

namespace VeriFactu.NoVeriFactu.Signature.Ms
{
  internal sealed class DSASignatureDescription : SignatureDescription
  {
    private const string HashAlgorithm = "SHA1";

    public DSASignatureDescription()
    {
      KeyAlgorithm = typeof(DSA).AssemblyQualifiedName;
      FormatterAlgorithm = typeof(DSASignatureFormatter).AssemblyQualifiedName;
      DeformatterAlgorithm = typeof(DSASignatureDeformatter).AssemblyQualifiedName;
      DigestAlgorithm = "SHA1";
    }

    public override sealed AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
    {
      AsymmetricSignatureDeformatter item = (AsymmetricSignatureDeformatter)CryptoConfig.CreateFromName(DeformatterAlgorithm);
      item.SetKey(key);
      item.SetHashAlgorithm(HashAlgorithm);
      return item;
    }

    public override sealed AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
    {
      AsymmetricSignatureFormatter item = (AsymmetricSignatureFormatter)CryptoConfig.CreateFromName(FormatterAlgorithm);
      item.SetKey(key);
      item.SetHashAlgorithm(HashAlgorithm);
      return item;
    }

    public override sealed HashAlgorithm CreateDigest()
    {
      return SHA1.Create();
    }
  }
}
