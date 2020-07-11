using OxyPlot;
using System.Collections.Generic;



namespace WpfApp1
{
    
        public class GraphViewModel
        {
            public string Title { get; set; }
            public List<DataPoint> Points { get; set; } = new List<DataPoint>();
        }
    

}
