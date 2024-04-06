using Fleck;
using Newtonsoft.Json;
using RubricaTelefonicaAziendale.Dtos;

namespace RubricaTelefonicaAziendale.Handlers
{
    public class WebSocketHandler
    {
        public static List<IWebSocketConnection> WsConnections { get; set; } = [];
        public static List<WsMessage> WsMessageIn { get; set; } = [];
        public static List<WsMessage> WsMessageOut { get; set; } = [];

        public static void Send(WsMessage message)
        {
            foreach (var webSocketConnection in WsConnections)
            {
                webSocketConnection.Send(JsonConvert.SerializeObject(message));
            }
        }
    
    }

    public class WsMessage
    {
        public String Message { get; set; } = String.Empty;
        public PeopleDto? Content { get; set; }
    }
}