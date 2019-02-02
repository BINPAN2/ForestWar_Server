using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using GameServer.Controller;
using Common;

namespace GameServer.Servers
{
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;
        private List<Client> clientList = new List<Client>();
        private ControllerManager controllerManager;
        private List<Room> roomList = new List<Room>();

        public Server()
        {

        }

        public Server(string ipStr, int port)
        {
            controllerManager = new ControllerManager(this);
            SetIpAndPort(ipStr, port);
        }
        public void SetIpAndPort(string ipStr, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }

        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            Console.WriteLine("接收到客户端连接");
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            client.Start();
            clientList.Add(client);
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        public void RemoveClient(Client client)
        {
            lock (clientList)
            {
                clientList.Remove(client);
                Console.WriteLine("一个客户端断开链接了");
            }
        }

        public void SendResponse(Client client, ActionCode actionCode, string data)
        {
            //给客户端响应
            client.Send(actionCode, data);
        }


        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }


        public void CreateRoom(Client client)
        {
            Room room = new Room(this);
            room.AddClient(client);
            roomList.Add(room);
        }

        public List<Room> GetRoomList()
        {
            return roomList;
        }

        public void RemoveRoom(Room room)
        {
            if (roomList!=null&&room!=null)
            {
                roomList.Remove(room);
            }
        }

        public Room GetRoomById(int id )
        {
            foreach (Room room in roomList)
            {
                if (room.GetId()==id)
                {
                    return room;
                }
            }
            return null;
        }
    }
}
