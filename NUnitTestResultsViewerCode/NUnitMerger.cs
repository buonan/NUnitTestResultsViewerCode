using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace NUnitTestResultsViewerCode
{
    public static class NUnitMerger
    {
        public static bool MergeFiles(IEnumerable<string> files, string output)
        {
            XElement environment = null;
            XElement culture = null;
            var suites = new List<XElement>();

            // Assuming only one assembly
            string nameAssembly = string.Empty;
            string dateAssembly = string.Empty;
            bool finalSuccess = true;
            string finalResult = string.Empty;
            double totalTime = 0;
            int total = 0, errors = 0, failures = 0, notrun = 0, inconclusive = 0, ignored = 0, skipped = 0, invalid = 0;
            foreach (var file in files)
            {
                var doc = XDocument.Load(file);
                var tr = doc.Element("test-results");

                if (environment == null)
                    environment = tr.Element("environment");
                if (culture == null)
                    culture = tr.Element("culture-info");

                nameAssembly = tr.Attribute("name").Value;
                dateAssembly = tr.Attribute("date").Value;
                total += Convert.ToInt32(tr.Attribute("total").Value);
                errors += Convert.ToInt32(tr.Attribute("errors").Value);
                failures += Convert.ToInt32(tr.Attribute("failures").Value);
                notrun += Convert.ToInt32(tr.Attribute("not-run").Value);
                inconclusive += Convert.ToInt32(tr.Attribute("inconclusive").Value);
                ignored += Convert.ToInt32(tr.Attribute("ignored").Value);
                skipped += Convert.ToInt32(tr.Attribute("skipped").Value);
                invalid += Convert.ToInt32(tr.Attribute("invalid").Value);

                var ts = tr.Element("test-suite");
                string assembly = ts.Attribute("name").Value;
                string result = ts.Attribute("result").Value;
                ts.Attribute("name").SetValue(dateAssembly);
                XAttribute tot = new XAttribute("total", tr.Attribute("total").Value);
                ts.Add(tot);
                XAttribute err = new XAttribute("errors", tr.Attribute("errors").Value);
                ts.Add(err);
                XAttribute fai = new XAttribute("failures", tr.Attribute("failures").Value);
                ts.Add(fai);
                XAttribute not = new XAttribute("not-run", tr.Attribute("not-run").Value);
                ts.Add(not);

                if (!Convert.ToBoolean(ts.Attribute("success").Value))
                    finalSuccess = false;

                totalTime += Convert.ToDouble(ts.Attribute("time").Value);

                if (finalResult != "Failure" && (String.IsNullOrEmpty(finalResult) || result == "Failure" || finalResult == "Success"))
                    finalResult = result;

                suites.Add(ts);
            }

            if (String.IsNullOrEmpty(finalResult))
            {
                finalSuccess = false;
                finalResult = "Inconclusive";
            }

            var project = XElement.Parse(String.Format("<test-suite type=\"Test Project\" name=\"{0}\" executed=\"True\" result=\"{1}\" success=\"{2}\" time=\"{3}\" asserts=\"0\" />", nameAssembly, finalResult, finalSuccess ? "True" : "False", totalTime));
            var results = XElement.Parse("<results/>");
            results.Add(suites.ToArray());
            project.Add(results);

            var now = DateTime.Now;
            var trfinal = XElement.Parse(String.Format("<test-results name=\"Merged results\" total=\"{0}\" errors=\"{1}\" failures=\"{2}\" not-run=\"{3}\" inconclusive=\"{4}\" ignored=\"{5}\" skipped=\"{6}\" invalid=\"{7}\" date=\"{8}\" time=\"{9}\" />", total, errors, failures, notrun, inconclusive, ignored, skipped, invalid, now.ToString("yyyy-MM-dd"), now.ToString("HH:mm:ss")));
            trfinal.Add(new[] { environment, culture, project });
            trfinal.Save(output);

            return finalSuccess;
        }

        public static void SaveResult(string mergedData)
        {
            System.IO.File.WriteAllText(@"MergedResults.xml", mergedData);
        }
    }
}

