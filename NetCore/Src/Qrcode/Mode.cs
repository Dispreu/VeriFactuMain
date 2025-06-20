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
  /// See ISO 18004:2006, 6.4.1, Tables 2 and 3.
  /// </summary>
  /// <remarks>
  /// See ISO 18004:2006, 6.4.1, Tables 2 and 3. This enum encapsulates the various modes in which data can be encoded
  /// to bits in the QR code standard.
  /// </remarks>
  /// <author>Sean Owen</author>
  internal sealed class Mode
  {
    // Not really a mode...
    public static readonly Mode TERMINATOR = new Mode(
      new int[]
      {
        0, 0, 0
      },
      0x00,
      "TERMINATOR");

    public static readonly Mode NUMERIC = new Mode(
      new int[]
      {
        10,
        12, 14
      },
      0x01,
      "NUMERIC");

    public static readonly Mode ALPHANUMERIC = new Mode(
      new int[]
      {
        9, 11, 13
      },
      0x02,
      "ALPHANUMERIC");

    // Not supported
    public static readonly Mode STRUCTURED_APPEND = new Mode(
      new int
            []
      {
        0, 0, 0
      },
      0x03,
      "STRUCTURED_APPEND");

    public static readonly Mode BYTE = new Mode(
      new int[]
      {
        8, 16,
        16
      },
      0x04,
      "BYTE");

    // character counts don't apply
    public static readonly Mode ECI = new Mode(null, 0x07, "ECI");

    public static readonly Mode KANJI = new Mode(new int[] { 8, 10, 12 }, 0x08, "KANJI");

    public static readonly Mode FNC1_FIRST_POSITION = new Mode(
      null,
      0x05,
      "FNC1_FIRST_POSITION");

    public static readonly Mode FNC1_SECOND_POSITION = new Mode(
      null,
      0x09,
      "FNC1_SECOND_POSITION");

    private readonly int[] characterCountBitsForVersions;

    private readonly int bits;

    private readonly string name;

    private Mode(int[] characterCountBitsForVersions, int bits, string name)
    {
      this.characterCountBitsForVersions = characterCountBitsForVersions;
      this.bits = bits;
      this.name = name;
    }

    /// <param name="bits">four bits encoding a QR Code data mode</param>
    /// <returns>
    /// <see cref="Mode"/> encoded by these bits
    /// </returns>
    public static Mode ForBits(int bits)
    {
      switch(bits)
      {
        case 0x0:
          return TERMINATOR;
        case 0x1:
          return NUMERIC;
        case 0x2:
          return ALPHANUMERIC;
        case 0x3:
          return STRUCTURED_APPEND;
        case 0x4:
          return BYTE;
        case 0x5:
          return FNC1_FIRST_POSITION;
        case 0x7:
          return ECI;
        case 0x8:
          return KANJI;
        case 0x9:
          return FNC1_SECOND_POSITION;
        default:
          throw new ArgumentException();
      }
    }

    /// <param name="version">version in question</param>
    /// <returns>
    /// number of bits used, in this QR Code symbol <see cref="Version"/> , to encode the count of characters that will
    /// follow encoded in this <see cref="Mode"/>
    /// </returns>
    public int GetCharacterCountBits(Version version)
    {
      if(characterCountBitsForVersions == null)
      {
        throw new ArgumentException("Character count doesn't apply to this mode");
      }
      int number = version.GetVersionNumber();
      int offset;
      if(number <= 9)
      {
        offset = 0;
      }
      else
      {
        if(number <= 26)
        {
          offset = 1;
        }
        else
        {
          offset = 2;
        }
      }
      return characterCountBitsForVersions[offset];
    }

    /// <returns>the bits of the mode</returns>
    public int GetBits()
    {
      return bits;
    }

    /// <returns>the name of the mode.</returns>
    public string GetName()
    {
      return name;
    }

    /// <returns>the name of the mode.</returns>
    public override string ToString()
    {
      return name;
    }
  }
}
