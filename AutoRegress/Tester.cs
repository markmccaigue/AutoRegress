using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AutoRegress
{
    public class Tester
    {
        private static string GetFilePathForObject<T>(T target)
        {
            return string.Format("{0}{1}{2}{3}{4}{5}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.DirectorySeparatorChar, "AutoRegress", Path.DirectorySeparatorChar, target.GetType().Name, ".json");
        }

        private static string GetResultsForObject<T>(T target)
        {
            var type = target.GetType();

            var methodsAndAccessors = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            //!IsSpecialName excludes property accessors
            var methodsWithoutParamaters = methodsAndAccessors.Where(m => m.GetParameters().Length == 0 && !m.IsSpecialName).OrderBy(m => m.Name);

            var methodsAndResults = new Dictionary<string, object>();
            methodsWithoutParamaters.ToList().ForEach(m =>
            {
                var result = m.Invoke(target, null);
                methodsAndResults.Add(m.Name, result);
            });

            var serialisedResults = JsonConvert.SerializeObject(methodsAndResults);
            return serialisedResults;
        }

        public static void StoreStateForClass<T>(T target)
        {
            var serialisedResults = GetResultsForObject(target);
            File.WriteAllText(GetFilePathForObject(target), serialisedResults);
        }

        public static bool CheckStateForClass<T>(T target)
        {
            var observed = GetResultsForObject(target);
            var actual = File.ReadAllText(GetFilePathForObject(target));
            return observed.Equals(actual);
        }
    }
}
