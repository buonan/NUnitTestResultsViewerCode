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

    public string Output
    {
        get
        {
            return readDescenadantNodeValue( "output" );
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
            string build_date = n.Ancestors( "test-suite" )
                .Where(x => (string) x.Attribute("type") == "Assembly")
                .Select(x => (string) x.Attribute("name"))
                .FirstOrDefault();
            if (n.Attribute("name").Value == testcaseName) {
                    if (n.Attribute("runstate").Value == "Runnable") {
                        history.Append(string.Format("build '{0}' executed = '{1}' result = '{2}' passed = '{3}' time = '{4}' asserts = '{5}'\n", 
                            build_date,
                            n.Attribute("runstate").Value,
                            n.Attribute("result").Value,
                            n.Attribute("passed")!=null?n.Attribute("passed").Value:"NA",
                            n.Attribute("duration").Value,
                            n.Attribute("asserts").Value??"NA"));
                    }
                    else {
                        history.Append(string.Format("executed = '{0}' result = '{1}''\n", 
                            n.Attribute("runstate").Value,
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
