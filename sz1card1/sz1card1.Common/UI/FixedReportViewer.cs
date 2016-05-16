using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.IO;

/// <summary>
///修复Chrome下ReportViewer CPU占用率100%的问题
/// </summary>
namespace sz1card1.Common.UI
{
    public class FixedReportViewer : Microsoft.Reporting.WebForms.ReportViewer
    {
        protected override void Render(HtmlTextWriter writer)
        {
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter tmpWriter = new HtmlTextWriter(sw);
                base.Render(tmpWriter);
                string val = sw.ToString();
                val = val.Replace(@"!= &#39;javascript:\&#39;\&#39;&#39;", @"!= &#39;javascript:\&#39;\&#39;&#39; && false");
                writer.Write(val);
            }
        }
    }
}
