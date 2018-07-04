using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MuzerAPI;
using MuzerAPI.Implementation;
using MuzerAPI.Models;

namespace ServicesTests
{
    [TestClass]
    public class FindTrackTaskServiceTests
    {
        const string Artist1 = "Artist1";
        const string Artist2 = "Artist2";
        const string Album1 = "Album1";
        const string Album2 = "Album2";
        const string Track1 = "Track1";
        const string Track2 = "Track2";
        const long TrackId1 = 1;
        const long TrackId2 = 2;

        private static readonly ArtistModel ArtistModel1 = new ArtistModel { Name = Artist1 };
        private static readonly AlbumModel AlbumModel1 = new AlbumModel { Title = Album1, Artist = ArtistModel1};
        private static readonly TrackModel TrackModel1 = new TrackModel{ Album = AlbumModel1, Title = Track1, Id = TrackId1};

        private static readonly ArtistModel ArtistModel2 = new ArtistModel { Name = Artist2 };
        private static readonly AlbumModel AlbumModel2 = new AlbumModel { Title = Album2, Artist = ArtistModel2 };
        private static readonly TrackModel TrackModel2 = new TrackModel { Album = AlbumModel2, Title = Track2, Id = TrackId2 };

        FindTrackTaskService _service;

        [TestInitialize]
        public void TestInitialize()
        {
            _service = new FindTrackTaskService
            {
                QueueName = DateTime.Now.ToLongTimeString()
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _service.DestroyStorage();
        }

        [TestMethod]
        public void AddingNewTaskShouldBeSuccessful()
        {
            var task = _service.NewTask(TrackModel1);
            var result = _service.AddTask(task);
            Assert.IsTrue(result);

            var readTask = _service.GetNextTask();
            try
            {
                Assert.IsNotNull(readTask);
                Assert.AreEqual(Artist1, readTask.Artist);
                Assert.AreEqual(Album1, readTask.Album);
                Assert.AreEqual(Track1, readTask.Track);
            }
            finally
            {
                _service.TaskDone(readTask);
            }
        }

        [TestMethod]
        public void Adding2NewTasksShouldBeSuccessful()
        {
            var task1 = _service.NewTask(TrackModel1);
            var result1 = _service.AddTask(task1);
            Assert.IsTrue(result1);

            var task2 = _service.NewTask(TrackModel2);
            var result2 = _service.AddTask(task2);
            Assert.IsTrue(result2);

            var readTask = _service.GetNextTask();
            try
            {
                Assert.IsNotNull(readTask);
                Assert.AreEqual(Artist1, readTask.Artist);
                Assert.AreEqual(Album1, readTask.Album);
                Assert.AreEqual(Track1, readTask.Track);
            }
            finally
            {
                _service.TaskDone(readTask);
            }

            readTask = _service.GetNextTask();
            try
            {
                Assert.IsNotNull(readTask);
                Assert.AreEqual(Artist2, readTask.Artist);
                Assert.AreEqual(Album2, readTask.Album);
                Assert.AreEqual(Track2, readTask.Track);
            }
            finally
            {
                _service.TaskDone(readTask);
            }
        }

        [TestMethod]
        public void AddingTwoSameTasksShouldBeFailed()
        {
            var task = _service.NewTask(TrackModel1);
            var result = _service.AddTask(task);
            Assert.IsTrue(result);

            result = _service.AddTask(task);
            Assert.IsFalse(result);

            var readTask = _service.GetNextTask();
            try
            {
                Assert.IsNotNull(readTask);
                Assert.AreEqual(Artist1, readTask.Artist);
                Assert.AreEqual(Album1, readTask.Album);
                Assert.AreEqual(Track1, readTask.Track);
            }
            finally
            {
                _service.TaskDone(readTask);
            }
        }

        [TestMethod]
        public void GetAndDoneTaskShouldRemoveTask()
        {
            var task = _service.NewTask(TrackModel1);
            var result = _service.AddTask(task);
            Assert.IsTrue(result);

            var readTask = _service.GetNextTask();
            try
            {
                Assert.IsNotNull(readTask);
                Assert.AreEqual(Artist1, readTask.Artist);
                Assert.AreEqual(Album1, readTask.Album);
                Assert.AreEqual(Track1, readTask.Track);
            }
            finally
            {
                _service.TaskDone(readTask);
            }

            readTask = _service.GetNextTask();
            Assert.IsNull(readTask);            
        }

        [TestMethod]
        public void GetAndFailTaskShouldLeaveTask()
        {
            var task = _service.NewTask(TrackModel1);
            var result = _service.AddTask(task);
            Assert.IsTrue(result);

            var readTask = _service.GetNextTask();
            try
            {
                Assert.IsNotNull(readTask);
                Assert.AreEqual(Artist1, readTask.Artist);
                Assert.AreEqual(Album1, readTask.Album);
                Assert.AreEqual(Track1, readTask.Track);
            }
            finally
            {
                _service.TaskFailed(readTask);
            }

            readTask = _service.GetNextTask();

            Assert.IsNotNull(readTask);
            Assert.AreEqual(Artist1, readTask.Artist);
            Assert.AreEqual(Album1, readTask.Album);
            Assert.AreEqual(Track1, readTask.Track);
        }

        [TestMethod]
        public void CorrectTasksCountShouldBeReturned()
        {
            Assert.AreEqual(0, _service.TasksCount());

            var task1 = _service.NewTask(TrackModel1);
            var result = _service.AddTask(task1);
            Assert.IsTrue(result);
            Assert.AreEqual(1, _service.TasksCount());

            var task2 = _service.NewTask(TrackModel2);
            result = _service.AddTask(task2);
            Assert.IsTrue(result);
            Assert.AreEqual(2, _service.TasksCount());

            var readTask = _service.GetNextTask();
            _service.TaskDone(readTask);
            Assert.AreEqual(1, _service.TasksCount());

            readTask = _service.GetNextTask();
            _service.TaskDone(readTask);
            Assert.AreEqual(0, _service.TasksCount());
        }

        [TestMethod]
        public void NewTaskShouldBeCreatedSuccesfully()
        {
            var task1 = _service.NewTask(TrackModel1);
            Assert.AreEqual(Artist1, task1.Artist);
            Assert.AreEqual(Album1, task1.Album);
            Assert.AreEqual(Track1, task1.Track);
        }
    }
}
