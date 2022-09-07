using GeoChatter.Core.Helpers;
using GeoChatter.Core.Model;
using GeoChatter.Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace GeoChatter.Test
{
    public class BorderTest
    {
        [SetUp]
        public void Setup()
        {
            BorderHelper.Initialize();
        }

        [Test]
        public void GetRandomCountry()
        {
            for (int i = 0; i < 1e+4; i++)
            {
                GeoJson g = BorderHelper.GetRandomCountry();

                Assert.IsNotNull(g);
                Assert.IsNotNull(g.features);

                Assert.IsFalse(string.IsNullOrEmpty(g.type), $"{g} {nameof(g.type)} was {g.type}");
                Assert.AreNotEqual(0D, g.features.Count);
            }
        }

        [Test]
        public void GetRandomFeature()
        {
            Dictionary<GeoJson, int> hits = new();
            for (int i = 0; i < 1e+4; i++)
            {
                GeoJson g = BorderHelper.GetRandomCountry();
                if (hits.ContainsKey(g))
                {
                    hits[g]++;
                }
                else
                {
                    hits.Add(g, 1);
                }

                Feature f = BorderHelper.GetRandomFeature(g);

                Assert.IsNotNull(f);
                Assert.IsNotNull(f.coords);
                Assert.IsNotNull(f.properties);
                Assert.IsNotNull(f.geometry);
                Assert.IsNull(f.geometry.coordinates);

                Assert.IsFalse(string.IsNullOrEmpty(f.properties.shapeType), $"{f} {nameof(f.properties.shapeType)} was {f.properties.shapeType}");
                Assert.IsFalse(string.IsNullOrEmpty(f.properties.shapeName), $"{f} {nameof(f.properties.shapeName)} was {f.properties.shapeName}");
                Assert.IsFalse(string.IsNullOrEmpty(f.properties.shapeISO), $"{f} {nameof(f.properties.shapeISO)} was {f.properties.shapeISO}");
                Assert.IsFalse(string.IsNullOrEmpty(f.properties.shapeGroup), $"{f} {nameof(f.properties.shapeGroup)} was {f.properties.shapeGroup}");

                Assert.IsFalse(string.IsNullOrEmpty(f.type), $"{f} {nameof(f.type)} was {f.type}");
                Assert.IsTrue(f.coords is Polygon or MultiPolygon, $"{f} {nameof(f.coords)} was {f.coords?.GetType()}");
            }
            Assert.AreEqual(BorderHelper.BorderData.Length, hits.Count);
            hits.Clear();
        }

        [Test]
        public void GetRandomPointAndFeatureCloseOrWithinAPolygon()
        {
            for (int i = 0; i < 1e+4; i++)
            {
                Coordinates c = BorderHelper.GetRandomPointCloseOrWithinAPolygon(out Feature feature);

                Assert.IsNotNull(c);
                Assert.IsNotNull(feature);
                Assert.AreNotEqual(0D, c.Longitude);
                Assert.AreNotEqual(0D, c.Latitude);
            }
        }

        [Test]
        public void GetRandomPointCloseOrWithinAPolygon()
        {
            for (int i = 0; i < 1e+4; i++)
            {
                Coordinates c = BorderHelper.GetRandomPointCloseOrWithinAPolygon();

                Assert.IsNotNull(c);
                Assert.AreNotEqual(0D, c.Longitude);
                Assert.AreNotEqual(0D, c.Latitude);
            }
        }
    }
}