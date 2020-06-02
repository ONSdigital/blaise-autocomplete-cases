﻿using Blaise.Nuget.PubSub.Contracts.Interfaces;
using BlaiseCaseAutoComplete.Interfaces.Providers;
using BlaiseCaseAutoComplete.Services;
using Moq;
using NUnit.Framework;

namespace BlaiseCaseAutoComplete.Tests.Services
{
    public class QueueServiceTests
    {
        private Mock<IConfigurationProvider> _configurationProviderMock;
        private Mock<IMessageHandler> _messageHandlerMock;
        private Mock<IFluentQueueApi> _queueProviderMock;

        private readonly string _projectId;
        private readonly string _topicId;
        private readonly string _subscriptionId;

        private QueueService _sut;

        public QueueServiceTests()
        {
            _projectId = "ProjectId";
            _topicId = "TopicId";
            _subscriptionId = "SubscriptionId";
        }

        [SetUp]
        public void SetUpTests()
        {
            _configurationProviderMock = new Mock<IConfigurationProvider>();
            _configurationProviderMock.Setup(c => c.ProjectId).Returns(_projectId);
            _configurationProviderMock.Setup(c => c.SubscriptionId).Returns(_subscriptionId);
            _configurationProviderMock.Setup(c => c.TopicId).Returns(_topicId);

            _messageHandlerMock = new Mock<IMessageHandler>();

            _queueProviderMock = new Mock<IFluentQueueApi>();

            _sut = new QueueService(
                _configurationProviderMock.Object,
                _queueProviderMock.Object);
        }

        [Test]
        public void Given_I_Call_Subscribe_Then_The_Correct_Call_Is_Made_And_Subscribes_To_The_Appropriate_Queues()
        {
            //arrange
            _queueProviderMock.Setup(q => q.ForProject(It.IsAny<string>())).Returns(_queueProviderMock.Object);
            _queueProviderMock.Setup(q => q.ForSubscription(It.IsAny<string>())).Returns(_queueProviderMock.Object);
            _queueProviderMock.Setup(q => q.StartConsuming(It.IsAny<IMessageHandler>()));

            //act
            _sut.Subscribe(_messageHandlerMock.Object);

            //assert
            _queueProviderMock.Verify(v => v.ForProject(_projectId), Times.Once);
            _queueProviderMock.Verify(v => v.ForSubscription(_subscriptionId), Times.Once);
            _queueProviderMock.Verify(v => v.StartConsuming(_messageHandlerMock.Object), Times.Once);
        }


        [Test]
        public void Given_I_Call_CancelAllSubscriptions_Then_The_Correct_Call_Is_Made()
        {
            //arrange
            _queueProviderMock.Setup(q => q.StopConsuming());

            //act
            _sut.CancelAllSubscriptions();

            //assert
            _queueProviderMock.Verify(v => v.StopConsuming(), Times.Once);
        }
    }
}