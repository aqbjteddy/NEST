﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using Nest.TestData.Domain;

namespace Nest.Tests.Dsl.Json.Facets
{
  [TestFixture]
  public class GeoDistanceFacetJson
  {
    [Test]
    public void TestGeoDistance()
    {
      var s = new SearchDescriptor<ElasticSearchProject>()
        .From(0)
        .Size(10)
        .QueryRawJson(@"{ raw : ""query""}")
        .FacetGeoDistance("geo1", gd => gd
          .OnValueField(f=>f.Origin)
          .PinTo(Lat: 40, Lon: -70)
          .Ranges(
            r=>r.To(10),
            r=>r.From(10).To(20),
            r=>r.From(20).To(100),
            r=>r.From(100)
          )
        );
      var json = ElasticClient.Serialize(s);
      var expected = @"{ from: 0, size: 10, 
          facets :  {
            ""geo1"" :  {
                geo_distance : {
                  ""pin.location"" : ""40, -70"",
                  value_field: ""origin"",
                  ""ranges"" : [
                    { ""to"" : 10 },
                    { ""from"" : 10, ""to"" : 20 },
                    { ""from"" : 20, ""to"" : 100 },
                    { ""from"" : 100 }
                  ]
                } 
            }
          }, query : { raw : ""query""}
      }";
      Assert.True(json.JsonEquals(expected), json);
    }
    [Test]
    public void GeoDistanceUsingHash()
    {
      var s = new SearchDescriptor<ElasticSearchProject>()
        .From(0)
        .Size(10)
        .QueryRawJson(@"{ raw : ""query""}")
        .FacetGeoDistance("geo1", gd => gd
          .OnValueField(f => f.Origin)
          .PinTo("drm3btev3e86")
        );
      var json = ElasticClient.Serialize(s);
      var expected = @"{ from: 0, size: 10, 
          facets :  {
            ""geo1"" :  {
                geo_distance : {
                    ""pin.location"" : ""drm3btev3e86"",
                    value_field: ""origin""
                } 
            }
          }, query : { raw : ""query""}
      }";
      Assert.True(json.JsonEquals(expected), json);
    }
    [Test]
    public void GeoDistanceUsingHashAndOptions()
    {
      var s = new SearchDescriptor<ElasticSearchProject>()
        .From(0)
        .Size(10)
        .QueryRawJson(@"{ raw : ""query""}")
        .FacetGeoDistance("geo1", gd => gd
          .OnValueField(f => f.Origin)
          .PinTo("drm3btev3e86")
          .Unit(GeoUnit.mi)
          .DistanceType(GeoDistance.arc)
        );
      var json = ElasticClient.Serialize(s);
      var expected = @"{ from: 0, size: 10, 
          facets :  {
            ""geo1"" :  {
                geo_distance : {
                    ""pin.location"" : ""drm3btev3e86"",
                    value_field: ""origin"",
                    unit: ""mi"",
                    distance_type: ""arc""
                } 
            }
          }, query : { raw : ""query""}
      }";
      Assert.True(json.JsonEquals(expected), json);
    }
    [Test]
    public void GeoDistanceScript()
    {
      var s = new SearchDescriptor<ElasticSearchProject>()
        .From(0)
        .Size(10)
        .QueryRawJson(@"{ raw : ""query""}")
        .FacetGeoDistance("geo1", gd => gd
          .OnValueScript("doc['num1'].value * factor")
          .Params(p=>p.Add("factor", 5))
          .PinTo(40, -70)
          .Unit(GeoUnit.mi)
          .DistanceType(GeoDistance.arc)
        );
      var json = ElasticClient.Serialize(s);
      var expected = @"{ from: 0, size: 10, 
          facets :  {
            ""geo1"" :  {
                geo_distance : {
                    ""pin.location"" : ""40, -70"",
                    value_script: ""doc['num1'].value * factor"",
                    unit: ""mi"",
                    distance_type: ""arc"",
                    params: { factor: 5 }
                } 
            }
          }, query : { raw : ""query""}
      }";
      Assert.True(json.JsonEquals(expected), json);
    }
  }
}
