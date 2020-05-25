using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiZhuaChart.NET.Templates
{
    public interface IChart
    {
        string Title { get; set; }
        string TransformText();
    }
}
