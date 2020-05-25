using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DiZhuaChart.NET.Templates
{
    partial class ChartPage
    {
        private List<string> charts { get; }

        public ChartPage()
        {
            this.charts = new List<string>();
        }

        public ChartPage(List<string> charts)
        {
            this.charts = charts;
        }

        public void Render(string htmlPath=null)
        {
            if(htmlPath is null)
            {
                File.WriteAllText("chart.html", this.TransformText());
            }
            else
            {
                File.WriteAllText(htmlPath, this.TransformText());
            }
        }

        public ChartPage Add(IChart chart)
        {
            this.charts.Add(chart.TransformText());
            return this;
        }
    }
}
