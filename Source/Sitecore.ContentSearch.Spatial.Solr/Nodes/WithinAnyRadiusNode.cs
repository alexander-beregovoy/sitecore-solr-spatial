using System.Collections.Generic;
using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.ContentSearch.Spatial.DataTypes;

namespace Sitecore.ContentSearch.Spatial.Solr.Nodes
{
    public class WithinAnyRadiusNode : QueryNode
    {
        public WithinAnyRadiusNode(QueryNode sourceNode, string field, IDictionary<SpatialPoint, double> points)
        {
            this.SourceNode = sourceNode;
            this.Field = field;
            this.Points = points;
        }

        public QueryNode SourceNode { get; protected set; }
        public string Field { get; protected set; }
        public IDictionary<SpatialPoint, double> Points { get; protected set; }

        public override QueryNodeType NodeType
        {
            get
            {
                return QueryNodeType.Custom;
            }
        }

        public override IEnumerable<QueryNode> SubNodes
        {
            get
            {
                yield return this.SourceNode;
            }
        }

    }
}
