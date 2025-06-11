using System;
using System.Collections.Generic;


namespace MediatorPatternDemo
{

    public interface IChatMediator
    {
        void SendMessage(string message, User sender);
        void RegisterUser(User user);
    }

    public abstract class User
    {
        protected IChatMediator _mediator;
        public string Name { get; }

        public User(IChatMediator mediator, string name)
        {
            _mediator = mediator;
            Name = name;
        }

        public abstract void Send(string message);
        public abstract void Receive(string message);
    }

    public class ChatRoom : IChatMediator
    {
        private List<User> _users = new List<User>();

        public void RegisterUser(User user)
        {
            _users.Add(user);
        }

        public void SendMessage(string message, User sender)
        {
            foreach (var user in _users)
            {
                if (user != sender)
                {
                    user.Receive(message);
                }
            }
        }
    }

    public class ChatUser : User
    {
        public ChatUser(IChatMediator mediator, string name) : base(mediator, name)
        {
        }

        public override void Send(string message)
        {
            Console.WriteLine($"[{Name}] відправляє: {message}");
            _mediator.SendMessage(message, this);
        }

        public override void Receive(string message)
        {
            Console.WriteLine($"[{Name}] отримує: {message}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Демонстрація шаблону Mediator ---");

            IChatMediator chatRoom = new ChatRoom();

            User user1 = new ChatUser(chatRoom, "Олена");
            User user2 = new ChatUser(chatRoom, "Іван");
            User user3 = new ChatUser(chatRoom, "Петро");

            chatRoom.RegisterUser(user1);
            chatRoom.RegisterUser(user2);
            chatRoom.RegisterUser(user3);

            user1.Send("Привіт усім!");
            Console.WriteLine();
            user2.Send("Привіт, Олено! Як справи?");

            Console.ReadKey();
        }
    }
}
