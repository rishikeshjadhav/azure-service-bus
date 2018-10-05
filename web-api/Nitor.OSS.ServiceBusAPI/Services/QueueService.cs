
namespace Nitor.OSS.ServiceBusAPI.Services
{
    using Nitor.OSS.Logger;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using Nitor.OSS.ServiceBusAPI.Helpers;
    using Nitor.OSS.ServiceBusAPI.Models;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class QueueService
    {
        private NamespaceManager nameSpaceManager;

        public QueueService(string serviceBusConnectionString)
        {
            nameSpaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
        }

        public bool CheckIfExists(string queueName)
        {
            bool result = false;
            if (nameSpaceManager.QueueExists(queueName))
            {
                result = true;
                Logger.LogError(string.Format(CultureInfo.InvariantCulture, "\nQueue with name {0} already exists in service bus namespace\n", queueName));
            }
            return result;
        }

        private QueueDescription GenerateQueueDescription(QueueModel queueRequest)
        {
            QueueDescription queueDescription = null;
            if (null != queueRequest)
            {
                queueDescription = new QueueDescription(queueRequest.Name);
                queueDescription.MaxSizeInMegabytes = Convert.ToInt32(queueRequest.MaxQueueSize, CultureInfo.InvariantCulture);
                queueDescription.DefaultMessageTimeToLive = new TimeSpan(0, 0, 0, Convert.ToInt32(queueRequest.MessageTimeToLive, CultureInfo.InvariantCulture));
                queueDescription.LockDuration = new TimeSpan(0, 0, 0, Convert.ToInt32(queueRequest.LockDurationTime, CultureInfo.InvariantCulture));
                queueDescription.RequiresDuplicateDetection = queueRequest.DuplicateDetection;
                if (queueDescription.RequiresDuplicateDetection)
                {
                    queueDescription.DuplicateDetectionHistoryTimeWindow = new TimeSpan(0, 0, 0, Convert.ToInt32(queueRequest.DuplicateDetectionTimeWindowInSeconds, CultureInfo.InvariantCulture));
                }
                queueDescription.EnableDeadLetteringOnMessageExpiration = queueRequest.DeadLettering;
                queueDescription.RequiresSession = queueRequest.Session;
                queueDescription.EnablePartitioning = queueRequest.EnablePartitioning;
            }

            return queueDescription;
        }

        public bool Create(IDictionary<string, object> apiRequestModel)
        {
            bool result = false;
            if (null != apiRequestModel)
            {
                QueueModel queueRequest = apiRequestModel.ToEntity<QueueModel>();
                if (null != queueRequest)
                {
                    Logger.LogMessage(string.Format("Checking if queue with name {0} already exists in service bus namespace...", queueRequest.Name));
                    if (!CheckIfExists(queueRequest.Name))
                    {
                        Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating queue with name {0} in service bus namespace", queueRequest.Name));
                        QueueDescription queueDescription = GenerateQueueDescription(queueRequest);
                        if (null != queueDescription)
                        {
                            Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating queue with name {0} in service bus namespace...", queueRequest.Name));
                            nameSpaceManager.CreateQueue(queueDescription);
                            Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Queue with name {0} created in service bus namespace", queueRequest.Name));
                            result = true;
                        }
                    }
                }
            }
            return result;
        }
    }
}
