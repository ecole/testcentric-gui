// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters
{
    using Model;

    [TestFixture]
    public class ResultSummaryReporterTests
    {
        [Test]
        public void ReportAlwaysStartsWithOveralResult()
        {
            var summary = new ResultSummary { OverallResult = "Passed" };

            var report = ResultSummaryReporter.WriteSummaryReport(summary);

            Assert.That(report, Does.StartWith("Overall result: Passed"));
        }

        [Test]
        public void ReportContainsAllCountsOfEachTestResult()
        {
            var summary = new ResultSummary
            {
                PassCount = 1,
                FailureCount = 2,
                WarningCount = 3,
                InconclusiveCount = 4,
                SkipCount = 5,
                TestCount = 15,
                OverallResult = String.Empty,
            };

            var report = ResultSummaryReporter.WriteSummaryReport(summary);

            Assert.That(report, Does.Contain("Test Count: 15"));
            Assert.That(report, Does.Contain("Passed: 1"));
            Assert.That(report, Does.Contain("Failed: 2"));
            Assert.That(report, Does.Contain("Warnings: 3"));
            Assert.That(report, Does.Contain("Inconclusive: 4"));
            Assert.That(report, Does.Contain("Skipped: 5"));
        }

        [Test]
        public void ReportDoesNotContainExtendedFailureInformationWhenNoTestsFailed()
        {
            var summary = new ResultSummary
            {
                FailureCount = 0,
                ErrorCount = 0,
                InvalidCount = 0,
                OverallResult = String.Empty
            };

            var report = ResultSummaryReporter.WriteSummaryReport(summary);
            Assert.That(report, Does.Not.Contain("Failed Tests - Failures:"));
            Assert.That(report, Does.Not.Contain("Errors:"));
            Assert.That(report, Does.Not.Contain("Invalid:"));
        }

        [TestCase(1, 0, 0)]
        [TestCase(0, 1, 0)]
        [TestCase(0, 0, 1)]
        public void ReportContainsExtendedFailureInformationWhenItHasFailures(int failureCount, int errorCount, int invalidCount)
        {
            var summary = new ResultSummary
            {
                FailureCount = failureCount,
                ErrorCount = errorCount,
                InvalidCount = invalidCount,
                OverallResult = String.Empty
            };

            var report = ResultSummaryReporter.WriteSummaryReport(summary);

            Assert.That(report, Does.Contain("Failed Tests - Failures: " + failureCount));
            Assert.That(report, Does.Contain("Errors: " + errorCount));
            Assert.That(report, Does.Contain("Invalid: " + invalidCount));
        }

        [Test]
        public void ReportDoesNotContainExtendedSkipInformationWhenNoTestsAreSkipped()
        {
            var summary = new ResultSummary
            {
                IgnoreCount = 0,
                ExplicitCount = 0,
                SkipCount = 0,
                OverallResult = String.Empty
            };

            var report = ResultSummaryReporter.WriteSummaryReport(summary);

            Assert.That(report, Does.Not.Contain("Skipped Tests - Ignored:"));
            Assert.That(report, Does.Not.Contain("Explicit:"));
            Assert.That(report, Does.Not.Contain("Other:"));
        }

        [TestCase(1, 0, 0)]
        [TestCase(0, 1, 0)]
        [TestCase(0, 0, 1)]
        public void ReportContainsExtendedSkipInformationWhenItHasSkips(int ignoreCount, int explicitCount, int skipCount)
        {
            var summary = new ResultSummary
            {
                IgnoreCount = ignoreCount,
                ExplicitCount = explicitCount,
                SkipCount = skipCount,
                OverallResult = String.Empty
            };

            var report = ResultSummaryReporter.WriteSummaryReport(summary);

            Assert.That(report, Does.Contain("Skipped Tests - Ignored: " + ignoreCount));
            Assert.That(report, Does.Contain("Explicit: " + explicitCount));
            Assert.That(report, Does.Contain("Other: " + skipCount));
        }

        [Test]
        public void ReportContainsStartAndEndDate()
        {
            var summary = new ResultSummary
            {
                StartTime = new DateTime(2015, 10, 21, 7, 28, 30),
                EndTime = new DateTime(2015, 10, 21, 16, 20, 45),
                OverallResult = String.Empty
            };

            var report = ResultSummaryReporter.WriteSummaryReport(summary);

            Assert.That(report, Does.Contain("Start time: 2015-10-21 07:28:30Z"));
            Assert.That(report, Does.Contain("End time: 2015-10-21 16:20:45Z"));
        }

        [Test, SetUICulture("de-DE")]
        public void ReportWithDifferentUICultureFormatsNumbersAccordingToUICulture()
        {
            var summary = new ResultSummary
            {
                PassCount = 10000,
                WarningCount = 10000,
                InconclusiveCount = 10000,
                SkipCount = 10000,
                TestCount = 10000,
                FailureCount = 10000,
                ErrorCount = 10000,
                InvalidCount = 10000,
                IgnoreCount = 10000,
                ExplicitCount = 10000,
                Duration = 123.456789,
                OverallResult = String.Empty
            };

            var report = ResultSummaryReporter.WriteSummaryReport(summary);

            Assert.That(report, Does.Contain("Test Count: 10.000"));
            Assert.That(report, Does.Contain("Passed: 10.000"));
            Assert.That(report, Does.Contain("Failed: 30.000"));
            Assert.That(report, Does.Contain("Warnings: 10.000"));
            Assert.That(report, Does.Contain("Inconclusive: 10.000"));
            Assert.That(report, Does.Contain("Skipped: 30.000"));
            Assert.That(report, Does.Contain("Failed Tests - Failures: 10.000"));
            Assert.That(report, Does.Contain("Errors: 10.000"));
            Assert.That(report, Does.Contain("Invalid: 10.000"));
            Assert.That(report, Does.Contain("Skipped Tests - Ignored: 10.000"));
            Assert.That(report, Does.Contain("Explicit: 10.000"));
            Assert.That(report, Does.Contain("Other: 10.000"));
            Assert.That(report, Does.Contain("Duration: 123,457"));
        }
    }
}
