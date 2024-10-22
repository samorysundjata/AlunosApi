using AlunosApi.Context;
using AlunosApi.Model;
using Microsoft.EntityFrameworkCore;

namespace AlunosApi.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly AppDbContext _context;

        public AlunoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Aluno>> GetAlunos()
        {
            try
            {
                return await _context.Alunos.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public async Task<Aluno> GetAluno(int id)
        {
            try
            {
                var aluno = await _context.Alunos.FindAsync(id);
                return aluno;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Erro ao recuperar aluno com id {id}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Aluno>> GetAlunoByNome(string nome)
        {
            try
            {
                IEnumerable<Aluno> alunos;
                if (!string.IsNullOrWhiteSpace(nome))
                {
                    alunos = await _context.Alunos.Where(a => a.Nome.Contains(nome)).ToListAsync();
                }else
                {
                    alunos = await GetAlunos();
                }
                return alunos;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao recuperar alunos com nome {nome}: {ex.Message}", ex);
            }
        }

        public async Task CreateAluno(Aluno aluno)
        {
            try
            {
                _context.Alunos.Add(aluno);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao criar aluno: {ex.Message}", ex);
            }
        }

        public async Task UpdateAluno(Aluno aluno)
        {
            try
            {
                _context.Entry(aluno).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao atualizar aluno com id {aluno.Id}: {ex.Message}", ex);
            }
        }

        public async Task DeleteAluno(int id)
        {
            try
            {
                var aluno = await _context.Alunos.FindAsync(id);
                if (aluno == null)
                {
                    throw new ApplicationException($"Aluno com id {id} não encontrado.");
                }

                _context.Alunos.Remove(aluno);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao deletar aluno com id {id}: {ex.Message}", ex);
            }
        }

    }
}
