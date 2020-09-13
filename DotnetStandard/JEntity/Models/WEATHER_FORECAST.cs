namespace JAction.Data {

    using System;

    public partial class WEATHER_FORECAST {
        public int ID { get; set; }

        public DateTime? DATE { get; set; }

        public int? TEMPERATURE_C { get; set; }

        public int? TEMPERATURE_F { get; set; }

        public string SUMMARY { get; set; }
    }
}