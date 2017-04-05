using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.ContentSearch.Linq.Solr;
using Sitecore.ContentSearch.Spatial.Solr.Nodes;

namespace Sitecore.ContentSearch.Spatial.Solr.Indexing
{
    public class SpatialSolrQueryOptimizer : SolrQueryOptimizer
    {
        protected override QueryNode Visit(QueryNode node, SolrQueryOptimizerState state)
        {
            if (node.NodeType == QueryNodeType.Custom)
            {
                var withinRadiusNode = node as WithinRadiusNode;
                if (withinRadiusNode != null)
                {
                    return VisitWithinRadius(withinRadiusNode, state);
                }

                var withinAnyRadiusNode = node as WithinAnyRadiusNode;
                if (withinAnyRadiusNode != null)
                {
                    return VisitWithinAnyRadius(withinAnyRadiusNode, state);
                }

                var withinBoundsNode = node as WithinBoundsNode;
                if (withinBoundsNode != null)
                {
                    return VisitWithinBounds(withinBoundsNode, state);
                }
            }
           
            return base.Visit(node, state);
        }

        private QueryNode VisitWithinBounds(WithinBoundsNode boundsNode, SolrQueryOptimizerState state)
        {
            return new WithinBoundsNode(Visit(boundsNode.SourceNode, state), boundsNode.Field, boundsNode.LowerLefttLat, boundsNode.LowerLeftLon, boundsNode.UpperRightLat, boundsNode.UpperRightLon, state.Boost);
        }

        private QueryNode VisitWithinRadius(WithinRadiusNode radiusNode, SolrQueryOptimizerState state)
        {
            return new WithinRadiusNode(Visit(radiusNode.SourceNode, state), radiusNode.Field, radiusNode.Lat, radiusNode.Lon, radiusNode.Radius);
        }

        private QueryNode VisitWithinAnyRadius(WithinAnyRadiusNode radiusNode, SolrQueryOptimizerState state)
        {
            return new WithinAnyRadiusNode(Visit(radiusNode.SourceNode, state), radiusNode.Field, radiusNode.Points);
        }
    }
}