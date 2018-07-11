using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServicesTests
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text;

    using MuzerAPI;
    using MuzerAPI.FileService;

    [TestClass]
    public class FileServiceTests
    {
        private const string STORAGE_PATH = @"c:\Development\Muzer\FileService";
        private FileService _service;

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:PrefixLocalCallsWithThis", Justification = "Reviewed. Suppression is OK here.")]
        [TestInitialize]
        public void TestInitialize()
        {
            RemoveDirectory(STORAGE_PATH);
            _service = new FileService
                           {
                                StorageRoot = STORAGE_PATH
                           };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            RemoveDirectory(STORAGE_PATH);
        }

        [TestMethod]
        public void FileShouldBeAddedSuccessfully()
        {
            var stream = GenerateStream();
            var id = _service.Add(stream);

            Assert.AreNotEqual(null, id);
            Assert.AreNotEqual(string.Empty, id);
        }

        [TestMethod]
        public void FileShouldBeAddedAndGetSuccessfully()
        {
            var stream = GenerateStream();
            var id = _service.Add(stream);

            var readStream = _service.Get(id);
            Assert.IsTrue(CompareStreams(stream, readStream));
        }
        
        [TestMethod]
        public void BigFileShouldBeAddedAndGetSuccessfully()
        {
            var stream = GenerateStream(1024 * 1024 * 100);
            var id = _service.Add(stream);

            var readStream = _service.Get(id);
            Assert.IsTrue(CompareStreams(stream, readStream));
        }

        [TestMethod]
        public void AddedFileShouldExists()
        {
            var stream = GenerateStream();
            var id = _service.Add(stream);

            Assert.IsTrue(_service.Exists(id));
        }

        [TestMethod]
        public void FileShouldBeRemovedSuccessfully()
        {
            var stream = GenerateStream();
            var id = _service.Add(stream);
            _service.Delete(id);            
        }

        [TestMethod]
        public void RemovedFileShouldNotExists()
        {
            var stream = GenerateStream();
            var id = _service.Add(stream);
            _service.Delete(id);

            var addedStream = this._service.Get(id);

            Assert.IsNull(addedStream);
            Assert.IsFalse(_service.Exists(id));
        }

        [TestMethod]
        public void RemovingUnexistedFileShouldFailed()
        {
            const string UNEXISTED_FILE_ID = "UNEXISTED_FILE_ID";

            try
            {
                _service.Delete(UNEXISTED_FILE_ID);
            }
            catch (FileNotFoundException e)
            {
                Assert.AreEqual(UNEXISTED_FILE_ID, e.FileName);                
            }
        }

        private void RemoveDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }

            var di = new DirectoryInfo(directory);
            foreach (FileInfo file in di.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.EnumerateDirectories())
            {
                dir.Delete(true);
            }
        }

        private Stream GenerateStream(int size = 1024)
        {
            string str = new string('*', size);
            return new MemoryStream(Encoding.UTF8.GetBytes(str));
        }

        private bool CompareStreams(Stream a, Stream b)
        {
            if (a == null &&
                b == null)
                return true;
            if (a == null ||
                b == null)
            {
                throw new ArgumentNullException(
                    a == null ? "a" : "b");
            }

            if (a.Length < b.Length)
                return false;
            if (a.Length > b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                int aByte = a.ReadByte();
                int bByte = b.ReadByte();
                if (aByte.CompareTo(bByte) != 0)
                    return false;
            }

            return true;
        }
    }
}
