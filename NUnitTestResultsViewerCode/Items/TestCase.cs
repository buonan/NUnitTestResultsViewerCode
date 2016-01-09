using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NUnitTestResultsViewerCode.Items
{
  class TestCase : TestResult
  {
    /// <summary>
    /// Initializes TestCase class.
    /// </summary>
    /// <param name="element"></param>
    private TestCase( XElement element )
      : base( element )
    {
    }

    /// <summary>
    /// Load test case.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static TestCase Load( XElement element )
    {
      return new TestCase( element );
    }

    public string History
    {
        get
        {
           XAttribute x = _element.Attribute("name");
            return findAllTestResults( x.Value ?? null );
        }
    }

    /// <summary>
    /// Test Case output message.
    /// </summary>
    public string Message
    {
      get
      {
        return readDescenadantNodeValue( "message" );
      }
    }

    /// <summary>
    /// Message stack trace.
    /// </summary>
    public string StackTrace
    {
      get
      {
        return readDescenadantNodeValue( "stack-trace" );
      }
    }

    private string readDescenadantNodeValue( string nodename )
    {
      var node = _element.Descendants( nodename ).FirstOrDefault();
      if( null == node )
      {
        return string.Empty;
      }
      return node.Value;
    }

    private string findAllTestResults( string testcaseName )
    {
        XElement root = null;
        XElement iter = null;
        StringBuilder history = new StringBuilder();
        if (testcaseName == null)
            return string.Empty;
        iter = _element.Parent;            
        do {
            iter = iter.Parent;
        } while (iter.Parent != null);
        root = iter;
        IEnumerable<XElement> nodes = root.Descendants( "test-case" );
        foreach ( XElement n in nodes )
        {
            if (n.Attribute("name").Value == testcaseName) {
                    if (n.Attribute("executed").Value == "True") {
                        history.Append(string.Format("executed = '{0}' result = '{1}' success = '{2}' time = '{3}' asserts = '{4}'\n", 
                            n.Attribute("executed").Value,
                            n.Attribute("result").Value,
                            n.Attribute("success").Value,
                            n.Attribute("time").Value,
                            n.Attribute("asserts").Value));
                    }
                    else {
                        history.Append(string.Format("executed = '{0}' result = '{1}''\n", 
                            n.Attribute("executed").Value,
                            n.Attribute("result").Value));
                    }
                    
            }
        }
        return history.ToString();
    }

    /// <summary>
    /// Throw an InvalidOperationException. Test Cases can not have child elements.
    /// </summary>
    /// <param name="item"></param>
    public override void Add( BaseItem item )
    {
      throw new InvalidOperationException( "Test Cases could not contains child elements." );
    }
  }
}
