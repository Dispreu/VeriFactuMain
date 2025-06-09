using System;
using System.Collections.Generic;
using System.Linq;

namespace VeriFactuTest
{
  public partial class MainForm : System.Windows.Forms.Form
  {
    private readonly Auxiliar.VeriFactuWrapper.VeriFactuHelper veriFactuHelper;

    public MainForm()
    {
      veriFactuHelper = new Auxiliar.VeriFactuWrapper.VeriFactuHelper("B67858753", "SES ILLES BAIX COST SOCIEDAD LIMITADA", "64A609F86721D1DB658BCD211CAA89DC", true, addMessage, (msg, isError) => addMessage(msg));
      veriFactuHelper.SistemaInformaticoNombreSistemaInformatico = "GestionNet";
      veriFactuHelper.SistemaInformaticoIdSistemaInformatico = "V4";
      veriFactuHelper.SistemaInformaticoNumeroInstalacion = "4.7.9256.1711";
      veriFactuHelper.SistemaInformaticoTipoUsoPosibleMultiOT = "N";
      veriFactuHelper.SistemaInformaticoTipoUsoPosibleSoloVerifactu = "S";
      veriFactuHelper.SaveSettings();
      InitializeComponent();
    }

    private void btnSendRegular_Click(object sender, EventArgs e)
    {
      veriFactuHelper.EnviarFactura(false);
      //enviarFacturaRegular();
    }

    private void btnSendSimplificada_Click(object sender, EventArgs e)
    {
      veriFactuHelper.EnviarFactura(false);
      //enviarFacturaSimplificada();
    }

    private void enviarFacturaRegular()
    {
      // Creamos una instacia de la clase factura
      VeriFactu.Business.Invoice invoice = new VeriFactu.Business.Invoice("GIT-EJ-0002", new DateTime(2024, 11, 15), "B67858753")
      {
        InvoiceType = VeriFactu.Xml.Factu.Alta.TipoFactura.F1,
        SellerName = "SES ILLES BAIX COST SOCIEDAD LIMITADA",
        BuyerID = "41494210W",
        BuyerName = "MAGDALENA SOCIAS FERNANDEZ",
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
      //--- Obtener el Hash de la factura.
      var blockchain = VeriFactu.Blockchain.Blockchain.Get(invoice.SellerID);
      // Obtenemos una instancia de la clase RegistroAlta a partir de
      // la instancia del objeto de negocio Invoice
      var registro = invoice.GetRegistroAlta();
      // Añadimos el registro de alta
      blockchain.Add(registro);
      addMessage($"La huella de la factura es: {registro.GetHashOutput()}");
      //--- Creamos la entrada de la factura
      VeriFactu.Business.InvoiceEntry invoiceEntry = new VeriFactu.Business.InvoiceEntry(invoice);
      try
      {
        //--- Guardamos la factura
        invoiceEntry.Save();
      }
      catch(Exception ex)
      {
        addMessage($"Exception:\n{ex.Message}");
      }
      // Consultamos el estado
      addMessage($"Respuesta de la AEAT:\n{invoiceEntry.Status}");
      if(invoiceEntry.Status == "Correcto")
      {
        // Consultamos el CSV
        addMessage($"Respuesta de la AEAT:\n{invoiceEntry.CSV}");
      }
      else
      {
        // Consultamos el error
        addMessage($"Respuesta de la AEAT:\n{invoiceEntry.ErrorCode}: {invoiceEntry.ErrorDescription}");
      }
      // Consultamos el resultado devuelto por la AEAT
      addMessage($"Respuesta de la AEAT:\n{invoiceEntry.Response}");
    }

    private void enviarFacturaSimplificada()
    {
      // Creamos una instacia de la clase factura
      VeriFactu.Business.Invoice invoice = new VeriFactu.Business.Invoice("54-2025000001", new DateTime(2024, 12, 4), "B67858753")
      {
        InvoiceType = VeriFactu.Xml.Factu.Alta.TipoFactura.F2,
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
      // Creamos la entrada de la factura
      VeriFactu.Business.InvoiceEntry invoiceEntry = new VeriFactu.Business.InvoiceEntry(invoice);
      try
      {
        // Guardamos la factura
        invoiceEntry.Save();
      }
      catch(Exception ex)
      {
        addMessage($"Exception:\n{ex.Message}");
      }
      // Consultamos el estado
      addMessage($"Respuesta de la AEAT:\n{invoiceEntry.Status}");
      if(invoiceEntry.Status == "Correcto")
      {
        // Consultamos el CSV
        addMessage($"Respuesta de la AEAT:\n{invoiceEntry.CSV}");
      }
      else
      {
        // Consultamos el error
        addMessage($"Respuesta de la AEAT:\n{invoiceEntry.ErrorCode}: {invoiceEntry.ErrorDescription}");
      }
      // Consultamos el resultado devuelto por la AEAT
      addMessage($"Respuesta de la AEAT:\n{invoiceEntry.Response}");
    }

    private void addMessage(string msg)
    {
      List<string> lineasExistentes = editMemo.Lines.ToList();
      lineasExistentes.Add(msg);
      editMemo.Lines = lineasExistentes.ToArray();
    }
  }
}
