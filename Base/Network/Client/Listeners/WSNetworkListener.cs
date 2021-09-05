using Base.Network.Shared;
using Base.Network.Shared.Interfaces;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;

namespace Base.Network.Client.Listeners
{
    /// <summary>
    /// This class is responsible for managing the network websocket listener.
    /// </summary>
    public class WSNetworkListener : NetworkListener
    {
        #region private members

        /// <summary>
        /// The client websocket connection.
        /// </summary>
        private ClientWebSocket _clientWebSocket;

        /// <summary>
        /// The buffer size of websocket connection.
        /// </summary>
        private int _messageBufferSize;

        /// <summary>
        /// The listener type of connection.
        /// </summary>
        private NetworkListenerType _listenerType;

        /// <summary>
        /// The buffer of websocket connection.
        /// </summary>
        private ArraySegment<byte> _buff;

        #endregion

        #region properties

        /// <inheritdoc/>
        public override bool IsConnected => _clientWebSocket != null ? _clientWebSocket.State == WebSocketState.Open : false;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a new instance of a <see cref="WSNetworkListener"/>.
        /// </summary>
        /// <param name="listenerType">The listener type of client connection.</param>
        /// <param name="messageReceivedHandler">The callback of message received handler implementation.</param>
        /// <param name="disconnectedHandler">The callback of client disconnected handler implementation.</param>
        public WSNetworkListener(NetworkListenerType listenerType, MessageReceivedHandler messageReceivedHandler, DisconnectedHandler disconnectedHandler)
            : base(messageReceivedHandler, disconnectedHandler)
        {
            _listenerType = listenerType;
        }

        #endregion

        #region public methods implementation

        /// <inheritdoc/>
        public override void StartClient(string ip, int port, ushort maxMessageBuffer)
        {
            try
            {
                _messageBufferSize = maxMessageBuffer;
                _buff = new ArraySegment<byte>(new byte[_messageBufferSize]);
                _clientWebSocket = new ClientWebSocket();

                StartConnection(new Uri($"ws://{ip}:{port}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}.");
            }
        }

        /// <inheritdoc/>
        public override void SendMessage(IBufferWriter writer)
        {
            try
            {
                if (IsConnected)
                {
                    if (_listenerType == NetworkListenerType.WSText)
                    {
                        var data = new ArraySegment<byte>(writer.BufferData, 4, writer.BufferData.Length - 4);
                        _clientWebSocket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else if (_listenerType == NetworkListenerType.WSBinary)
                    {
                        var data = new ArraySegment<byte>(writer.BufferData);
                        _clientWebSocket.SendAsync(data, WebSocketMessageType.Binary, true, CancellationToken.None);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}.");
            }
        }

        /// <inheritdoc/>
        public override void Stop()
        {
            try
            {
                _clientWebSocket.Abort();
                _clientWebSocket.Dispose();

                base.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}.");
            }
        }

        #endregion

        #region private methods implementation

        /// <summary>
        /// Method responsible for starts the web socket connection.
        /// </summary>
        /// <param name="uri">The uri to start the connection</param>
        public async void StartConnection(Uri uri)
        {
            try
            {
                if (_clientWebSocket.State == WebSocketState.Open) return;
                await _clientWebSocket.ConnectAsync(uri, CancellationToken.None);

                while (IsConnected)
                {
                    var ret = await _clientWebSocket.ReceiveAsync(_buff, CancellationToken.None);

                    if (ret.MessageType == WebSocketMessageType.Text)
                    {
                        var data = _buff.Take(ret.Count).ToArray();

                        var writer = BufferWriter.Create();
                        writer.Write(data);

                        var reader = BufferReader.Create(writer.BufferData, 0, writer.Length);

                        _messageReceivedHandler(reader);
                    }
                    else if (ret.MessageType == WebSocketMessageType.Binary)
                    {
                        var reader = BufferReader.Create(_buff.Take(ret.Count).ToArray(), 0, ret.Count);
                        _messageReceivedHandler(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                if (IsConnected)
                    _disconnectedHandler();
                else
                    Console.WriteLine($"Error: {ex.Message}.");
            }
        }

        #endregion
    }
}
