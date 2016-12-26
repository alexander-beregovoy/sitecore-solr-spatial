using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.ContentSearch.Linq.Solr;
using Sitecore.ContentSearch.Spatial.Solr.Nodes;
using SolrNet;

namespace Sitecore.ContentSearch.Spatial.Solr.Indexing
{
    public class SolrSpatialQueryMapper: SolrQueryMapper
    {
        public SolrSpatialQueryMapper(SolrIndexParameters parameters) : base(parameters)
        {
        }

        protected override AbstractSolrQuery Visit(QueryNode node, SolrQueryMapperState state)
        {
            if (node.NodeType == QueryNodeType.Custom)
            {
                var withinRadiusNode = node as WithinRadiusNode;
                if (withinRadiusNode != null)
                {
                    return VisitWithinRadius(withinRadiusNode, state);
                }

                var withinBoundsNode = node as WithinBoundsNode;
                if (withinBoundsNode != null)
                {
                    return VisitWithinBound(withinBoundsNode, state);
                }
            }
            return base.Visit(node, state);
        }

        private AbstractSolrQuery VisitWithinBound(WithinBoundsNode boundsNode, SolrQueryMapperState state)
        {
            var orignialQuery = this.Visit(boundsNode.SourceNode, state);
            var spatialQuery = new SolrQuery($"{boundsNode.Field}:[{boundsNode.LowerLefttLat},{boundsNode.LowerLeftLon} TO {boundsNode.UpperRightLat},{boundsNode.UpperRightLon}]");

            if (state.FilterQuery == null)
            {
                state.FilterQuery = spatialQuery;
            }
            else
            {
                state.FilterQuery = state.FilterQuery && spatialQuery;
            }

            return orignialQuery;
        }

        protected virtual AbstractSolrQuery VisitWithinRadius(WithinRadiusNode radiusNode, SolrQueryMapper.SolrQueryMapperState state)
        {
            var orignialQuery = this.Visit(radiusNode.SourceNode, state);
            var spatialQuery = new SolrQuery(string.Format("{{!geofilt pt={0},{1} sfield={2} d={3} score=distance}}", radiusNode.Lat, radiusNode.Lon, radiusNode.Field, (int)radiusNode.Radius));
            var combinedQuery = orignialQuery && spatialQuery;
            return combinedQuery;
        }
    }


}