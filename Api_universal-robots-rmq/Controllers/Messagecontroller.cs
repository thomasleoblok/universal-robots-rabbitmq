using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Api_universal_robots_rmq.Service;
using Api_universal_robots_rmq.DAL;
using MessageModel;

namespace Api_universal_robots_rmq.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Messagecontroller : ControllerBase
    {
        private IMessageService _messageService;
        private readonly robotcontext _context;

        public Messagecontroller(IMessageService messageService, robotcontext context)
        {
            _context = context;
            _messageService = messageService;
        }

        [AllowAnonymous]
        [HttpPost("Create")]
        public IActionResult Message([FromBody] Message model)
        {
            try
            {
                var message = _context.messages.SingleOrDefault(id => id.Id == model.Id);
                if (message == null) return BadRequest();
                var messageresult = new Message { Description = model.Description, State = model.State, Created = DateTime.Now };
                _messageService.Create(messageresult);
                return Ok(messageresult);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public IActionResult UMessage(Guid sid5, Message model)
        {
            try { 
            
            if (sid5 != model.Id) return BadRequest();
            var messageresult = new Message { Description = model.Description};
            _messageService.update(sid5, messageresult);
            return Ok(messageresult);
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var messages = _messageService.Getall();
            return Ok(messages);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Getbyid(int id)
        {
            try
            {
               var messages = _messageService.GetById(id);
               return Ok(messages);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteMessage(int id)
        {
            try
            {
                var messages = _messageService.DeleteMessage(id);
                return Ok(messages);

            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
