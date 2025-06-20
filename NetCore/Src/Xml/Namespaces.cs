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

using System.Collections.Generic;

namespace VeriFactu.Xml
{
  /// <summary>
  /// Espacios de nombre de VERI*FACTU.
  /// </summary>
  public class Namespaces
  {

        #region Propiedades Públicas Estáticas

    /// <summary>
    /// Prefijo de espacios de nombres AEAT TIKE CONT.
    /// </summary>
    public const string NamespacePrefix = "https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/tike/cont/ws/";

    /// <summary>
    /// Espacio de nombres SF (sum1) y (sum) para Consulta
    /// </summary>
    public const string NamespaceSF = NamespacePrefix + "SuministroInformacion.xsd";

    /// <summary>
    /// Espacio de nombres SF (sum)
    /// </summary>
    public const string NamespaceSFLR = NamespacePrefix + "SuministroLR.xsd";

    /// <summary>
    /// Espacio de nombres RespuestaSuministro
    /// </summary>
    public const string NamespaceTikR = NamespacePrefix + "RespuestaSuministro.xsd";

    /// <summary>
    /// Espacio de nombres ConsultaLR (con)
    /// </summary>
    public const string NamespaceCon = NamespacePrefix + "ConsultaLR.xsd";

    /// <summary>
    /// Espacio de nombres RespuestaConsultaLR (tikLRRC)
    /// </summary>
    public const string NamespaceTikLRRC = NamespacePrefix + "RespuestaConsultaLR.xsd";

    /// <summary>
    /// Espacio de nombres web service validación NIF.
    /// </summary>
    public const string NamespaceVNifV2Ent = "http://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/burt/jdit/ws/VNifV2Ent.xsd";

    /// <summary>
    /// Espacio de nombres web service validación NIF.
    /// </summary>
    public const string NamespaceVNifV2Sal = "http://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/burt/jdit/ws/VNifV2Sal.xsd";

    /// <summary>
    /// Espacio de nombres soap
    /// </summary>
    public const string NamespaceSoap = "http://schemas.xmlsoap.org/soap/envelope/";

    /// <summary>
    /// Diccionario de espacios de nombres.
    /// </summary>
    public static Dictionary<string, string> Items = new Dictionary<string, string>
    {
      { "soapenv", NamespaceSoap },
      { "sum", NamespaceSFLR },
      { "sum1", NamespaceSF },
      { "con", NamespaceCon }
    };

    /// <summary>
    /// Diccionario de espacios de nombres para el servicio de validación de NIF de la AEAT.
    /// </summary>
    public static Dictionary<string, string> NifItems = new Dictionary<string, string>
    {
      { "soapenv", NamespaceSoap },
      { "VNifV2Ent", NamespaceVNifV2Ent },
      { "VNifV2Sal", NamespaceVNifV2Sal }
    };

    #endregion       
  }
}