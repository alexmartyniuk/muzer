using System;
using System.Collections.Generic;
using System.Messaging;
using MuzerAPI.Implementation;

namespace MuzerAPI
{
    public class FindTrackTaskService
    {
        private const string MessageQueueName = @".\Private$\MuzerFindTrackQueue";
        private Dictionary<FindTrackTask, MessageQueueTransaction> _openedTransactions = new Dictionary<FindTrackTask, MessageQueueTransaction>();

        private MessageQueue GetMessageQueue()
        {
            MessageQueue result;
            if (!MessageQueue.Exists(MessageQueueName))
            {
                result = MessageQueue.Create(MessageQueueName, true);
            }
            else
            {
                result = new MessageQueue(MessageQueueName);
            }

            result.Formatter = new XmlMessageFormatter(new[] { typeof(FindTrackTask) });

            return result;
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
                    messageQueue.Send(task, transaction);
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
                FindTrackTask task = (FindTrackTask)messageQueue.Receive(transaction).Body;
                _openedTransactions.Add(task, transaction);
                return task;
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
    }
}
