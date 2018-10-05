
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

    public class TopicService
    {
        private NamespaceManager nameSpaceManager;

        public TopicService(string serviceBusConnectionString)
        {
            nameSpaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
        }

        public bool CheckIfExists(string topicName)
        {
            bool result = false;
            if (nameSpaceManager.TopicExists(topicName))
            {
                result = true;
                Logger.LogError(string.Format(CultureInfo.InvariantCulture, "\nTopic with name {0} already exists in service bus namespace\n", topicName));
            }
            return result;
        }

        private TopicDescription GenerateTopicDescription(TopicModel topicRequest)
        {
            TopicDescription topicDescription = new TopicDescription(topicRequest.Name);
            topicDescription.MaxSizeInMegabytes = Convert.ToInt32(topicRequest.MaxTopicSize, CultureInfo.InvariantCulture);
            topicDescription.DefaultMessageTimeToLive = new TimeSpan(0, 0, 0, Convert.ToInt32(topicRequest.MessageTimeToLive, CultureInfo.InvariantCulture));
            topicDescription.RequiresDuplicateDetection = topicRequest.DuplicateDetection;
            if (topicDescription.RequiresDuplicateDetection)
            {
                topicDescription.DuplicateDetectionHistoryTimeWindow = new TimeSpan(0, 0, 0, Convert.ToInt32(topicRequest.DuplicateDetectionTimeWindowInSeconds, CultureInfo.InvariantCulture));
            }
            topicDescription.EnablePartitioning = topicRequest.EnablePartitioning;
            return topicDescription;
        }

        public bool Create(IDictionary<string, object> apiRequestModel)
        {
            bool result = false;
            if (null != apiRequestModel)
            {
                TopicModel topicRequest = apiRequestModel.ToEntity<TopicModel>();
                if (null != topicRequest)
                {
                    Logger.LogMessage(string.Format("Checking if topic with name {0} already exists in service bus namespace...", topicRequest.Name));
                    if (!CheckIfExists(topicRequest.Name))
                    {
                        Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating Topic with name {0} in service bus namespace", topicRequest.Name));
                        TopicDescription topicDescription = GenerateTopicDescription(topicRequest);
                        if (null != topicDescription)
                        {
                            Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating topic with name {0} in service bus namespace...", topicRequest.Name));
                            nameSpaceManager.CreateTopic(topicDescription);
                            Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Topic with name {0} created in service bus namespace", topicRequest.Name));
                            result = true;
                        }
                    }
                }
            }
            return result;
        }
    }
}
