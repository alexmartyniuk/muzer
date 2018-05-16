using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MuzerAPI;
using MuzerAPI.Implementation;

namespace ServicesTests
{
    [TestClass]
    public class FindTrackTaskServiceTests
    {
        [TestMethod]
        public void AddingNewTaskShouldBeSuccessful()
        {
            const string Artist = "Artist";
            const string Album = "Album";
            const string Track = "Track";

            var task = new FindTrackTask { Artist = Artist, Album = Album, Track = Track };
            var service = new FindTrackTaskService();

            var result = service.AddTask(task);
            Assert.IsTrue(result);

            var readTask = service.GetNextTask();
            try
            {
                Assert.IsNotNull(readTask);
                Assert.AreEqual(Artist, readTask.Artist);
                Assert.AreEqual(Album, readTask.Album);
                Assert.AreEqual(Track, readTask.Track);
            }
            finally
            {
                service.TaskDone(readTask);
            }

            readTask = service.GetNextTask();
            Assert.IsNull(readTask);
        }

        [TestMethod]
        public void AddingTwoSameTasksShouldBeFailed()
        {
            const string Artist = "Artist";
            const string Album = "Album";
            const string Track = "Track";

            var task = new FindTrackTask { Artist = Artist, Album = Album, Track = Track };
            var service = new FindTrackTaskService();

            var result = service.AddTask(task);
            Assert.IsTrue(result);

            result = service.AddTask(task);
            Assert.IsFalse(result);

            var readTask = service.GetNextTask();
            try
            {
                Assert.IsNotNull(readTask);
                Assert.AreEqual(Artist, readTask.Artist);
                Assert.AreEqual(Album, readTask.Album);
                Assert.AreEqual(Track, readTask.Track);
            }
            finally
            {
                service.TaskDone(readTask);
            }

            readTask = service.GetNextTask();
            Assert.IsNull(readTask);
        }
    }
}
