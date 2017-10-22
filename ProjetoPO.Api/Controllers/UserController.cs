using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProjetoPO.Api.Contexts;
using ProjetoPO.Api.Models;
using ProjetoPO.Api.Results;

namespace ProjetoPO.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly BaseContext _context;

        public UserController(BaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todos os usuários
        /// </summary>
        /// <response code="200">A lista de usuários cadastrados</response>
        /// <response code="204">Se não houver usuários cadastrados</response>
        [ProducesResponseType(typeof(List<User>), 200)]
        [ProducesResponseType(204)]
        [HttpGet]
        public IActionResult Get()
        {
            var users = _context.Users.ToList();
            if (!users.Any())
            {
                return new NoContentOperationResult("Nenhum usuário encontrado.");
            }

            return new OkOperationResult("Lista de usuários obtida com sucesso!", users);
        }

        /// <summary>
        /// Obtém um usuário pelo Id
        /// </summary>
        /// <param name="id">O Id do usuário</param>
        /// <response code="200">O usuário encontrado pelo Id especificado</response>
        /// <response code="204">Se não houver usuário com o Id especificado</response>
        /// <response code="400">Se o Id especificado for inválido</response>
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestOperationResult("Id inválido.");
            }

            var user = _context.Users.Find(id);
            if (user == null)
            {
                return new NoContentOperationResult("Nenhum usuário encontrado com o Id especificado.");
            }

            return new OkOperationResult("usuário obtido com sucesso!", user);
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <param name="newUser">O novo usuário</param>
        /// <response code="201">O usuário criado</response>
        /// <response code="400">Se o campo Username for inválido</response>
        /// <response code="400">Se o campo Password for inválido</response>
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(400)]
        [HttpPost]
        public IActionResult Post([FromBody]BaseUser newUser)
        {
            if (string.IsNullOrWhiteSpace(newUser.Username))
            {
                return new BadRequestOperationResult("Username não pode ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(newUser.Password))
            {
                return new BadRequestOperationResult("Password não pode ser nulo.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = newUser.Username,
                Password = newUser.Password
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            return new CreatedOperationResult("usuário criado com sucesso!", user);
        }

        /// <summary>
        /// Atualiza um usuário
        /// </summary>
        /// <param name="id">O Id do usuário</param>
        /// <param name="updatedUser">O novos dados do usuário</param>
        /// <response code="202">O usuário atualizado</response>
        /// <response code="204">Se não houver usuário com o Id especificado</response>
        /// <response code="400">Se o Id especificado for inválido</response>
        [ProducesResponseType(typeof(User), 202)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]BaseUser updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestOperationResult("Id inválido.");
            }

            var user = _context.Users.Find(id);
            if (user == null)
            {
                return new NoContentOperationResult("Nenhum usuário encontrado com o Id especificado.");
            }

            user.Username = string.IsNullOrWhiteSpace(updatedUser.Username) ? user.Username : updatedUser.Username;
            user.Password = string.IsNullOrWhiteSpace(updatedUser.Password) ? user.Password : updatedUser.Password;
            user = _context.Update(user).Entity;
            _context.SaveChanges();

            return new AcceptedOperationResult("usuário atualizado com sucesso!", user);
        }

        /// <summary>
        /// Exclui um usuário
        /// </summary>
        /// <param name="id">O Id do usuário</param>
        /// <response code="200">O usuário excluído</response>
        /// <response code="204">Se não houver usuário com o Id especificado</response>
        /// <response code="400">Se o Id especificado for inválido</response>
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestOperationResult("Id inválido.");
            }

            var user = _context.Users.Find(id);
            if (user == null)
            {
                return new NoContentOperationResult("Nenhum usuário encontrado com o Id especificado.");
            }

            user = _context.Remove(user).Entity;
            _context.SaveChanges();

            return new OkOperationResult("usuário excluído com sucesso!", user);
        }
    }
}
