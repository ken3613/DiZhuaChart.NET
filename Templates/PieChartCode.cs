using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiZhuaChart.NET.Templates
{
    partial class PieChart
    {
        public string title { get; set; }
        private string chart_id;
        private string series_name;
        private Dictionary<string, int> data;

        private string FormChartid()
        {
            string str = "abcdefghijklmnopqrstuvwxyz1234567890";
            string id = "";
            Random random = new Random();
            for(int i = 0; i < 32; i++)
            {
                int ir = random.Next(0, 35);
                id += str[ir];
            }
            return id;
        }
        
        public PieChart(string Series_name, Dictionary<string,int> Data, string title)
        {
            this.chart_id = this.FormChartid();
            this.series_name = Series_name;
            this.data = Data;
            this.title = title;
        }

    }
}
