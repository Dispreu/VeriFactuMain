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

using VeriFactu.Net.Rest.Json.Parser.Lexer;

namespace VeriFactu.Net.Rest.Json.Parser
{
  /// <summary>
  /// Deserializador JSON.
  /// </summary>
  public class JsonParser
  {

    #region Variables Privadas de Instacia

    /// <summary>
    /// Analizador léxico.
    /// </summary>
    private JsonLexer _JsonLexer;

    /// <summary>
    /// Lector de fragmentos utilizado para compener el objeto resultado.
    /// </summary>
    private JsonParserReader _JsonParserReader;

    #endregion

    #region Construtores de Instancia

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="jsonText">Texto JSON.</param>
    public JsonParser(string jsonText)
    {
      _JsonLexer = new JsonLexer(jsonText);
      _JsonParserReader = new JsonParserReader(_JsonLexer);
      _JsonParserReader.Read();
    }

    #endregion

    #region Métodos Públicos de Instancia

    /// <summary>
    /// Devuelve el resultado de la deserialización de la cadena JSON.
    /// </summary>
    /// <returns>
    /// resultado de la deserialización de la cadena JSON.
    /// </returns>
    public dynamic GetResult()
    {
      return _JsonParserReader.GetResult();
    }

    #endregion
  }
}