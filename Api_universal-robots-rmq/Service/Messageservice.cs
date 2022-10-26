using Api_universal_robots_rmq.DAL;
using MessageModel;

namespace Api_universal_robots_rmq.Service
{
    public interface IMessageService
    {
        Message Create(Message CreateMessage);
        Message update(Guid id, Message UpdateMessage);
        IEnumerable<Message> Getall();
        Message GetById(int id);
        Message DeleteMessage(int id);
    }
    public class Messageservice: IMessageService
    {
        private robotcontext _context;
        public Messageservice
        (
            robotcontext context
        )
        {
            _context = context;
        }

        public Message Create(Message CreateMessage)
        {
            _context.messages.Add(CreateMessage);
            _context.SaveChanges();
            return CreateMessage;
        }

        public Message update(Guid id, Message updateMessage)
        {
            var message = _context.messages.Find(id);
            if (message == null) throw new Exception("Message not found");
            _context.messages.Update(updateMessage);
            _context.SaveChanges();
            return updateMessage;
        }

        public IEnumerable<Message> Getall()
        {
            return _context.messages;
        }
        public Message GetById(int id)
        {
            var message = _context.messages.Find(id);
            if (message == null) throw new KeyNotFoundException("Message not found");
            return message;
        }

        public Message DeleteMessage(int id)
        {
            var message = _context.messages.Find(id);
            if (message == null) throw new KeyNotFoundException("message not found");
            _context.messages.Remove(message);
            _context.SaveChanges();
            return message;
        }
    }
}
