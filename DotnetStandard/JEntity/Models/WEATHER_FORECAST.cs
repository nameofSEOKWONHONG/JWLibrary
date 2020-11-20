namespace ServiceExample.Data {

    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class WEATHER_FORECAST {
        public int ID { get; set; }

        public DateTime? DATE { get; set; }

        public int? TEMPERATURE_C { get; set; }

        [NotMapped]
        public int? TEMPERATURE_F { get; set; }

        public string SUMMARY { get; set; }
    }
}