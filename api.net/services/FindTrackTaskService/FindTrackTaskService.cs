using System;
using System.Collections.Generic;
using System.Messaging;
using MuzerAPI.Implementation;

namespace MuzerAPI
{
    public class FindTrackTaskService
    {
        private string _messageQueueName;

        private const long GetTimeoutInMilliseconds = 1000;

        private Dictionary<FindTrackTask, MessageQueueTransaction> _openedTransactions = new Dictionary<FindTrackTask, MessageQueueTransaction>();

        public FindTrackTaskService(string queueName = "muzer_find_track")
        {
            _messageQueueName = @".\Private$\" + queueName;
        }

        public bool IsTaskExists(FindTrackTask task)
        {
            MessageQueue messageQueue = GetMessageQueue();
            try
            {
                Message[] allMessages = messageQueue.GetAllMessages();
                foreach (Message msg in allMessages)
                {
                    FindTrackTask currentTask = (FindTrackTask) msg.Body;
                    if (task.Equals(currentTask))
                    {
                        return true;
                    }
                }

                return false;
            }
            finally
            {
                messageQueue.Close();
            }
        }

        public bool AddTask(FindTrackTask task)
        {
            if (IsTaskExists(task))
            {
                return false;
            }

            MessageQueue messageQueue = GetMessageQueue();
            try
            {
                var transaction = new MessageQueueTransaction();
                transaction.Begin();
                try
                {
                    messageQueue.Send(task, task.ToString(), transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Abort();
                    throw;
                }

            }
            finally
            {
                messageQueue.Close();
            }

            return true;
        }

        public FindTrackTask GetNextTask()
        {
            MessageQueue messageQueue = GetMessageQueue();
            try
            {
                var transaction = new MessageQueueTransaction();
                transaction.Begin();
                try
                {
                    FindTrackTask task = (FindTrackTask)messageQueue.Receive(TimeSpan.FromMilliseconds(GetTimeoutInMilliseconds), transaction)?.Body;
                    _openedTransactions.Add(task, transaction);
                    return task;
                }
                catch (MessageQueueException ex)
                {
                    if (ex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                    {
                        return null;
                    }

                    throw;
                }
            }
            finally
            {
                messageQueue.Close();
            }
        }

        public void TaskDone(FindTrackTask task)
        {
            if (_openedTransactions.TryGetValue(task, out var transaction))
            {
                transaction.Commit();
                _openedTransactions.Remove(task);
            }
            else
            {
                throw new Exception($"Transaction for {task} not found.");
            }
        }

        public void TaskFailed(FindTrackTask task)
        {
            if (_openedTransactions.TryGetValue(task, out var transaction))
            {
                transaction.Abort();
                _openedTransactions.Remove(task);
            }
            else
            {
                throw new Exception($"Transaction for {task} not found.");
            }
        }

        public int TasksCount()
        {
            MessageQueue messageQueue = GetMessageQueue();
            try
            {
                Message[] allMessages = messageQueue.GetAllMessages();
                
                return allMessages.Length;
            }
            finally
            {
                messageQueue.Close();
            }
        }

        public void DestroyStorage()
        {
            if (MessageQueue.Exists(_messageQueueName))
            {
                MessageQueue.Delete(_messageQueueName);
            }
        }

        public FindTrackTask NewTask(string artist, string album, string track)
        {
            return new FindTrackTask { Artist = artist, Album = album, Track = track };
        }

        private MessageQueue GetMessageQueue()
        {
            MessageQueue result;
            if (!MessageQueue.Exists(_messageQueueName))
            {
                result = MessageQueue.Create(_messageQueueName, true);
            }
            else
            {
                result = new MessageQueue(_messageQueueName);
            }

            //result.Formatter = new XmlMessageFormatter(new[] { typeof(FindTrackTask) });
            result.Formatter = new JsonMessageFormatter<FindTrackTask>();

            return result;
        }
    }
}
