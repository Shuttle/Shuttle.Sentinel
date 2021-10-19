using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Logging;
using Shuttle.Core.Serialization;
using Shuttle.Core.Streams;

namespace Shuttle.Sentinel.WebApi.Controllers
{
    [Route("[controller]")]
    public class MessageTypesController : Controller
    {
        private static bool _initialized;
        private static readonly List<MessageTypeModel> MessageTypes = new List<MessageTypeModel>();
        private static readonly object Lock = new object();
        private readonly ILog _log;
        private readonly ISerializer _serializer;

        public MessageTypesController(ISerializer serializer)
        {
            Guard.AgainstNull(serializer, nameof(serializer));

            _serializer = serializer;
            _log = Log.For(this);
        }

        [HttpGet]
        public IActionResult GetSearch()
        {
            return Ok(new
            {
                Data = GetMessageTypes(string.Empty)
            });
        }

        [HttpGet("{search}")]
        public IActionResult GetSearch(string search)
        {
            return Ok(new
            {
                Data = GetMessageTypes(search)
            });
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

        private void InitializeMessageTypes()
        {
            var messagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages");

            if (!Directory.Exists(messagesFolder))
            {
                return;
            }

            foreach (var file in Directory.GetFiles(messagesFolder))
            {
                var assembly = Assembly.LoadFile(file);

                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        var instance = Activator.CreateInstance(type);

                        MessageTypes.Add(new MessageTypeModel
                        {
                            MessageType = type.FullName,
                            EmptyMessageType = Encoding.ASCII.GetString(_serializer.Serialize(instance).ToBytes())
                        });
                    }
                }
                catch (Exception ex)
                {
                    _log.Warning(ex.Message);
                }
            }
        }
    }
}