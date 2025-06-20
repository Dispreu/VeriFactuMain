﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography;

namespace VeriFactu.NoVeriFactu.Signature.Ms
{
  internal abstract class RSAPKCS1SignatureDescription : SignatureDescription
  {
    public RSAPKCS1SignatureDescription(string hashAlgorithmName)
    {
      KeyAlgorithm = typeof(RSA).AssemblyQualifiedName;
      FormatterAlgorithm = typeof(RSAPKCS1SignatureFormatter).AssemblyQualifiedName;
      DeformatterAlgorithm = typeof(RSAPKCS1SignatureDeformatter).AssemblyQualifiedName;
      DigestAlgorithm = hashAlgorithmName;
    }

    public override sealed AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
    {
      AsymmetricSignatureDeformatter item = (AsymmetricSignatureDeformatter)CryptoConfig.CreateFromName(DeformatterAlgorithm);
      item.SetKey(key);
      item.SetHashAlgorithm(DigestAlgorithm);
      return item;
    }

    public override sealed AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
    {
      AsymmetricSignatureFormatter item = (AsymmetricSignatureFormatter)CryptoConfig.CreateFromName(FormatterAlgorithm);
      item.SetKey(key);
      item.SetHashAlgorithm(DigestAlgorithm);
      return item;
    }

    public abstract override HashAlgorithm CreateDigest();
  }
}
