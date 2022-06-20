using Informator.Database;
using Informator.Models;
using Informator.SendServices.Email;
using Informator.SendServices.Telegram;

namespace Informator.SendServices
{
    public class HandlerManager
    {
        private List<Thread> _threads = new List<Thread>();
        //Лист содержащий обработчики
        private List<IHandler> _handlers = new List<IHandler>();

        //Реализация класса
        public HandlerManager()
        {
            NewHandler<TelegramHandler>();
            NewHandler<EmailHandler>();
            NewThreaders();
        }

        private void NewThreaders()
        {
            foreach (var handler in _handlers)
            {
                _threads.Add(new Thread(handler.Listener));
            }
            //_handlers.ForEach(handler => _threads.Add(new Thread(handler.Listener)));
            foreach (var thread in _threads)
            {
                thread.Start();
            }
            //_threads.ForEach(thread => thread.Start());
        }

        

        //Метод для создания новых обработчиков и добавления их в лист обработчиков
        //с целью сделать не более одного обработчика определённого типа.
        private void NewHandler<T>()
            where T: IHandler
        {
            //Проверка на существование типа передаваемого обработчика в листе обработчиков.
            if (_handlers.Any(p => p.GetType() == typeof(T)))
            {
                return;
            }
            
            //Если такого типа обработчика в листе нет, то добавляем передаваемый обработчик.
            _handlers.Add((T)Activator.CreateInstance<T>());
        }

        //Метод для рассылки сообщения по всем сервисам
        public void SendMessageAll(string message)
        {
            
        }

        //Метод рассылки сообщения по определённым сервисам
        //T - Generic, обобщённый тип
        //Из всех типов, передаваемых методу, нам необходим интерфейс,
        //который является родителем для обработчиков
        public void SendMessageChoicePlatform<T>(string message, List<string> mailingList = default) where T : IHandler
        {
            var handler = _handlers.FirstOrDefault(p => p.GetType().Equals(typeof(T)));
            handler.Send(message, mailingList);
        }
        
    }
}
