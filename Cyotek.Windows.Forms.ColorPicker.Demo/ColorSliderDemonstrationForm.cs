﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Cyotek.Windows.Forms.ColorPicker.Demo
{
  // Cyotek Color Picker controls library
  // Copyright © 2013-2015 Cyotek Ltd.
  // http://cyotek.com/blog/tag/colorpicker

  // Licensed under the MIT License. See license.txt for the full text.

  // If you use this code in your applications, donations or attribution are welcome

  internal partial class ColorSliderDemonstrationForm : BaseForm
  {
    #region Public Constructors

    public ColorSliderDemonstrationForm()
    {
      InitializeComponent();
    }

    #endregion

    #region Event Handlers

    private void GotFocusHandler(object sender, EventArgs e)
    {
      propertyGrid.SelectedObject = sender;
    }

    private void ValueChangedHandler(object sender, EventArgs e)
    {
      eventsListBox.AddEvent((Control)sender, "ValueChanged", new Dictionary<string, object>
                                                              {
                                                                {
                                                                  "Value", ((ColorSlider)sender).Value
                                                                }
                                                              });
    }

    private void closeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    #endregion
  }
}
