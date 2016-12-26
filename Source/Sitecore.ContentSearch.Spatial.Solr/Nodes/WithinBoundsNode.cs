using System.Collections.Generic;
using Sitecore.ContentSearch.Linq.Nodes;

namespace Sitecore.ContentSearch.Spatial.Solr.Nodes
{
    public class WithinBoundsNode : QueryNode
    {
        private const float DefaultBoost = 1F;
        public QueryNode SourceNode { get; protected set; }
        public override QueryNodeType NodeType => QueryNodeType.Custom;
        public override IEnumerable<QueryNode> SubNodes
        {
            get { yield return SourceNode; }
        }

        public double LowerLefttLat { get; }

        public double LowerLeftLon { get; }

        public double UpperRightLat { get; }

        public double UpperRightLon { get; }

        public string Field { get; private set; }

        public float Boost { get; protected set; }


        public WithinBoundsNode(QueryNode sourceNode, string field, double lowerLefttLat, double lowerLeftLon, double upperRightLat, double upperRightLon)
            : this(sourceNode, field, lowerLefttLat, lowerLeftLon, upperRightLat, upperRightLon, DefaultBoost) { }

        public WithinBoundsNode(QueryNode sourceNode, string field, double lowerLefttLat, double lowerLeftLon, double upperRightLat, double upperRightLon, float boost)
        {
            SourceNode = sourceNode;
            Field = field;
            LowerLefttLat = lowerLefttLat;
            LowerLeftLon = lowerLeftLon;
            UpperRightLat = upperRightLat;
            UpperRightLon = upperRightLon;
            Boost = boost;
        }
    }
}
