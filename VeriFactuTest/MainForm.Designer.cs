namespace VeriFactuTest
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if(disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.editMemo = new System.Windows.Forms.TextBox();
      this.panel1 = new System.Windows.Forms.Panel();
      this.btnSendSimplificada = new System.Windows.Forms.Button();
      this.btnSendRegular = new System.Windows.Forms.Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // editMemo
      // 
      this.editMemo.BackColor = System.Drawing.Color.Black;
      this.editMemo.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.editMemo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.editMemo.ForeColor = System.Drawing.Color.Lime;
      this.editMemo.Location = new System.Drawing.Point(0, 63);
      this.editMemo.Multiline = true;
      this.editMemo.Name = "editMemo";
      this.editMemo.ReadOnly = true;
      this.editMemo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editMemo.Size = new System.Drawing.Size(800, 387);
      this.editMemo.TabIndex = 1;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.btnSendSimplificada);
      this.panel1.Controls.Add(this.btnSendRegular);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(800, 58);
      this.panel1.TabIndex = 2;
      // 
      // btnSendSimplificada
      // 
      this.btnSendSimplificada.Dock = System.Windows.Forms.DockStyle.Fill;
      this.btnSendSimplificada.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnSendSimplificada.Location = new System.Drawing.Point(382, 0);
      this.btnSendSimplificada.Name = "btnSendSimplificada";
      this.btnSendSimplificada.Size = new System.Drawing.Size(418, 58);
      this.btnSendSimplificada.TabIndex = 2;
      this.btnSendSimplificada.Text = "Send Factura Simplificada";
      this.btnSendSimplificada.UseVisualStyleBackColor = true;
      this.btnSendSimplificada.Click += new System.EventHandler(this.btnSendSimplificada_Click);
      // 
      // btnSendRegular
      // 
      this.btnSendRegular.Dock = System.Windows.Forms.DockStyle.Left;
      this.btnSendRegular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnSendRegular.Location = new System.Drawing.Point(0, 0);
      this.btnSendRegular.Name = "btnSendRegular";
      this.btnSendRegular.Size = new System.Drawing.Size(382, 58);
      this.btnSendRegular.TabIndex = 1;
      this.btnSendRegular.Text = "Send Factura Regular";
      this.btnSendRegular.UseVisualStyleBackColor = true;
      this.btnSendRegular.Click += new System.EventHandler(this.btnSendRegular_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.editMemo);
      this.Name = "MainForm";
      this.Text = "Form1";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.TextBox editMemo;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button btnSendSimplificada;
    private System.Windows.Forms.Button btnSendRegular;
  }
}

