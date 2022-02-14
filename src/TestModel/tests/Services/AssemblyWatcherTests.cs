// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.IO;
using NUnit.Framework;

namespace TestCentric.Gui.Model.Services
{
    [TestFixture]
    public class AssemblyWatcherTests
    {
        private AssemblyWatcher watcher;
        private CounterEventHandler handler;
        private static int watcherDelayMs = 100;
        private string fileName;
        private string tempFileName;

        [SetUp]
        public void CreateFile()
        {
            string tempDir = Path.GetTempPath();
            fileName = Path.Combine(tempDir, "temp.txt");
            tempFileName = Path.Combine(tempDir, "newTempFile.txt");

            StreamWriter writer = new StreamWriter(fileName);
            writer.Write("Hello");
            writer.Close();

            handler = new CounterEventHandler();
            watcher = new AssemblyWatcher();
            watcher.Setup(watcherDelayMs, fileName);
            watcher.AssemblyChanged += new AssemblyChangedHandler(handler.OnChanged);
            watcher.Start();
        }

        [TearDown]
        public void DeleteFile()
        {
            watcher.Stop();
            FileInfo fileInfo = new FileInfo(fileName);
            fileInfo.Delete();

            FileInfo temp = new FileInfo(tempFileName);
            if (temp.Exists) temp.Delete();
        }

        [Test]
        // TODO: Exclusion should really only apply to Mono on Windows
        //Platform attr does not allow to specify skipping on mono and win10. Therefore this has been skipping on either of them. As a solution, changed the attr to Mono only
        [Platform(Exclude = "Mono")]
        public void MultipleCloselySpacedChangesTriggerWatcherOnlyOnce()
        {
            for (int i = 0; i < 3; i++)
            {
                StreamWriter writer = new StreamWriter(fileName, true);
                writer.WriteLine("Data");
                writer.Close();
                System.Threading.Thread.Sleep(20);
            }

            WaitForTimerExpiration();
            Assert.AreEqual(1, handler.Counter);
            Assert.AreEqual(Path.GetFullPath(fileName), handler.FileName);
        }

        [Test]
        // TODO: Exclusion should really only apply to Mono on Windows
        [Platform(Exclude = "Mono")]
        public void ChangingFileTriggersWatcher()
        {
            StreamWriter writer = new StreamWriter(fileName);
            writer.Write("Goodbye");
            writer.Close();

            WaitForTimerExpiration();
            Assert.AreEqual(1, handler.Counter);
            Assert.AreEqual(Path.GetFullPath(fileName), handler.FileName);
        }

        [Test]
        [Platform(Exclude = "Linux", Reason = "Attribute change triggers watcher")]
        public void ChangingAttributesDoesNotTriggerWatcher()
        {
            FileInfo fi = new FileInfo(fileName);
            FileAttributes attr = fi.Attributes;
            fi.Attributes = FileAttributes.Hidden | attr;

            WaitForTimerExpiration();
            Assert.AreEqual(0, handler.Counter);
        }

        [Test]
        public void CopyingFileDoesNotTriggerWatcher()
        {
            FileInfo fi = new FileInfo(fileName);
            fi.CopyTo(tempFileName);
            fi.Delete();

            WaitForTimerExpiration();
            Assert.AreEqual(0, handler.Counter);
        }

        private static void WaitForTimerExpiration()
        {
            System.Threading.Thread.Sleep(watcherDelayMs * 10);
        }

        private class CounterEventHandler
        {
            int counter;
            String fileName;

            public int Counter
            {
                get { return counter; }
            }

            public String FileName
            {
                get { return fileName; }
            }

            public void OnChanged(String fullPath)
            {
                fileName = fullPath;
                counter++;
            }
        }
    }
}
