using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Infraestrutura.Dados
{
    /// <summary>
    /// Implementação do padrão Unit Of Work
    /// </summary>
    public class Uow : Notificavel, IUow
    {
        private readonly EfDataContext _context;

        public Uow(EfDataContext context)
        {
            _context = context;
        }

        public async Task Commit()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                var mensagemException = dbEx.GetBaseException().Message;

                if (mensagemException.Contains("add or update")) // insert ou update
                {
                    this.NotificarSeVerdadeiro(mensagemException.Contains("REFERENCES `categoria`"), CategoriaMensagem.Id_Categoria_Nao_Existe);

                }
                else if (mensagemException.Contains("delete")) // delete
                {
                    this.NotificarSeVerdadeiro(mensagemException.Contains("REFERENCES `conta`"), ContaMensagem.Conta_Excluir_Erro_FK);
                }
                else
                {
                    this.AdicionarNotificacao($"Não é possível realizar o commit: {mensagemException}");
                }
            }
        }
    }
}
