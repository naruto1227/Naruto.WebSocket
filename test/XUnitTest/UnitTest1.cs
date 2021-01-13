using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Net.WebSockets;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using MessagePack;

namespace XUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            List<ClientWebSocket> s = new List<ClientWebSocket>();
            for (int i = 0; i < 5000; i++)
            {
                ClientWebSocket webSocket = new ClientWebSocket();
                s.Add(webSocket);
                await webSocket.ConnectAsync(new Uri("ws://localhost:5003/ws"), CancellationToken.None);
            }

        }

        [Fact]
        public async Task Test2()
        {
            ClientWebSocket webSocket = new ClientWebSocket();
            await webSocket.ConnectAsync(new Uri("ws://localhost:5003/ws"), CancellationToken.None);

            var msg = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { action = "current", connectionId = "12345678", msg = "hello word" }));
            for (int i = 0; i < 1000; i++)
            {
                await webSocket.SendAsync(msg, WebSocketMessageType.Text, true, CancellationToken.None);
            }

            await Task.Delay(100000);
        }
        [Fact]
        public void Test_MessagePack()
        {
            var mc = new MyClass
            {
                Age = 99,
                FirstName = "hoge",
                LastName = "hoge",
            };
            var res = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mc));
            // Call Serialize/Deserialize, that's all.
            byte[] bytes = MessagePackSerializer.Serialize(mc);
            MyClass mc2 = MessagePackSerializer.Deserialize<MyClass>(bytes);

            // You can dump MessagePack binary blobs to human readable json.
            // Using indexed keys (as opposed to string keys) will serialize to MessagePack arrays,
            // hence property names are not available.
            // [99,"hoge","huga"]
            var json = MessagePackSerializer.ConvertToJson(bytes);
            var res2 = MessagePackSerializer.ConvertFromJson(json);
            MyClass mc3 = MessagePackSerializer.Deserialize<MyClass>(bytes);
            Console.WriteLine(json);
        }
    }

    [MessagePackObject]
    public class MyClass
    {
        // Key attributes take a serialization index (or string name)
        // The values must be unique and versioning has to be considered as well.
        // Keys are described in later sections in more detail.
        [Key(0)]
        public int Age { get; set; }

        [Key(1)]
        public string FirstName { get; set; }

        [Key(2)]
        public string LastName { get; set; }

        // All fields or properties that should not be serialized must be annotated with [IgnoreMember].
        [IgnoreMember]
        public string FullName { get { return FirstName + LastName; } }
    }
}
