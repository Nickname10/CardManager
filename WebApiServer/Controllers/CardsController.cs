using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApiServer.Models;
using WebApiServer.Repositories;

namespace WebApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly CardJsonRepository _cards;

        public CardsController(CardJsonRepository repository)
        {
            _cards = repository;
        }

        [HttpGet]
        public IEnumerable<Card> GetAll()
        {
            return _cards.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Card> GetById(int id)
        {
            try
            {
                var card = _cards.GetById(id);
                if (card == null)
                    return NotFound();
                return new ObjectResult(card);
            }
            catch
            {
                return BadRequest();
            }
        }
        //Post: api/Cards
        [HttpPost]
        public ActionResult<Card> PublishCard(Card card)
        {
            if (string.IsNullOrEmpty(card.Title) || card.Image == null)
            {
                return BadRequest();
            }
            _cards.Create(card);
            return Ok(card.Id);
        }

        [HttpPut]
        public ActionResult<Card> ReplaceCard(Card newCard)
        {
            if (newCard == null) return BadRequest();
            _cards.Update(newCard);
            return Ok(newCard.Id);
        }
        [HttpDelete("{id}")]
        public ActionResult<Card> DeleteCard(int id)
        {
            try
            {
                _cards.Delete(id);
                return Ok(id);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}