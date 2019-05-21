using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ATDD.TestScriptor
{
    public static class ExtensionMethods
    {
        public static void AddRange<T>(this Collection<T> collection, IEnumerable<T> items) =>
            items.ForEach(i => collection.Add(i));

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }

            return items;
        }

        public static string Join(this IEnumerable<string> items, string separator) =>
            string.Join(separator, items);

        public static void WriteLineIf(this IndentedTextWriter writer, bool condition, string line)
        {
            if (condition)
                writer.WriteLine(line);
        }

        public static void WriteLines(this IndentedTextWriter writer, IEnumerable<string> lines) =>
            lines.ForEach(l => writer.WriteLine(l));
    }
}