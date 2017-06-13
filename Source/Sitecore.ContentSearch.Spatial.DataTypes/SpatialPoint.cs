using System;
using System.Diagnostics;

namespace Sitecore.ContentSearch.Spatial.DataTypes
{
    [Serializable]
    [DebuggerDisplay("{Lat},{Lon}")]
    public class SpatialPoint
    {
        public SpatialPoint()
        {
            
        }

        public SpatialPoint(string value)
        {
            if (value == null) throw new ArgumentNullException("value");
            var tokens = value.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 2) throw  new ArgumentException("incorrect spatial point format. Value must be supplied in the following format lat,lon");
            var strLat = tokens[0].Trim();
            var strLon = tokens[1].Trim();
            Lat = double.Parse(strLat);
            Lon = double.Parse(strLon);
        }

        public double Lat { get; set; }

        public double Lon { get; set; }

        public override string ToString()
        {
            return string.Format("{0},{1}", Lat, Lon);
        }
    }
}
