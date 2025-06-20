/*
    This file is part of the VeriFactu (R) project.
    Copyright (c) 2024-2025 Irene Solutions SL
    Authors: Irene Solutions SL.

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License version 3
    as published by the Free Software Foundation with the addition of the
    following permission added to Section 15 as permitted in Section 7(a):
    FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
    IRENE SOLUTIONS SL. IRENE SOLUTIONS SL DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
    OF THIRD PARTY RIGHTS
    
    This program is distributed in the hope that it will be useful, but
    WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
    or FITNESS FOR A PARTICULAR PURPOSE.
    See the GNU Affero General Public License for more details.
    You should have received a copy of the GNU Affero General Public License
    along with this program; if not, see http://www.gnu.org/licenses or write to
    the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
    Boston, MA, 02110-1301 USA, or download the license from the following URL:
        http://www.irenesolutions.com/terms-of-use.pdf
    
    The interactive user interfaces in modified source and object code versions
    of this program must display Appropriate Legal Notices, as required under
    Section 5 of the GNU Affero General Public License.
    
    You can be released from the requirements of the license by purchasing
    a commercial license. Buying such a license is mandatory as soon as you
    develop commercial activities involving the VeriFactu software without
    disclosing the source code of your own applications.
    These activities include: offering paid services to customers as an ASP,
    serving VeriFactu XML data on the fly in a web application, shipping VeriFactu
    with a closed source product.
    
    For more information, please contact Irene Solutions SL. at this
    address: info@irenesolutions.com
 */

using System;

namespace VeriFactu.Qrcode
{
  /// <summary>
  /// Encapsulates a QR Code's format information, including the data mask used and error correction level.
  /// </summary>
  /// <author>Sean Owen</author>
  /// <seealso cref="ErrorCorrectionLevel"/>
  internal sealed class FormatInformation
  {
    private const int FORMAT_INFO_MASK_QR = 0x5412;

    /// <summary>
    /// See ISO 18004:2006, Annex C, Table C.1
    /// </summary>
    private static readonly int[][] FORMAT_INFO_DECODE_LOOKUP = new int[][]
    {
      new int[] { 0x5412, 0x00 }, new 
            int[]
                                  {
                                    0x5125, 0x01
                                  }, new int[] { 0x5E7C, 0x02 }, new int[] { 0x5B4B, 0x03 }, new int[]
                                                                                             {
                                                                                               0x45F9, 0x04
                                                                                             }, new int[] { 0x40CE, 0x05 }, new int[] { 0x4F97, 0x06 }, new int[] { 0x4AA0, 0x07 },
      new int[] { 0x77C4, 0x08 }, new int[] { 0x72F3, 0x09 },
      new int[] { 0x7DAA, 0x0A }, new int[] { 0x789D, 0x0B }, new int[
            ]
                                                              {
                                                                0x662F, 0x0C
                                                              }, new int[] { 0x6318, 0x0D }, new int[] { 0x6C41, 0x0E }, new int[] { 0x6976, 0x0F }, new int[] { 0x1689, 0x10 },
      new int[] { 0x13BE, 0x11 },
      new int[] { 0x1CE7, 0x12 }, new int[] { 0x19D0, 0x13 }, new int[] { 0x0762, 0x14 }, new int[] { 0x0255, 0x15 }, new int[] { 0x0D0C, 0x16 }, new int[
            ]
                                                                                                                                                  {
                                                                                                                                                    0x083B, 0x17
                                                                                                                                                  }, new int[] { 0x355F, 0x18 },
      new int[] { 0x3068, 0x19 }, new int[] { 0x3F31, 0x1A }, new int[] { 0x3A06, 0x1B }, new int[] { 0x24B4, 0x1C },
      new int[] { 0x2183, 0x1D }, new int[] { 0x2EDA, 0x1E }, new int[] { 0x2BED, 0x1F }
    };

    /// <summary>
    /// Offset i holds the number of 1 bits in the binary representation of i
    /// </summary>
    private static readonly int[] BITS_SET_IN_HALF_BYTE = new int[] { 0, 1, 1, 2, 1, 2, 2, 3, 1, 2, 2, 3, 2, 3, 3, 4 };

    private readonly ErrorCorrectionLevel errorCorrectionLevel;

    private readonly byte dataMask;

    private FormatInformation(int formatInfo)
    {
      // Bits 3,4
      errorCorrectionLevel = ErrorCorrectionLevel.ForBits((formatInfo >> 3) & 0x03);
      // Bottom 3 bits
      dataMask = (byte)(formatInfo & 0x07);
    }

    internal static int NumBitsDiffering(int a, int b)
    {
      // a now has a 1 bit exactly where its bit differs with b's
      a ^= b;
      // Count bits set quickly with a series of lookups:
      return BITS_SET_IN_HALF_BYTE[a & 0x0F] + BITS_SET_IN_HALF_BYTE[((int)(((uint)a) >> 4) & 0x0F)] + BITS_SET_IN_HALF_BYTE[((int)(((uint)a) >> 8) & 0x0F)] + BITS_SET_IN_HALF_BYTE[
        ((int)(((uint)a) >> 12) & 0x0F)] + BITS_SET_IN_HALF_BYTE[((int)(((uint)a) >> 16) & 0x0F)] + BITS_SET_IN_HALF_BYTE[((int)(((uint)a) >> 20) & 0x0F)] + BITS_SET_IN_HALF_BYTE[
        ((int)(((uint)a) >> 24) & 0x0F)] + BITS_SET_IN_HALF_BYTE[((int)(((uint)a) >> 28) & 0x0F)];
    }

    /// <param name="maskedFormatInfo1">format info indicator, with mask still applied</param>
    /// <param name="maskedFormatInfo2">
    /// second copy of same info; both are checked at the same time to establish best match
    /// </param>
    /// <returns>
    /// information about the format it specifies, or <c>null</c> if doesn't seem to match any known pattern
    /// </returns>
    internal static FormatInformation DecodeFormatInformation(
      int maskedFormatInfo1,
      int
             maskedFormatInfo2)
    {
      FormatInformation formatInfo = DoDecodeFormatInformation(maskedFormatInfo1, maskedFormatInfo2);
      if(formatInfo != null)
      {
        return formatInfo;
      }
      // Should return null, but, some QR codes apparently
      // do not mask this info. Try again by actually masking the pattern
      // first
      return DoDecodeFormatInformation(maskedFormatInfo1 ^ FORMAT_INFO_MASK_QR, maskedFormatInfo2 ^ FORMAT_INFO_MASK_QR);
    }

    private static FormatInformation DoDecodeFormatInformation(
      int maskedFormatInfo1,
      int
             maskedFormatInfo2)
    {
      // Find the int in FORMAT_INFO_DECODE_LOOKUP with fewest bits differing
      int bestDifference = int.MaxValue;
      int bestFormatInfo = 0;
      for(int i = 0; i < FORMAT_INFO_DECODE_LOOKUP.Length; i++)
      {
        int[] decodeInfo = FORMAT_INFO_DECODE_LOOKUP[i];
        int targetInfo = decodeInfo[0];
        if(targetInfo == maskedFormatInfo1 || targetInfo == maskedFormatInfo2)
        {
          // Found an exact match
          return new FormatInformation(decodeInfo[1]);
        }
        int bitsDifference = NumBitsDiffering(maskedFormatInfo1, targetInfo);
        if(bitsDifference < bestDifference)
        {
          bestFormatInfo = decodeInfo[1];
          bestDifference = bitsDifference;
        }
        if(maskedFormatInfo1 != maskedFormatInfo2)
        {
          // also try the other option
          bitsDifference = NumBitsDiffering(maskedFormatInfo2, targetInfo);
          if(bitsDifference < bestDifference)
          {
            bestFormatInfo = decodeInfo[1];
            bestDifference = bitsDifference;
          }
        }
      }
      // Hamming distance of the 32 masked codes is 7, by construction, so <= 3 bits
      // differing means we found a match
      if(bestDifference <= 3)
      {
        return new FormatInformation(bestFormatInfo);
      }
      return null;
    }

    internal ErrorCorrectionLevel GetErrorCorrectionLevel()
    {
      return errorCorrectionLevel;
    }

    /// <returns>The datamask in byte-format</returns>
    internal byte GetDataMask()
    {
      return dataMask;
    }

    /// <returns>the hashcode of the QR-code format information</returns>
    public override int GetHashCode()
    {
      return (errorCorrectionLevel.Ordinal() << 3) | (int)dataMask;
    }

    /// <summary>
    /// Compares the Format Information of this and o
    /// </summary>
    /// <param name="o">object to compare to</param>
    /// <returns>
    /// True if o is a FormatInformationObject and the error-correction level and the datamask are equal, false
    /// otherwise
    /// </returns>
    public override bool Equals(object o)
    {
      if(!(o is FormatInformation))
      {
        return false;
      }
      FormatInformation other = (FormatInformation)o;
      return errorCorrectionLevel == other.errorCorrectionLevel && dataMask == other.dataMask;
    }
  }
}
