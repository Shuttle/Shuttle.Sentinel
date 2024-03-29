﻿using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.Server
{
    public class MessageHeaderHandler :
        IMessageHandler<AddMessageHeader>,
        IMessageHandler<RemoveMessageHeader>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IMessageHeaderQuery _messageHeaderQuery;

        public MessageHeaderHandler(IDatabaseContextFactory databaseContextFactory,
            IMessageHeaderQuery messageHeaderQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(messageHeaderQuery, nameof(messageHeaderQuery));

            _databaseContextFactory = databaseContextFactory;
            _messageHeaderQuery = messageHeaderQuery;
        }

        public void ProcessMessage(IHandlerContext<AddMessageHeader> context)
        {
            var message = context.Message;

            using (_databaseContextFactory.Create())
            {
                _messageHeaderQuery.Save(message.Id, message.Key, message.Value);
            }
        }

        public void ProcessMessage(IHandlerContext<RemoveMessageHeader> context)
        {
            using (_databaseContextFactory.Create())
            {
                _messageHeaderQuery.Remove(context.Message.Id);
            }
        }
    }
}