using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Drawing;
using NUnitTestResultsViewerCode.Properties;

namespace NUnitTestResultsViewerCode.Items
{
  /// <summary>
  /// Represent Test Results base class.
  /// </summary>
  abstract class TestResult : BaseItem
  {
    protected bool _typeAssembly = false;

    /// <summary>
    /// Initializes class.
    /// </summary>
    /// <param name="element"></param>
    protected TestResult( XElement element )
      : base( element )
    {
        XAttribute t = element.Attribute("type");
        if (t != null) {
            _typeAssembly = (t.Value == "Assembly" ? true : false);
        }
    }

    /// <summary>
    /// Is test has been executed.
    /// </summary>
    public bool IsExecuted
    {
      get
      {
        return (bool)(readAttribute<string>( "runstate" )=="Runnable");//return (bool)readAttribute<bool>( "executed" );
      }
    }

    /// <summary>
    /// Test Result.
    /// </summary>
    public TestResultsEnum TestResultValue
    {
      get
      {
        return (TestResultsEnum)readAttribute<TestResultsEnum>( "result" );
        //return TestResultsEnum.Inconclusive;
      }
    }

    public string Errors
    {
        get
        {
            if( _typeAssembly )
            {
                try {
                    string t = (string)readAttribute<string>( "errors" );
                    return t;
                }
                catch (Exception e) {
                    return null;
                }
            }
            return null;   
        }
    }

    public string Failures
    {
        get
        {
            if( _typeAssembly )
            {
                try {
                    string t = (string)readAttribute<string>( "failed" );
                    return t;
                }
                catch (Exception e) {
                    return null;
                }
            }
            return null;        
        }
    }

    public string Passed
    {
        get
        {
            if( _typeAssembly )
            {
                try {
                    string t = (string)readAttribute<string>( "passed" );
                    return t;
                }
                catch (Exception e) {
                    return null;
                }
            }
            return null;        
        }
    }

    public string Ignored
    {
        get
        {
            if( _typeAssembly )
            {
                try {
                    string t = (string)readAttribute<string>( "skipped" );
                    return t;
                }
                catch (Exception e) {
                    return null;
                }
            }
            return null;            
        }
    }

    public string Total
    {
        get
        {
            if( _typeAssembly )
            {
                try {
                    string t = (string)readAttribute<string>( "total" );
                    return t;
                }
                catch (Exception e) {
                    return null;
                }
            }
            return null;      
        }
    }

    public string PassRate
    {
        get
        {
            if( _typeAssembly )
            {
                try {
                        double total = double.Parse(this.Total ?? "0");
                        double passed = double.Parse(this.Passed ?? "0");
                        double errors = double.Parse(this.Errors ?? "0");
                        double failures = double.Parse(this.Failures ?? "0");
                        double ignored = double.Parse(this.Ignored ?? "0");
                        double total_failed = errors + failures;
                        //double pass = ((total - total_failed) / total) * 100;
                        double pass = ((passed) / total) * 100;
                        string val = string.Empty;
                        if (!double.IsNaN(pass)) {
                            val = string.Format("{0} %", pass.ToString("#.##"));
                        }
                        return val;
                }
                catch (Exception e) {
                    return null;
                }
            }
            return null;      
        }
    }

    /// <summary>
    /// Exucution time.
    /// </summary>
    public string Time
    {
      get
      {
        if( !IsExecuted )
        {
          return null;
        }
        return (string)readAttribute<string>( "time" );
      }
    }

    /// <summary>
    /// Count of asserts.
    /// </summary>
    public int? Asserts
    {
      get
      {
        if( !IsExecuted )
        {
          return null;
        }
        return (int)readAttribute<int>( "asserts" );
      }
    }

    /// <summary>
    /// Initializes with default value.
    /// </summary>
    protected override void intialize()
    {
      base.intialize();

      switch( TestResultValue )
      {
        case TestResultsEnum.Success:
        case TestResultsEnum.Passed:
          ImageKey = Defines.IMAGE_KEY_SUCCESS;
          SelectedImageKey = ImageKey;
          break;
        case TestResultsEnum.Failure:
        case TestResultsEnum.Failed:
        case TestResultsEnum.Error:
        case TestResultsEnum.Invalid:
          ImageKey = Defines.IMAGE_KEY_FAILURE;
          SelectedImageKey = ImageKey;
          break;
        case TestResultsEnum.Skipped:
          ImageKey = Defines.IMAGE_KEY_SKIPPED;
          SelectedImageKey = ImageKey;
          break;
        case TestResultsEnum.Ignored:
          ImageKey = Defines.IMAGE_KEY_IGNORED;
          SelectedImageKey = ImageKey;
          break;
        case TestResultsEnum.Inconclusive:
          ImageKey = Defines.IMAGE_KEY_INCONCLUSIVE;
          SelectedImageKey = ImageKey;
          break;

      }
    }
  }
}
