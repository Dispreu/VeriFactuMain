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
  /// See ISO 18004:2006, 6.5.1.
  /// </summary>
  /// <remarks>
  /// See ISO 18004:2006, 6.5.1. This enum encapsulates the four error correction levels defined by the QR code
  /// standard.
  /// </remarks>
  /// <author>Sean Owen</author>
  public sealed class ErrorCorrectionLevel
  {
    /// <summary>
    /// L = ~7% correction
    /// </summary>
    public static readonly ErrorCorrectionLevel L = new ErrorCorrectionLevel(0, 0x01, "L");

    /// <summary>
    /// M = ~15% correction
    /// </summary>
    public static readonly ErrorCorrectionLevel M = new ErrorCorrectionLevel(1, 0x00, "M");

    /// <summary>
    /// Q = ~25% correction
    /// </summary>
    public static readonly ErrorCorrectionLevel Q = new ErrorCorrectionLevel(2, 0x03, "Q");

    /// <summary>
    /// H = ~30% correction
    /// </summary>
    public static readonly ErrorCorrectionLevel H = new ErrorCorrectionLevel(3, 0x02, "H");

    private static readonly ErrorCorrectionLevel[] FOR_BITS = new ErrorCorrectionLevel
            []
    {
      M, L, H, Q
    };

    private readonly int ordinal;

    private readonly int bits;

    private readonly string name;

    private ErrorCorrectionLevel(int ordinal, int bits, string name)
    {
      this.ordinal = ordinal;
      this.bits = bits;
      this.name = name;
    }

    /// <summary>
    /// Gets the ordinal value.
    /// </summary>
    /// <returns>the ordinal</returns>
    public int Ordinal()
    {
      return ordinal;
    }

    /// <summary>
    /// Returns bits.
    /// </summary>
    /// <returns>bits</returns>
    public int GetBits()
    {
      return bits;
    }

    /// <summary>
    /// Returns name.
    /// </summary>
    /// <returns>name</returns>
    public string GetName()
    {
      return name;
    }

    /// <summary>
    /// Textual representation of the instance.
    /// </summary>
    /// <returns>Textual representation of the instance.</returns>
    public override string ToString()
    {
      return name;
    }

    /// <summary>
    /// error correction level
    /// </summary>
    /// <param name="bits">int containing the two bits encoding a QR Code's error correction level</param>
    /// <see cref="ErrorCorrectionLevel"/>
    /// <returns>error correction level</returns>
    public static ErrorCorrectionLevel ForBits(int bits)
    {
      if(bits < 0 || bits >= FOR_BITS.Length)
      {
        throw new ArgumentException();
      }
      return FOR_BITS[bits];
    }
  }
}
