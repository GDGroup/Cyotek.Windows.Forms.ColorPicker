using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using CommonMark;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace Cyotek.Windows.Forms.ColorPicker.Demo
{
  // Cyotek Color Picker controls library
  // Copyright � 2013-2015 Cyotek Ltd.
  // http://cyotek.com/blog/tag/colorpicker

  // Licensed under the MIT License. See license.txt for the full text.

  // If you use this code in your applications, donations or attribution are welcome

  internal partial class AboutDialog : BaseForm
  {
    #region Public Constructors

    public AboutDialog()
    {
      this.InitializeComponent();
    }

    #endregion

    #region Class Members

    internal static void ShowAboutDialog()
    {
      using (Form dialog = new AboutDialog())
      {
        dialog.ShowDialog();
      }
    }

    #endregion

    #region Overridden Methods

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      if (!this.DesignMode)
      {
        FileVersionInfo info;
        Assembly assembly;
        string title;

        assembly = typeof(ColorGrid).Assembly;
        info = FileVersionInfo.GetVersionInfo(assembly.Location);
        title = info.ProductName;

        this.Text = string.Format("About {0}", title);
        nameLabel.Text = title;
        versionLabel.Text = string.Format("Version {0}", info.FileVersion);
        copyrightLabel.Text = info.LegalCopyright;

        this.AddReadme("changelog.md");
        this.AddReadme("readme.md");
        this.AddReadme("acknowledgements.md");
        this.AddReadme("license.txt");

        this.LoadDocumentForTab(docsTabControl.SelectedTab);
      }
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);

      if (docsTabControl != null)
      {
        docsTabControl.SetBounds(docsTabControl.Left, docsTabControl.Top, this.ClientSize.Width - (docsTabControl.Left * 2), this.ClientSize.Height - (docsTabControl.Top + footerGroupBox.Height + docsTabControl.Left));
      }
    }

    #endregion

    #region Protected Properties

    protected TabControl TabControl
    {
      get { return docsTabControl; }
    }

    #endregion

    #region Private Members

    private void AddReadme(string fileName)
    {
      this.docsTabControl.TabPages.Add(new TabPage
      {
        UseVisualStyleBackColor = true,
        Padding = new Padding(9),
        ToolTipText = this.GetFullReadmePath(fileName),
        Text = fileName,
        Tag = fileName
      });
    }

    private string GetFullReadmePath(string fileName)
    {
      return Path.GetFullPath(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"), fileName));
    }

    #endregion

    #region Event Handlers

    private void closeButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void docsTabControl_Selecting(object sender, TabControlCancelEventArgs e)
    {
      this.LoadDocumentForTab(e.TabPage);
    }

    private void LoadDocumentForTab(TabPage page)
    {
      if (page != null && page.Controls.Count == 0 && page.Tag != null)
      {
        Control documentView;
        string fullPath;
        string text;

        Cursor.Current = Cursors.WaitCursor;

        Debug.Print("Loading readme: {0}", page.Tag);

        fullPath = this.GetFullReadmePath(page.Tag.ToString());
        text = File.Exists(fullPath) ? File.ReadAllText(fullPath) : string.Format("Cannot find file '{0}'", fullPath);

        if (text.IndexOf('\n') != -1 && text.IndexOf('\r') == -1)
        {
          text = text.Replace("\n", "\r\n");
        }

        switch (Path.GetExtension(fullPath))
        {
          case ".md":
            documentView = new HtmlPanel
            {
              Dock = DockStyle.Fill,
              BaseStylesheet = Properties.Resources.CSS,
              Text = string.Concat("<html><body>", CommonMarkConverter.Convert(text), "</body></html>") // HACK: HTML panel screws up rendering if a <body> tag isn't present
            };
            break;
          default:
            documentView = new TextBox
            {
              ReadOnly = true,
              Multiline = true,
              WordWrap = true,
              ScrollBars = ScrollBars.Vertical,
              Dock = DockStyle.Fill,
              Text = text
            };
            break;
        }

        page.Controls.Add(documentView);

        Cursor.Current = Cursors.Default;
      }
    }

    private void footerGroupBox_Paint(object sender, PaintEventArgs e)
    {
      e.Graphics.DrawLine(SystemPens.ControlDark, 0, 0, footerGroupBox.Width, 0);
      e.Graphics.DrawLine(SystemPens.ControlLightLight, 0, 1, footerGroupBox.Width, 1);
    }

    private void webLinkLabel_Click(object sender, EventArgs e)
    {
      try
      {
        Process.Start(((Control)sender).Text);
      }
      catch (Exception ex)
      {
        MessageBox.Show(string.Format("Unable to start the specified URI.\n\n{0}", ex.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    #endregion
  }
}
