﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NUnitTestResultsViewerCode.UI
{
  public partial class CompTestCase : UserControl, ICompResultsUI
  {
    public CompTestCase()
    {
      InitializeComponent();
    }

    public void InitializeData( Items.BaseItem result )
    {
      var res = result as Items.TestCase;
      if( null == res )
      {
        throw new Exception( "Invalid argument type." );
      }

      compTestResult.InitializeData( res );
      txtHistory.Text = res.History;
      txtMessage.Text = res.Message;
      txtStackTrace.Text = res.StackTrace;
      txtOutput.Text = res.Output;
    }
  }
}
