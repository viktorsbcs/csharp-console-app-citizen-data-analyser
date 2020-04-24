using System;
using System.Collections.Generic;
using System.Text;


namespace declared_persons_analyser.Models
{
    public class OData
    {
        public string Metadata { get; set; }
        public List<Value> Value { get; set; }
    }

    public class Value
    {
        public Int32 id { get; set; }
        public Int32 year { get; set; }
        public Int32 month { get; set; }
        public Int32 day { get; set; }
        public decimal value { get; set; }
        public Int32 district_id { get; set; }
        public string district_name { get; set; }
    }
}
