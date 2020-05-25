using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiZhuaChart.NET.Templates
{
    partial class PieChart:IChart
    {
        public string Title { get; set; }
        private string Chart_id { get; }
        private string Series_name;
        private Dictionary<string, int> Data;

        private string FormChartid()
        {
            return Guid.NewGuid().ToString().Split('-')[0];
        }
        
        public PieChart(string series_name, Dictionary<string,int> data, string title)
        {
            this.Chart_id = this.FormChartid();
            this.Series_name = series_name;
            this.Data = data;
            this.Title = title;
        }

    }
}
