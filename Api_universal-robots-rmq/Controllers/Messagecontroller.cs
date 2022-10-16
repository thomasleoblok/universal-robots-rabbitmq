﻿using Microsoft.AspNetCore.Mvc;
using Api_universal_robots_rmq.Model;
using Microsoft.AspNetCore.Authorization;
using Api_universal_robots_rmq.Service;
using Api_universal_robots_rmq.DAL;

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
        public IActionResult Message([FromBody] Model.Message model)
        {
            try
            {
                var message = _context.messages.SingleOrDefault(id => id.Id == model.Id);
                if (message == null) return BadRequest();
                var messageresult = new Message { message = model.message, created = model.created };
                _messageService.Create(messageresult);
                return Ok(messageresult);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public IActionResult UMessage(int sid5, Message model)
        {
            try { 
            
            if (sid5 != model.Id) return BadRequest();
            var messageresult = new Message { message = model.message};
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
