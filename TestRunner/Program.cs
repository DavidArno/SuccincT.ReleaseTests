using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SuccincT.Functional;
using SuccincT.JSON;
using SuccincT.Options;
using SuccincT.Parsers;
using SuccincT.Unions;
using static System.Console;
using static System.Environment;
using static Newtonsoft.Json.JsonConvert;

namespace FRamework45.TestRunner
{
    internal static class Program
    {
        private static string Output = "";

        private static void Main()
        {
            TestOptionsSome().Into(ReportResult);
            TestOptionsNone().Into(ReportResult);
            TestCons().Into(ReportResult);
            TestParsers().Into(ReportResult);
            TestUnionPatternMatcher().Into(ReportResult);
            TestJsonOptionConverter().Into(ReportResult);
            TestJsonUnionConverter().Into(ReportResult);
            TestJsonContractResolver().Into(ReportResult);
            Write($"[{Output}]");

            Exit(Output.Contains("!") ? 1 : 0);
        }

        private static void ReportResult(bool result) => Output += result ? "." : "!";

        private static bool TestOptionsSome() => Option<int>.Some(1) is var result && result.HasValue;

        private static bool TestOptionsNone() => Option<int>.None() is var result && !result.HasValue;

        private static bool TestCons()
        {
            var list = new[] { 1, 2, 3 };
            var (h1, (h2, t)) = list;
            return h1 == 1 && h2 == 2 && t.First() == 3;
        }

        private static bool TestParsers() => "123".TryParseInt().Value == 123;

        private static bool TestUnionPatternMatcher()
        {
            var union = new Union<int, string>(42);
            return union.Match<bool>()
                        .CaseOf<int>()
                        .Where(i => i == 42)
                        .Do(true)
                        .CaseOf<int>()
                        .Do(false)
                        .Else(false)
                        .Result();
        }

        private static bool TestJsonOptionConverter()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new OptionConverter());
            var option = Option<int>.Some(1);
            var json = SerializeObject(option, settings);
            var newOption = DeserializeObject<Option<int>>(json, settings);

            return newOption.HasValue && newOption.Value == 1;
        }

        private static bool TestJsonUnionConverter()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new UnionOf2Converter());
            var union = new Union<List<int>, string>(new List<int> { 1, 2 });
            var json = SerializeObject(union, settings);
            var newUnion = DeserializeObject<Union<List<int>, string>>(json, settings);

            return newUnion.Case1.Count == 2 &&
                Variant.Case1 == newUnion.Case &&
                newUnion.Case1[0] == 1 &&
                newUnion.Case1[1] == 2;
        }

        private static bool TestJsonContractResolver()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = SuccinctContractResolver.Instance
            };

            var option1 = Option<string>.Some("a");
            var option2 = Option<string>.None();
            var list = new List<Option<string>> { option1, option2 };
            var json = SerializeObject(list, settings);
            var newList = DeserializeObject<List<Option<string>>>(json, settings);

            return newList.Count == 2 && newList[0].HasValue && !newList[1].HasValue && newList[0].Value == "a";
        }
    }
}