﻿/*
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

using System.Xml.Serialization;

namespace VeriFactu.Xml.Factu
{
  /// <summary>
  /// Clave para establecer el tipo de identificación en el pais de residencia. L7.
  /// </summary>
  public enum IDType
  {
    /// <summary>
    /// NIF-IVA (02).
    /// </summary>
    [XmlEnum("02")]
        NIF_IVA = 2,

    /// <summary>
    /// PASAPORTE (03).
    /// </summary>
    [XmlEnum("03")]
        PASAPORTE = 3,

    /// <summary>
    /// DOCUMENTO OFICIAL DE IDENTIFICACIÓN EXPEDIDO POR EL PAÍS O TERRITORIO DE RESIDENCIA (04).
    /// </summary>
    [XmlEnum("04")]
        DOCUMENTO_OFICIAL = 4,

    /// <summary>
    /// CERTIFICADO DE RESIDENCIA (05).
    /// </summary>
    [XmlEnum("05")]
        CERTIFICADO_RESIDENCIA = 5,

    /// <summary>
    /// OTRO DOCUMENTO PROBATORIO (06).
    /// </summary>
    [XmlEnum("06")]
        OTRO_DOC_PROBATORIO = 6,

    /// <summary>
    /// NO CENSADO (07).
    /// </summary>
    [XmlEnum("07")]
        NO_CENSADO = 7
  }
}