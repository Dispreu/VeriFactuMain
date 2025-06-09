using System;
using System.Linq;

namespace Auxiliar.VeriFactuWrapper
{
  public interface IVeriFactuHelper
  {
    /// <summary>
    /// En la versión implantado, se pasará un objeto Albaran o Historico a este método.
    /// </summary>
    /// <param name="isFacturaSimplificada"></param>
    /// <returns>True si se ha enviado correctamente, sino False</returns>
    bool EnviarFactura(bool isFacturaSimplificada);
    void SaveSettings();

    string CertificateSerial { get; }
    string EmisorNif { get; }
    string EmisorRazonSocial { get; }
    string SettingsFolder { get; }
    string SistemaInformaticoIdSistemaInformatico { get; set; }
    string SistemaInformaticoNIF { get; set; }
    string SistemaInformaticoNombreRazon { get; set; }
    string SistemaInformaticoNombreSistemaInformatico { get; set; }
    string SistemaInformaticoNumeroInstalacion { get; set; }
    string SistemaInformaticoTipoUsoPosibleMultiOT { get; set; }
    string SistemaInformaticoTipoUsoPosibleSoloVerifactu { get; set; }
    string VeriFactuEndPointPrefix { get; }
    string VeriFactuEndPointValidatePrefix { get; }
  }
}
