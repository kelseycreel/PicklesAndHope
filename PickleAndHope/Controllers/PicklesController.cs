using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PickleAndHope.Models;
using PickleAndHope.DataAccess;

namespace PickleAndHope.Controllers
{
    [Route("api/pickles")]
    [ApiController]
    public class PicklesController : ControllerBase
    {
        PickleRepository _repository = new PickleRepository();

        [HttpPost]
        public IActionResult AddPickle(Pickle pickleToAdd)
        {
            var pickleExists = _repository.GetByType(pickleToAdd.Type);
            if (pickleExists == null)
            {
                var newPickle = _repository.Add(pickleToAdd);
                return Created("", newPickle); //201 response
            }
            else
            {
                var updatedPickle = _repository.UpdateNumInStock(pickleToAdd);
                return Ok(updatedPickle); //200 response
            }

        }

        //api/pickles
        [HttpGet]
        public IActionResult GetAllPickles()
        {
            var allPickles = _repository.GetAllPickles();
            return Ok(allPickles);
        }

        [HttpGet("{id}")]
        public IActionResult getPickleById(int id)
        {
            var pickle = _repository.GetById(id);
            return Ok(pickle);
        }

        [HttpGet("type/{type}")]
        public IActionResult GetPickleByType(string type)
        {
            var pickle = _repository.GetByType(type);
            if (pickle == null) return NotFound("No pickle with that type exists");
            return Ok(pickle);
        }
    }
}