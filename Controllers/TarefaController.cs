using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            try
            {
                var tarefa = _context.Tarefas.Where(x => x.Id == id);
                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Erro = "Ocorreu um erro ao processar a sua solicitação." + ex.Message });
            }
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            try
            {
                var tarefas = _context.Tarefas.ToList();
                return Ok(tarefas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Erro = "Ocorreu um erro ao processar a sua solicitação." + ex.Message });
            }
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            try
            {
                var tarefa = _context.Tarefas.Where(x => x.Titulo == titulo);

                if (tarefa.IsNullOrEmpty())
                {
                    return NotFound();
                }

                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Erro = "Ocorreu um erro ao processar a sua solicitação." + ex.Message });
            }
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            try
            {
                var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Erro = "Ocorreu um erro ao processar a sua solicitação." + ex.Message });
            }
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            try
            {
                var tarefa = _context.Tarefas.Where(x => x.Status == status);
                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Erro = "Ocorreu um erro ao processar a sua solicitação." + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            try
            {
                _context.Tarefas.Add(tarefa);

                _context.SaveChanges();

                return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Erro = "Ocorreu um erro ao processar a sua solicitação." + ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            try
            {
                var tarefaBanco = _context.Tarefas.Find(id);

                if (tarefaBanco == null)
                    return NotFound(new { Erro = "Tarefa não encontrada." });

                if (tarefa.Data == DateTime.MinValue)
                    return BadRequest(new { Erro = "A data da tarefa não pode ser vazia." });

                tarefaBanco.Titulo = tarefa.Titulo;
                tarefaBanco.Descricao = tarefa.Descricao;
                tarefaBanco.Data = tarefa.Data;
                tarefaBanco.Status = tarefa.Status;

                _context.Tarefas.Update(tarefaBanco);
                _context.SaveChanges();

                return Ok(tarefaBanco);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Erro = "Ocorreu um erro ao tentar atualizar a tarefa." + ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();
            try
            {
                _context.Tarefas.Remove(tarefaBanco);

                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Erro = "Ocorreu um erro ao tentar deletar a tarefa." + ex.Message });
            }
        }
    }
}
