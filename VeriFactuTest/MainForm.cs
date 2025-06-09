using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace VeriFactuTest
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      // Carpeta del sistema dónde se ubica el archivo Settings.xml
      string settingsFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}{System.IO.Path.DirectorySeparatorChar}VeriFactu";
      // Cambio configuración a los endpoints de producción
      VeriFactu.Config.Settings.Current.VeriFactuEndPointPrefix = VeriFactu.VeriFactuEndPointPrefixes.Test;
      VeriFactu.Config.Settings.Current.VeriFactuEndPointValidatePrefix = VeriFactu.VeriFactuEndPointPrefixes.TestValidate;
      VeriFactu.Config.Settings.Current.CertificateSerial = "64A609F86721D1DB658BCD211CAA89DC"; //--- Ses Illes
      // Guardo los cambios
      VeriFactu.Config.Settings.Save();
    }

    private void btnSendRegular_Click(object sender, EventArgs e)
    {
      enviarFacturaRegular();
    }

    private void btnSendSimplificada_Click(object sender, EventArgs e)
    {
      enviarFacturaSimplificada();
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
