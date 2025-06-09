using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auxiliar.VeriFactuWrapper
{
  public class VeriFactuHelper : IVeriFactuHelper
  {
    private readonly bool isEntornoTest;
    private readonly Action<string> feedback;
    private readonly Action<string, bool> reportError;

    #region C'tor

    public VeriFactuHelper(string emisorNif, string emisorRazonSocial, string certificateSerial, bool isEntornoTest, Action<string> feedback, Action<string, bool> reportError)
    {
      this.isEntornoTest = isEntornoTest;
      this.feedback = feedback;
      this.reportError = reportError;
      //--- Carpeta del sistema dónde se ubica el archivo Settings.xml
      SettingsFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}{System.IO.Path.DirectorySeparatorChar}VeriFactu";
      // Cambio configuración a los endpoints de producción
      VeriFactu.Config.Settings.Current.VeriFactuEndPointPrefix = isEntornoTest ? VeriFactu.VeriFactuEndPointPrefixes.Test : VeriFactu.VeriFactuEndPointPrefixes.Prod;
      VeriFactu.Config.Settings.Current.VeriFactuEndPointValidatePrefix = isEntornoTest ? VeriFactu.VeriFactuEndPointPrefixes.TestValidate : VeriFactu.VeriFactuEndPointPrefixes.ProdValidate;
      VeriFactu.Config.Settings.Current.CertificateSerial = certificateSerial; // "64A609F86721D1DB658BCD211CAA89DC"; //--- Ses Illes
    }

    #endregion

    #region Propiedades públicas

    //--- Datos fiscales del emisor
    public string EmisorNif { get; private set; }
    public string EmisorRazonSocial { get; private set; }
    public string SettingsFolder { get; private set; }

    //--- Confoguración para Veri*Factu
    public string VeriFactuEndPointPrefix
    {
      get => VeriFactu.Config.Settings.Current.VeriFactuEndPointPrefix;
      private set => VeriFactu.Config.Settings.Current.VeriFactuEndPointPrefix = value;
    }
    public string VeriFactuEndPointValidatePrefix
    {
      get => VeriFactu.Config.Settings.Current.VeriFactuEndPointValidatePrefix;
      private set => VeriFactu.Config.Settings.Current.VeriFactuEndPointValidatePrefix = value;
    }
    public string CertificateSerial
    {
      get => VeriFactu.Config.Settings.Current.CertificateSerial;
      private set => VeriFactu.Config.Settings.Current.CertificateSerial = value;
    }

    //--- Datos del sistema informático
    public string SistemaInformaticoNIF
    {
      get => VeriFactu.Config.Settings.Current.SistemaInformatico.NIF;
      set => VeriFactu.Config.Settings.Current.SistemaInformatico.NIF = value;
    }
    public string SistemaInformaticoNombreRazon
    {
      get => VeriFactu.Config.Settings.Current.SistemaInformatico.NombreRazon;
      set => VeriFactu.Config.Settings.Current.SistemaInformatico.NombreRazon = value;
    }
    public string SistemaInformaticoNombreSistemaInformatico
    {
      get => VeriFactu.Config.Settings.Current.SistemaInformatico.NombreSistemaInformatico;
      set => VeriFactu.Config.Settings.Current.SistemaInformatico.NombreSistemaInformatico = value;
    }
    public string SistemaInformaticoIdSistemaInformatico
    {
      get => VeriFactu.Config.Settings.Current.SistemaInformatico.IdSistemaInformatico;
      set => VeriFactu.Config.Settings.Current.SistemaInformatico.IdSistemaInformatico = value;
    }
    public string SistemaInformaticoNumeroInstalacion
    {
      get => VeriFactu.Config.Settings.Current.SistemaInformatico.NumeroInstalacion;
      set => VeriFactu.Config.Settings.Current.SistemaInformatico.NumeroInstalacion = value;
    }
    public string SistemaInformaticoTipoUsoPosibleMultiOT
    {
      get => VeriFactu.Config.Settings.Current.SistemaInformatico.TipoUsoPosibleMultiOT;
      set => VeriFactu.Config.Settings.Current.SistemaInformatico.TipoUsoPosibleMultiOT = value;
    }
    public string SistemaInformaticoTipoUsoPosibleSoloVerifactu
    {
      get => VeriFactu.Config.Settings.Current.SistemaInformatico.TipoUsoPosibleSoloVerifactu;
      set => VeriFactu.Config.Settings.Current.SistemaInformatico.TipoUsoPosibleSoloVerifactu = value;
    }

    #endregion

    #region Métodos de IVeriFactuHelper

    public void SaveSettings()
    {
      VeriFactu.Config.Settings.Save();
    }

    /// <summary>
    /// En la versión implantado, se pasará un objeto Albaran o Historico a este método.
    /// </summary>
    /// <param name="isFacturaSimplificada"></param>
    /// <returns>True si se ha enviado correctamente, sino False</returns>
    public bool EnviarFactura(bool isFacturaSimplificada)
    {
      //--- Creamos una instacia de la clase factura
      VeriFactu.Business.Invoice invoice = new VeriFactu.Business.Invoice("GIT-EJ-0002", new DateTime(2024, 11, 15), "B67858753")
      {
        InvoiceType = isFacturaSimplificada ? VeriFactu.Xml.Factu.Alta.TipoFactura.F2 : VeriFactu.Xml.Factu.Alta.TipoFactura.F1,
        SellerName = "SES ILLES BAIX COST SOCIEDAD LIMITADA",
        Text = "PRESTACION SERVICIOS DESARROLLO SOFTWARE",
        TaxItems = new List<VeriFactu.Business.TaxItem>
          {
            new VeriFactu.Business.TaxItem
            {
              TaxRate = 4,
              TaxBase = 10,
              TaxAmount = 0.4m
            },
            new VeriFactu.Business.TaxItem
            {
              TaxRate = 21,
              TaxBase = 100,
              TaxAmount = 21
            }
          }
      };
      if(!isFacturaSimplificada)
      {
        invoice.BuyerID = "41494210W";
        invoice.BuyerName = "MAGDALENA SOCIAS FERNANDEZ";
      }
      //--- Obtener el Hash de la factura.
      VeriFactu.Blockchain.Blockchain blockchain = VeriFactu.Blockchain.Blockchain.Get(invoice.SellerID);
      // Obtenemos una instancia de la clase RegistroAlta a partir de
      // la instancia del objeto de negocio Invoice
      VeriFactu.Xml.Factu.Alta.RegistroAlta registro = invoice.GetRegistroAlta();
      // Añadimos el registro de alta
      blockchain.Add(registro);
      feedback($"La huella de la factura es: {registro.GetHashOutput()}");
      //--- Creamos la entrada de la factura
      VeriFactu.Business.InvoiceEntry invoiceEntry = new VeriFactu.Business.InvoiceEntry(invoice);
      try
      {
        //--- Guardamos la factura
        invoiceEntry.Save();
      }
      catch(Exception ex)
      {
        reportError($"Exception:\n{ex.Message}", true);
      }
      //--- Consultamos el estado
      feedback($"Respuesta de la AEAT:\n{invoiceEntry.Status}");
      if(invoiceEntry.Status == "Correcto")
      {
        //--- Consultamos el CSV
        feedback($"Respuesta de la AEAT:\n{invoiceEntry.CSV}");
      }
      else
      {
        //--- Consultamos el error
        feedback($"Respuesta de la AEAT:\n{invoiceEntry.ErrorCode}: {invoiceEntry.ErrorDescription}");
      }
      //--- Consultamos el resultado devuelto por la AEAT
      feedback($"Respuesta de la AEAT:\n{invoiceEntry.Response}");
      return (invoiceEntry.Status == "Correcto");
    }

    #endregion

  }
}
