﻿using System;
using System.Collections.Generic;
using Glav.CacheAdapter.Core;
using Glav.CacheAdapter.Core.DependencyInjection;
using Glav.CacheAdapter.DependencyManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Glav.CacheAdapter.Core.Diagnostics;

namespace Glav.CacheAdapter.Tests
{
    [TestClass]
    public class LoggingTests
    {
        [TestMethod]
        public void ShouldLogInfoMessages()
        {
            AppServices.SetLogger(new TestLogger());
            var provider = AppServices.Cache;

            provider.Add("test1", DateTime.Now.AddMinutes(1), "data");

            Assert.IsTrue(TestLogger.InfoCount > 1,"Expected some informational messages to be logged but they were not");
        }

        [TestMethod]
        public void ShouldLogInfoAndErrorMEssages()
        {
            try {
                var config = new CacheConfig { CacheToUse = "memcached" };
                config.ServerNodes.Add(new Distributed.ServerNode("1.2.3.4", 1));
                AppServices.SetDependencies(new TestLogger(), config );
                var provider = AppServices.Cache;
                provider.Add("test1", DateTime.Now.AddMinutes(1), "data");
            } catch {  }

            Assert.IsTrue(TestLogger.InfoCount > 1, "Expected some informational messages to be logged but they were not");
            Assert.IsTrue(TestLogger.ErrorCount > 1, "Expected some error messages to be logged but they were not");
        }

    }

    public class TestLogger : ILogging
    {
        private static int _errorCount = 0;
        private static int _infoCount = 0;
        public static int ErrorCount { get { return _errorCount; } }
        public static int InfoCount { get { return _infoCount; } }
        public static void ResetCounts()
        {
            _errorCount = 0;
            _infoCount = 0;
        }

        public void WriteErrorMessage(string message)
        {
            _errorCount++;
            throw new NotImplementedException();
        }

        public void WriteException(Exception ex)
        {
            _errorCount++;
        }

        public void WriteInfoMessage(string message)
        {
            _infoCount++;
        }
    }
}
