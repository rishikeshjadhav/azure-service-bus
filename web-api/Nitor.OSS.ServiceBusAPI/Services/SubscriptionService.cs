
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

    public class SubscriptionService
    {
        private static NamespaceManager nameSpaceManager;

        public SubscriptionService(string serviceBusConnectionString)
        {
            nameSpaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
        }

        public bool CheckIfExists(string topicName, string subscriptionName)
        {
            bool result = false;
            if (nameSpaceManager.SubscriptionExists(topicName, subscriptionName))
            {
                result = true;
                Logger.LogError(string.Format(CultureInfo.InvariantCulture, "Subscription with name {0} already exists under Topic with name {1} under in service bus namespace", subscriptionName, topicName));
            }
            return result;
        }

        private SubscriptionDescription GenerateSubscriptionDescription(SubscriptionModel subscriptionRequest)
        {
            SubscriptionDescription subscriptionDescription = new SubscriptionDescription(subscriptionRequest.TopicName, subscriptionRequest.Name);
            subscriptionDescription.DefaultMessageTimeToLive = new TimeSpan(0, 0, 0, Convert.ToInt32(subscriptionRequest.DefaultMessageTime, CultureInfo.InvariantCulture));
            subscriptionDescription.LockDuration = new TimeSpan(0, 0, Convert.ToInt32(subscriptionRequest.LockDuration, CultureInfo.InvariantCulture));
            subscriptionDescription.MaxDeliveryCount = Convert.ToInt32(subscriptionRequest.MaxDeliveryCount, CultureInfo.InvariantCulture);
            subscriptionDescription.EnableDeadLetteringOnMessageExpiration = subscriptionRequest.MoveExpiredToDLQ;
            subscriptionDescription.EnableDeadLetteringOnFilterEvaluationExceptions = subscriptionRequest.EnableDeadLetteringOnFilterEvaluationExceptions;
            subscriptionDescription.RequiresSession = subscriptionRequest.EnableSessions;
            return subscriptionDescription;
        }

        private RuleDescription GenerateRuleDescription(SubscriptionModel subscriptionRequest)
        {
            RuleDescription ruleDescription = null;
            if (subscriptionRequest.AreFiltersRequired)
            {
                if (!string.IsNullOrWhiteSpace(subscriptionRequest.SQLFilterExpression))
                {
                    ruleDescription = new RuleDescription()
                    {
                        Name = string.Format(CultureInfo.InvariantCulture, "{0}_{1}_Rule", subscriptionRequest.TopicName, subscriptionRequest.Name),
                        Filter = new SqlFilter(subscriptionRequest.SQLFilterExpression)
                    };
                }
            }
            return ruleDescription;
        }

        public bool Create(IDictionary<string, object> apiRequestModel)
        {
            bool result = false;
            if (null != apiRequestModel)
            {
                SubscriptionModel subscriptionRequest = apiRequestModel.ToEntity<SubscriptionModel>();
                if (null != subscriptionRequest)
                {
                    Logger.LogMessage(string.Format("Checking if Subscription with name {0} already exists under Topic with name {0}...", subscriptionRequest.Name, subscriptionRequest.TopicName));
                    if (!CheckIfExists(subscriptionRequest.TopicName, subscriptionRequest.Name))
                    {
                        Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating Subscription with name {0} under Topic {1} in service bus namespace", subscriptionRequest.Name, subscriptionRequest.TopicName));
                        SubscriptionDescription subscriptionDescription = GenerateSubscriptionDescription(subscriptionRequest);
                        RuleDescription ruleDescription = GenerateRuleDescription(subscriptionRequest);
                        if (null == ruleDescription)
                        {
                            nameSpaceManager.CreateSubscription(subscriptionDescription);
                        }
                        else
                        {
                            nameSpaceManager.CreateSubscription(subscriptionDescription, ruleDescription);
                        }
                        Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Subscription with name {0} created under Topic with name {1} in service bus namespace", subscriptionRequest.Name, subscriptionRequest.TopicName));
                        result = true;
                    }
                }
            }
            return result;
        }
    }
}
