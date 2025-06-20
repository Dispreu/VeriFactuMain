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
  /// See ISO 18004:2006 Annex D.
  /// </summary>
  /// <author>Sean Owen</author>
  internal sealed class Version
  {
    /// <summary>
    /// See ISO 18004:2006 Annex D.
    /// </summary>
    /// <remarks>
    /// See ISO 18004:2006 Annex D. Element i represents the raw version bits that specify version i + 7
    /// </remarks>
    private static readonly int[] VERSION_DECODE_INFO = new int[]
    {
      0x07C94, 0x085BC, 0x09A99, 0x0A4D3, 0x0BBF6, 0x0C762, 0x0D847, 0x0E60D, 0x0F928, 0x10B78, 0x1145D, 0x12A17, 0x13532, 0x149A6, 0x15683, 0x168C9, 0x177EC, 0x18EC4, 0x191E1,
      0x1AFAB, 0x1B08E, 0x1CC1A, 0x1D33F, 0x1ED75, 0x1F250, 0x209D5, 0x216F0, 0x228BA, 0x2379F, 0x24B0B, 0x2542E, 0x26A64, 0x27541, 0x28C69
    };

    private static readonly Version[] VERSIONS = BuildVersions();

    private readonly int versionNumber;

    private readonly int[] alignmentPatternCenters;

    private readonly ECBlocks[] ecBlocks;

    private readonly int totalCodewords;

    private Version(
      int versionNumber,
      int[] alignmentPatternCenters,
      ECBlocks ecBlocks1,
      ECBlocks
             ecBlocks2,
      ECBlocks ecBlocks3,
      ECBlocks ecBlocks4)
    {
      this.versionNumber = versionNumber;
      this.alignmentPatternCenters = (int[])alignmentPatternCenters.Clone();
      ecBlocks = new ECBlocks[] { ecBlocks1, ecBlocks2, ecBlocks3, ecBlocks4 };
      int total = 0;
      int ecCodewords = ecBlocks1.GetECCodewordsPerBlock();
      ECB[] ecbArray = ecBlocks1.GetECBlocks();
      for(int i = 0; i < ecbArray.Length; i++)
      {
        ECB ecBlock = ecbArray[i];
        total += ecBlock.GetCount() * (ecBlock.GetDataCodewords() + ecCodewords);
      }
      totalCodewords = total;
    }

    /// <returns>the version number</returns>
    public int GetVersionNumber()
    {
      return versionNumber;
    }

    /// <returns>int[] containing the positions of the alignment pattern centers</returns>
    public int[] GetAlignmentPatternCenters()
    {
      return alignmentPatternCenters;
    }

    /// <returns>total number of code words</returns>
    public int GetTotalCodewords()
    {
      return totalCodewords;
    }

    /// <returns>the square dimension for the current version number</returns>
    public int GetDimensionForVersion()
    {
      return 17 + 4 * versionNumber;
    }

    /// <param name="ecLevel">error correction level</param>
    /// <returns>the number of EC blocks for the given error correction level</returns>
    public ECBlocks GetECBlocksForLevel(ErrorCorrectionLevel ecLevel)
    {
      return ecBlocks[ecLevel.Ordinal()];
    }

    /// <summary>
    /// Deduces version information purely from QR Code dimensions.
    /// </summary>
    /// <param name="dimension">dimension in modules</param>
    /// <returns>
    /// <see cref="Version"/> for a QR Code of that dimension
    /// </returns>
    public static Version GetProvisionalVersionForDimension(int dimension)
    {
      if(dimension % 4 != 1)
      {
        throw new ArgumentException();
      }
      try
      {
        return GetVersionForNumber((dimension - 17) >> 2);
      }
      catch(ArgumentException)
      {
        throw;
      }
    }

    /// <param name="versionNumber">Version number</param>
    /// <returns>the version for the given version number</returns>
    public static Version GetVersionForNumber(int versionNumber)
    {
      if(versionNumber < 1 || versionNumber > 40)
      {
        throw new ArgumentException();
      }
      return VERSIONS[versionNumber - 1];
    }

    /// <summary>
    /// Decode the version information.
    /// </summary>
    /// <param name="versionBits">bits stored as int containing</param>
    /// <returns>Version decoded from the versionBits</returns>
    internal static Version DecodeVersionInformation(int versionBits)
    {
      int bestDifference = int.MaxValue;
      int bestVersion = 0;
      for(int i = 0; i < VERSION_DECODE_INFO.Length; i++)
      {
        int targetVersion = VERSION_DECODE_INFO[i];
        // Do the version info bits match exactly? done.
        if(targetVersion == versionBits)
        {
          return GetVersionForNumber(i + 7);
        }
        // Otherwise see if this is the closest to a real version info bit string
        // we have seen so far
        int bitsDifference = FormatInformation.NumBitsDiffering(versionBits, targetVersion);
        if(bitsDifference < bestDifference)
        {
          bestVersion = i + 7;
          bestDifference = bitsDifference;
        }
      }
      // We can tolerate up to 3 bits of error since no two version info codewords will
      // differ in less than 4 bits.
      if(bestDifference <= 3)
      {
        return GetVersionForNumber(bestVersion);
      }
      // If we didn't find a close enough match, fail
      return null;
    }

    /// <summary>
    /// Build the function pattern, See ISO 18004:2006 Annex E.
    /// </summary>
    /// <returns>Bitmatrix containing the pattern</returns>
    internal BitMatrix BuildFunctionPattern()
    {
      int dimension = GetDimensionForVersion();
      BitMatrix bitMatrix = new BitMatrix(dimension);
      // Top left finder pattern + separator + format
      bitMatrix.SetRegion(0, 0, 9, 9);
      // Top right finder pattern + separator + format
      bitMatrix.SetRegion(dimension - 8, 0, 8, 9);
      // Bottom left finder pattern + separator + format
      bitMatrix.SetRegion(0, dimension - 8, 9, 8);
      // Alignment patterns
      int max = alignmentPatternCenters.Length;
      for(int x = 0; x < max; x++)
      {
        int i = alignmentPatternCenters[x] - 2;
        for(int y = 0; y < max; y++)
        {
          if((x == 0 && (y == 0 || y == max - 1)) || (x == max - 1 && y == 0))
          {
            // No alignment patterns near the three finder paterns
            continue;
          }
          bitMatrix.SetRegion(alignmentPatternCenters[y] - 2, i, 5, 5);
        }
      }
      // Vertical timing pattern
      bitMatrix.SetRegion(6, 9, 1, dimension - 17);
      // Horizontal timing pattern
      bitMatrix.SetRegion(9, 6, dimension - 17, 1);
      if(versionNumber > 6)
      {
        // Version info, top right
        bitMatrix.SetRegion(dimension - 11, 0, 3, 6);
        // Version info, bottom left
        bitMatrix.SetRegion(0, dimension - 11, 6, 3);
      }
      return bitMatrix;
    }

    /// <summary>
    /// Encapsulates a set of error-correction blocks in one symbol version.
    /// </summary>
    /// <remarks>
    /// Encapsulates a set of error-correction blocks in one symbol version. Most versions will use blocks of differing
    /// sizes within one version, so, this encapsulates the parameters for each set of blocks. It also holds the number
    /// of error-correction codewords per block since it will be the same across all blocks within one version.
    /// </remarks>
    public sealed class ECBlocks
    {
      private readonly int ecCodewordsPerBlock;

      private readonly ECB[] ecBlocks;

      internal ECBlocks(int ecCodewordsPerBlock, ECB ecBlocks)
      {
        this.ecCodewordsPerBlock = ecCodewordsPerBlock;
        this.ecBlocks = new ECB[] { ecBlocks };
      }

      internal ECBlocks(int ecCodewordsPerBlock, ECB ecBlocks1, ECB ecBlocks2)
      {
        this.ecCodewordsPerBlock = ecCodewordsPerBlock;
        ecBlocks = new ECB[] { ecBlocks1, ecBlocks2 };
      }

      /// <returns>The number of error-correction words per block</returns>
      public int GetECCodewordsPerBlock()
      {
        return ecCodewordsPerBlock;
      }

      public int GetNumBlocks()
      {
        int total = 0;
        for(int i = 0; i < ecBlocks.Length; i++)
        {
          total += ecBlocks[i].GetCount();
        }
        return total;
      }

      /// <returns>the total number of error-correction words</returns>
      public int GetTotalECCodewords()
      {
        return ecCodewordsPerBlock * GetNumBlocks();
      }

      public ECB[] GetECBlocks()
      {
        return ecBlocks;
      }
    }

    /// <summary>
    /// Encapsualtes the parameters for one error-correction block in one symbol version.
    /// </summary>
    /// <remarks>
    /// Encapsualtes the parameters for one error-correction block in one symbol version. This includes the number of
    /// data codewords, and the number of times a block with these parameters is used consecutively in the QR code
    /// version's format.
    /// </remarks>
    public sealed class ECB
    {
      private readonly int count;

      private readonly int dataCodewords;

      internal ECB(int count, int dataCodewords)
      {
        this.count = count;
        this.dataCodewords = dataCodewords;
      }

      public int GetCount()
      {
        return count;
      }

      public int GetDataCodewords()
      {
        return dataCodewords;
      }
    }

    /// <returns>The version number as a string</returns>
    public override string ToString()
    {
      return versionNumber.ToString();
    }

    /// <summary>
    /// See ISO 18004:2006 6.5.1 Table 9.
    /// </summary>
    private static Version[] BuildVersions()
    {
      return new Version[]
      {
        new Version(
        1,
        new int[] { },
        new ECBlocks(7, new ECB(1, 19)),
        new
                ECBlocks(10, new ECB(1, 16)),
        new ECBlocks(13, new ECB(1, 13)),
        new ECBlocks(17, new ECB(1, 9))), new Version(
        2,
        new int[] { 6, 18 },
        new ECBlocks(
          10,
          new ECB(1, 34)),
        new ECBlocks(16, new ECB(1, 28)),
        new ECBlocks(
          22,
          new ECB(
            1,
            22)),
        new ECBlocks(28, new ECB(1, 16))), new Version(
        3,
        new int[] { 6, 22 },
        new ECBlocks(15, new ECB(1, 55)),
        new ECBlocks(26, new ECB(1, 44)),
        new ECBlocks(
          18,
          new ECB(2, 17)),
        new ECBlocks(22, new ECB(2, 13))), new Version(
        4,
        new int[]
                                             {
                                               6, 26
                                             },
        new ECBlocks(20, new ECB(1, 80)),
        new ECBlocks(
          18,
          new ECB(2, 32)),
        new ECBlocks(26, new ECB(2, 24)),
        new ECBlocks(
          16,
          new ECB(
            4,
            9))), new Version(
        5,
        new int[] { 6, 30 },
        new ECBlocks(26, new ECB(1, 108)),
        new ECBlocks(24, new ECB(2, 43)),
        new ECBlocks(18, new ECB(2, 15), new ECB(2, 16)),
        new ECBlocks(22, new ECB(2, 11), new ECB(2, 12))), new Version(
        6,
        new int[]
                                                             {
                                                               6, 34
                                                             },
        new ECBlocks(18, new ECB(2, 68)),
        new ECBlocks(
          16,
          new ECB(
            4,
            27)),
        new ECBlocks(24, new ECB(4, 19)),
        new ECBlocks(
          28,
          new ECB(
            4,
            15))), new Version(
        7,
        new int[] { 6, 22, 38 },
        new ECBlocks(20, new ECB(2, 78)),
        new ECBlocks(18, new ECB(4, 31)),
        new ECBlocks(18, new ECB(2, 14), new ECB(4, 15)),
        new ECBlocks(26, new ECB(4, 13), new ECB(1, 14))), new Version(
        8,
        new int[]
                                                             {
                                                               6, 24, 42
                                                             },
        new ECBlocks(24, new ECB(2, 97)),
        new ECBlocks(
          22,
          new ECB(2, 38),
          new ECB(2, 39)),
        new ECBlocks(
          22,
          new ECB(4, 18),
          new ECB(
            2,
            19)),
        new ECBlocks(26, new ECB(4, 14), new ECB(2, 15))), new Version(
        9,
        new int
                []
                                                             {
                                                               6, 26, 46
                                                             },
        new ECBlocks(30, new ECB(2, 116)),
        new ECBlocks(
          22,
          new ECB(3, 36),
          new ECB(2, 37)),
        new ECBlocks(
          20,
          new ECB(4, 16),
          new ECB(
            4,
            17)),
        new ECBlocks(24, new ECB(4, 12), new ECB(4, 13))), new Version(
        10,
        new int
                []
                                                             {
                                                               6, 28, 50
                                                             },
        new ECBlocks(18, new ECB(2, 68), new ECB(2, 69)),
        new ECBlocks(26, new ECB(4, 43), new ECB(1, 44)),
        new ECBlocks(
          24,
          new ECB(6, 19),
          new ECB(2, 20)),
        new ECBlocks(28, new ECB(6, 15), new ECB(2, 16))), new 
                Version(
        11,
        new int[] { 6, 30, 54 },
        new ECBlocks(20, new ECB(4, 81)),
        new ECBlocks(30, new ECB(1, 50), new ECB(4, 51)),
        new ECBlocks(
          28,
          new ECB(4, 22),
          new ECB(4, 23)),
        new ECBlocks(24, new ECB(3, 12), new ECB(8, 13))), new 
                Version(
        12,
        new int[] { 6, 32, 58 },
        new ECBlocks(
          24,
          new ECB(2, 92),
          new ECB(
            2,
            93)),
        new ECBlocks(22, new ECB(6, 36), new ECB(2, 37)),
        new ECBlocks(26, new ECB(4, 20), new ECB(6, 21)),
        new ECBlocks(
          28,
          new ECB(7, 14),
          new ECB(4, 15))), new Version(
        13,
        new int[] { 6, 34, 62 },
        new ECBlocks(
          26,
          new ECB(4, 107)),
        new ECBlocks(22, new ECB(8, 37), new ECB(1, 38)),
        new ECBlocks(24, new ECB(8, 20), new ECB(4, 21)),
        new ECBlocks(
          22,
          new ECB(12, 11),
          new ECB(4, 12))), new Version(
        14,
        new int[] { 6, 26, 46, 66 },
        new ECBlocks(
          30,
          new
                ECB(3, 115),
          new ECB(1, 116)),
        new ECBlocks(
          24,
          new ECB(4, 40),
          new ECB(5, 41)),
        new ECBlocks(20, new ECB(11, 16), new ECB(5, 17)),
        new ECBlocks(24, new ECB(11, 12), new ECB(5, 13))), new Version(
        15,
        new int[] { 6, 26, 48, 70 },
        new
                ECBlocks(22, new ECB(5, 87), new ECB(1, 88)),
        new ECBlocks(
          24,
          new ECB(5, 41),
          new ECB(5, 42)),
        new ECBlocks(
          30,
          new ECB(5, 24),
          new ECB(
            7,
            25)),
        new ECBlocks(24, new ECB(11, 12), new ECB(7, 13))), new Version(
        16,
        new 
                int[]
                                                              {
                                                                6, 26, 50, 74
                                                              },
        new ECBlocks(24, new ECB(5, 98), new ECB(1, 99)),
        new
                ECBlocks(28, new ECB(7, 45), new ECB(3, 46)),
        new ECBlocks(
          24,
          new ECB(15, 19),
          new ECB(2, 20)),
        new ECBlocks(
          30,
          new ECB(3, 15),
          new ECB(
            13,
            16))), new Version(
        17,
        new int[] { 6, 30, 54, 78 },
        new ECBlocks(
          28,
          new ECB(1, 107),
          new ECB(5, 108)),
        new ECBlocks(28, new ECB(10, 46), new ECB(1, 47)),
        new ECBlocks(28, new ECB(1, 22), new ECB(15, 23)),
        new ECBlocks(
          28,
          new
                ECB(2, 14),
          new ECB(17, 15))), new Version(
        18,
        new int[] { 6, 30, 56, 82 },
        new ECBlocks(30, new ECB(5, 120), new ECB(1, 121)),
        new ECBlocks(26, new ECB(9, 43), new ECB(4, 44)),
        new ECBlocks(28, new ECB(17, 22), new ECB(1, 23)),
        new ECBlocks(28, new ECB(2, 14), new ECB(19, 15))), new Version(
        19,
        new int[]
                                                              {
                                                                6, 30, 58, 86
                                                              },
        new ECBlocks(28, new ECB(3, 113), new ECB(4, 114)),
        new ECBlocks(26, new ECB(3, 44), new ECB(11, 45)),
        new ECBlocks(26, new ECB(17, 21), new ECB(4, 22)),
        new ECBlocks(26, new ECB(9, 13), new ECB(16, 14))),
        new Version(
        20,
        new int[] { 6, 34, 62, 90 },
        new ECBlocks(
          28,
          new ECB(3, 107),
          new ECB(5, 108)),
        new ECBlocks(26, new ECB(3, 41), new ECB(13, 42)),
        new ECBlocks(30, new ECB(15, 24), new ECB(5, 25)),
        new ECBlocks(28, new ECB(15, 15), new ECB(10, 16))), new Version(
        21,
        new int[] { 6, 28, 50, 72, 94 },
        new ECBlocks(
          28,
          new ECB(4, 116),
          new ECB(4, 117)),
        new ECBlocks(26, new ECB(17, 42)),
        new ECBlocks(28, new ECB(17, 22), new ECB(6, 23)),
        new ECBlocks(
          30,
          new ECB(19, 16),
          new ECB(6, 17))), new Version(
        22,
        new int[] { 6, 26, 50, 74, 98 },
        new
                ECBlocks(28, new ECB(2, 111), new ECB(7, 112)),
        new ECBlocks(
          28,
          new ECB(17, 46)),
        new ECBlocks(30, new ECB(7, 24), new ECB(16, 25)),
        new ECBlocks(24, new ECB(34, 13))), new Version(
        23,
        new int[] { 6, 30, 54, 74, 102 },
        new ECBlocks(30, new ECB(4, 121), new ECB(5, 122)),
        new ECBlocks(28, new ECB(4, 47), new ECB(14, 48)),
        new ECBlocks(30, new ECB(11, 24), new ECB(14, 25)),
        new ECBlocks(30, new ECB(16, 15), new ECB(14, 16))),
        new Version(
        24,
        new int
                []
          {
            6, 28, 54, 80, 106
          },
        new ECBlocks(30, new ECB(6, 117), new ECB(4, 118)),
        new ECBlocks(28, new ECB(6, 45), new ECB(14, 46)),
        new ECBlocks(
          30,
          new
                ECB(11, 24),
          new ECB(16, 25)),
        new ECBlocks(
          30,
          new ECB(30, 16),
          new ECB(2, 17))), new Version(
        25,
        new int[] { 6, 32, 58, 84, 110 },
        new ECBlocks(
          26,
          new ECB(
            8,
            106),
          new ECB(4, 107)),
        new ECBlocks(
          28,
          new ECB(8, 47),
          new ECB(
            13,
            48)),
        new ECBlocks(30, new ECB(7, 24), new ECB(22, 25)),
        new ECBlocks(30, new ECB(22, 15), new ECB(13, 16))), new Version(
        26,
        new int[]
                                                               {
                                                                 6, 30, 58, 86, 114
                                                               },
        new ECBlocks(28, new ECB(10, 114), new ECB(2, 115)),
        new ECBlocks(
          28,
          new ECB(19, 46),
          new ECB(4, 47)),
        new ECBlocks(
          28,
          new ECB(28, 22),
          new ECB(6, 23)),
        new ECBlocks(30, new ECB(33, 16), new ECB(4, 17))),
        new Version(
        27,
        new int[] { 6, 34, 62, 90, 118 },
        new ECBlocks(
          30,
          new ECB(8, 122),
          new
                ECB(4, 123)),
        new ECBlocks(28, new ECB(22, 45), new ECB(3, 46)),
        new ECBlocks(30, new ECB(8, 23), new ECB(26, 24)),
        new ECBlocks(30, new ECB(12, 15), new ECB(28, 16))), new Version(
        28,
        new int[] { 6, 26, 50, 74, 98, 122 },
        new ECBlocks(30, new ECB(3, 117), new ECB(10, 118)),
        new ECBlocks(28, new ECB(3, 45), new ECB(23, 46)),
        new ECBlocks(30, new ECB(4, 24), new ECB(31, 25)),
        new ECBlocks(30, new ECB(11, 15), new ECB(31, 16))), new Version(
        29,
        new int
                []
                                                               {
                                                                 6, 30, 54, 78, 102, 126
                                                               },
        new ECBlocks(30, new ECB(7, 116), new ECB(7, 117)),
        new ECBlocks(28, new ECB(21, 45), new ECB(7, 46)),
        new ECBlocks(
          30,
          new ECB(1, 23),
          new ECB(37, 24)),
        new ECBlocks(
          30,
          new ECB(19, 15),
          new ECB(26, 16))), new Version(
        30,
        new int[] { 6, 26, 52, 78, 104, 130 },
        new ECBlocks(30, new ECB(5, 115), new ECB(10, 116)),
        new ECBlocks(
          28,
          new ECB(
            19,
            47),
          new ECB(10, 48)),
        new ECBlocks(30, new ECB(15, 24), new ECB(25, 25)),
        new ECBlocks(30, new ECB(23, 15), new ECB(25, 16))), new Version(
        31,
        new int
                []
                                                               {
                                                                 6, 30, 56, 82, 108, 134
                                                               },
        new ECBlocks(
          30,
          new ECB(13, 115),
          new ECB(
            3,
            116)),
        new ECBlocks(28, new ECB(2, 46), new ECB(29, 47)),
        new ECBlocks(30, new ECB(42, 24), new ECB(1, 25)),
        new ECBlocks(30, new ECB(23, 15), new ECB(28, 16))), new Version(
        32,
        new int[] { 6, 34, 60, 86, 112, 138 },
        new ECBlocks(30, new ECB(17, 115)),
        new ECBlocks(
          28,
          new ECB(10, 46),
          new ECB(
            23,
            47)),
        new ECBlocks(30, new ECB(10, 24), new ECB(35, 25)),
        new ECBlocks(30, new ECB(19, 15), new ECB(35, 16))),
        new Version(
        33,
        new int[] { 6, 30, 58, 86, 114, 142 },
        new ECBlocks(30, new ECB(17, 115), new ECB(1, 116)),
        new ECBlocks(28, new ECB(14, 46), new ECB(21, 47)),
        new ECBlocks(30, new ECB(29, 24), new ECB(19, 25)),
        new ECBlocks(30, new ECB(11, 15), new ECB(46, 16))),
        new Version(
        34,
        new int[] { 6, 34, 62, 90, 118, 146 },
        new ECBlocks(
          30,
          new ECB(
            13,
            115),
          new ECB(6, 116)),
        new ECBlocks(
          28,
          new ECB(14, 46),
          new ECB(
            23,
            47)),
        new ECBlocks(30, new ECB(44, 24), new ECB(7, 25)),
        new ECBlocks(30, new ECB(59, 16), new ECB(1, 17))),
        new Version(
        35,
        new int[] { 6, 30, 54, 78, 102, 126, 150 },
        new ECBlocks(30, new ECB(12, 121), new ECB(7, 122)),
        new ECBlocks(28, new ECB(12, 47), new ECB(26, 48)),
        new ECBlocks(30, new ECB(39, 24), new ECB(14, 25)),
        new ECBlocks(30, new ECB(22, 15), new ECB(41, 16))),
        new Version(
        36,
        new int[] { 6, 24, 50, 76, 102, 128, 154 },
        new ECBlocks(
          30,
          new ECB(6, 121),
          new ECB(14, 122)),
        new ECBlocks(
          28,
          new ECB(6, 47),
          new ECB(
            34,
            48)),
        new ECBlocks(30, new ECB(46, 24), new ECB(10, 25)),
        new ECBlocks(30, new ECB(2, 15), new ECB(64, 16))),
        new Version(
        37,
        new int[] { 6, 28, 54, 80, 106, 132, 158 },
        new ECBlocks(30, new ECB(17, 122), new ECB(4, 123)),
        new ECBlocks(28, new ECB(29, 46), new ECB(14, 47)),
        new ECBlocks(30, new ECB(49, 24), new ECB(10, 25)),
        new ECBlocks(30, new ECB(24, 15), new ECB(46, 16))),
        new Version(
        38,
        new int[] { 6, 32, 58, 84, 110, 136, 162 },
        new ECBlocks(
          30,
          new ECB(4, 122),
          new ECB(18, 123)),
        new ECBlocks(
          28,
          new ECB(13, 46),
          new ECB(32, 47)),
        new ECBlocks(30, new ECB(48, 24), new ECB(14, 25)),
        new ECBlocks(30, new ECB(42, 15), new ECB(32, 16))),
        new Version(
        39,
        new int[] { 6, 26, 54, 82, 110, 138, 166 },
        new ECBlocks(30, new ECB(20, 117), new ECB(4, 118)),
        new ECBlocks(28, new ECB(40, 47), new ECB(7, 48)),
        new ECBlocks(30, new ECB(43, 24), new ECB(22, 25)),
        new ECBlocks(30, new ECB(10, 15), new ECB(67, 16))),
        new Version(
        40,
        new int[] { 6, 30, 58, 86, 114, 142, 170 },
        new ECBlocks(
          30,
          new ECB(19, 118),
          new ECB(6, 119)),
        new ECBlocks(
          28,
          new ECB(18, 47),
          new ECB(31, 48)),
        new ECBlocks(30, new ECB(34, 24), new ECB(34, 25)),
        new ECBlocks(30, new ECB(20, 15), new ECB(61, 16)))
      };
    }
  }
}
