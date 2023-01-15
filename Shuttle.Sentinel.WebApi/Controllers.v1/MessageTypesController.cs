using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shuttle.Core.Contract;
using Shuttle.Core.Serialization;
using Shuttle.Core.Streams;
using Shuttle.Sentinel.WebApi.Models.v1;

namespace Shuttle.Sentinel.WebApi.Controllers.v1
{
    [Route("[controller]", Order = 1)]
    [Route("v{version:apiVersion}/[controller]", Order = 2)]
    [ApiVersion("1")]
    public class MessageTypesController : Controller
    {
        private static bool _initialized;
        private static readonly List<MessageTypeModel> MessageTypes = new();
        private static readonly object Lock = new();
        private readonly ILogger<MessageTypesController> _logger;
        private readonly ISerializer _serializer;

        public MessageTypesController(ILogger<MessageTypesController> logger, ISerializer serializer)
        {
            _logger = Guard.AgainstNull(logger, nameof(logger));
            _serializer = Guard.AgainstNull(serializer, nameof(serializer));
        }

        private IEnumerable<MessageTypeModel> GetMessageTypes(string search)
        {
            lock (Lock)
            {
                if (!_initialized)
                {
                    InitializeMessageTypes();

                    _initialized = true;
                }

                return MessageTypes
                    .Where(item => string.IsNullOrEmpty(search) || item.MessageType.Contains(search));
            }
        }

        [HttpGet]
        public IActionResult GetSearch()
        {
            return Ok(GetMessageTypes(string.Empty));
        }

        [HttpGet("{search}")]
        public IActionResult GetSearch(string search)
        {
            return Ok(GetMessageTypes(search));
        }

        private void InitializeMessageTypes()
        {
            var messagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages");

            if (!Directory.Exists(messagesFolder))
            {
                return;
            }

            try
            {
                foreach (var file in Directory.GetFiles(messagesFolder))
                {
                    var assembly = Assembly.LoadFile(file);

                    foreach (var type in assembly.GetTypes())
                    {
                        try
                        {
                            var instance = Activator.CreateInstance(type);

                            MessageTypes.Add(new MessageTypeModel
                            {
                                MessageType = type.FullName,
                                EmptyMessageType = Encoding.ASCII.GetString(_serializer.Serialize(instance).ToBytes())
                            });
                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
            }
        }
    }
}