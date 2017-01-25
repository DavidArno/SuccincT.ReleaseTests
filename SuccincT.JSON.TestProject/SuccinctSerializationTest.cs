using Newtonsoft.Json;
using NUnit.Framework;
using SuccincT.JSON;
using SuccincT.Options;
using SuccincT.Unions;
using static Newtonsoft.Json.JsonConvert;
using static NUnit.Framework.Assert;
using static SuccincT.Unions.None;

namespace SuccincT.JSONTests
{
    [TestFixture]
    public class SuccinctSerializationTest
    {
        [Test]
        public void SuccinctTypes_CanBeJsonSerialized()
        {
            DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = SuccinctContractResolver.Instance
            };

            var value = new TestCollection
            {
                Value1 = Option<int>.Some(1),
                Value2 = none,
                Value3 = new Union<int, string>("a")
            };
            var json = SerializeObject(value);
            var newValue = DeserializeObject<TestCollection>(json);

            AreEqual(value.Value1, newValue.Value1);
            AreEqual(value.Value2, newValue.Value2);
            AreEqual(value.Value3, newValue.Value3);
        }

        private class TestCollection
        {
            public Option<int> Value1 { get; set; }
            public None Value2 { get; set; }
            public Union<int, string> Value3 { get; set; }
        }
    }
}
