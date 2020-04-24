using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;


namespace declared_persons_analyser.Models
{
    public class DataOutput
    {
        public List<Data> data { get; set; }
        public Summary summary { get; set; }

        public DataOutput()
        {
            this.data = new List<Data>();
            this.summary = new Summary();
        }

    }

    public class Data
    {
        //public int id { get; set; }
        public string district_name { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public decimal value { get; set; }

        //public int district_id { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal change { get; set; }
    }

    public class Summary
    {
        public decimal max { get; set; }
        public decimal min { get; set; }
        public decimal average { get; set; }
        public MaxDrop max_drop { get; set; }
        public MaxIncrease max_increase { get; set; }

        public Summary()
        {
            this.max_drop = new MaxDrop();
            this.max_increase = new MaxIncrease();
        }

    }

    public class MaxDrop
    {
        public decimal value { get; set; }
        public string group { get; set; }
        public string disctrict_name { get; set; }

    }

    public class MaxIncrease
    {
        public decimal value { get; set; }
        public string group { get; set; }
        public string disctrict_name { get; set; }
    }
}
