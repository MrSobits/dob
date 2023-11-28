namespace Bars.B4.Modules.Analytics.Reports.Generators
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Application;
    using B4.Utils;
    using B4.Utils.Annotations;
    using Logging;
    using Stimulsoft.Report;
    using MD5 = System.Security.Cryptography.MD5;

    internal static class StimulReportAssemblyCache
    {
        private static readonly object SyncRoot = new object();

        private static readonly Dictionary<string, Assembly> Storage = new Dictionary<string, Assembly>();

        private static ILogManager Log
        {
            get { return ApplicationContext.Current.Container.Resolve<ILogManager>(); }
        }

        public static bool Contains(Stream stream, out string hash)
        {
            ArgumentChecker.NotNull(stream, "stream");

            hash = ComputeHash(stream);

            return Storage.ContainsKey(hash);
        }

        public static Assembly Get(string hash)
        {
            ArgumentChecker.NotNull(hash, "hash");

            if (!Storage.ContainsKey(hash))
            {
                throw new ArgumentOutOfRangeException(string.Format("The given key {0} does not present in dictionary", hash));
            }

            return Storage[hash];
        }

        public static void Add(Stream stream)
        {
            ArgumentChecker.NotNull(stream, "stream");

            var hash = ComputeHash(stream);

            try
            {
                if (!Storage.ContainsKey(hash))
                {
                    lock (SyncRoot)
                    {
                        if (!Storage.ContainsKey(hash))
                        {
                            using (var report = new StiReport())
                            {
                                report.Load(stream);

                                var fileName = Path.GetTempFileName();

                                var compilerResults = StiReport.CompileReportsToAssembly(fileName, new[] { report }, StiReportLanguageType.CSharp);

                                if (compilerResults.Errors.Count > 0) throw new Exception(compilerResults.Errors[0].ErrorText);

                                Storage[hash] = compilerResults.CompiledAssembly;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                Storage.Remove(hash);
                throw;
            }

            Log.Debug("Добавлена новая сборка в кеш сборок отчетов");
        }

        public static void Clear()
        {
            Storage.Clear();
        }

        private static string ComputeHash(Stream stream)
        {
            using (var algo = MD5.Create())
            {
                var hash = algo.ComputeHash(stream).AsBase64String();

                stream.Seek(0, SeekOrigin.Begin);

                return hash;
            }
        }
    }
}