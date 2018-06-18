using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Infraestrutura.NotifiqueMe;
using System.Linq;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class ContaServico : Notificavel, IContaServico
    {
        private readonly IContaRepositorio _contaRepositorio;
        private readonly IUow _uow;

        public ContaServico(IContaRepositorio contaRepositorio, IUow uow)
        {
            _contaRepositorio = contaRepositorio;
            _uow = uow;
        }

        public ISaida ObterContaPorId(int idConta, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idConta, 0, $"O ID da conta informado ({idConta}) é inválido.");
            this.NotificarSeMenorOuIgualA(idUsuario, 0, $"O ID do usuário informado ({idUsuario}) é inválido.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var conta = _contaRepositorio.ObterPorId(idConta);

            // Verifica se a conta existe
            this.NotificarSeNulo(conta, $"A conta de ID {idConta} não existe.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a conta pertece ao usuário informado.
            this.NotificarSeDiferentes(conta.IdUsuario, idUsuario, "A conta não pertence ao usuário autenticado.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            return new Saida(true, new[] { "Conta encontrada com sucesso." }, new ContaSaida(conta));
        }

        public ISaida ObterContasPorUsuario(int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idUsuario, 0, $"O ID do usuário informado ({idUsuario}) é inválido.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lstContas = _contaRepositorio.ObterPorUsuario(idUsuario);

            return lstContas.Any()
                ? new Saida(true, new[] { "Contas do usuário encontradas com sucesso." }, lstContas.Select(x => new ContaSaida(x)))
                : new Saida(true, new[] { "Nenhuma conta do usuário foi encontrada." }, null);
        }

        public ISaida CadastrarConta(CadastrarContaEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (!cadastroEntrada.Valido())
                return new Saida(false, cadastroEntrada.Mensagens, null);

            // Verifica se o usuário já possui alguma conta com o nome informado
            this.NotificarSeVerdadeiro(_contaRepositorio.VerificarExistenciaPorNome(cadastroEntrada.IdUsuario, cadastroEntrada.Nome), $"Você já possui uma conta cadastrada com o nome \"{cadastroEntrada.Nome}\". Informe um nome diferente para a conta.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var conta = new Conta(cadastroEntrada.IdUsuario, cadastroEntrada.Nome)
            {
                NomeInstituicao   = cadastroEntrada.NomeInstituicao,
                Numero            = cadastroEntrada.Numero,
                NumeroAgencia     = cadastroEntrada.NumeroAgencia,
                ValorSaldoInicial = cadastroEntrada.ValorSaldoInicial
            };

            _contaRepositorio.Inserir(conta);

            _uow.Commit();

            return new Saida(true, new[] { $"Conta \"{conta.Nome}\" cadastrada com sucesso." }, new ContaSaida(conta));
        }

        public ISaida AlterarConta(AlterarContaEntrada alterarEntrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (!alterarEntrada.Valido())
                return new Saida(false, alterarEntrada.Mensagens, null);

            // Verifica se o usuário já possui alguma conta com o nome informado
            this.NotificarSeVerdadeiro(_contaRepositorio.VerificarExistenciaPorNome(alterarEntrada.IdUsuario, alterarEntrada.Nome, alterarEntrada.IdConta), $"Você já possui uma conta cadastrada com o nome \"{alterarEntrada.Nome}\". Informe um nome diferente para a conta.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var conta = _contaRepositorio.ObterPorId(alterarEntrada.IdConta, true);

            // Verifica se a conta existe
            this.NotificarSeNulo(conta, $"A conta de ID {alterarEntrada.IdConta} não existe.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a conta pertece ao usuário informado.
            this.NotificarSeDiferentes(conta.IdUsuario, alterarEntrada.IdUsuario, "A conta que se deseja alterar, não pertence ao usuário autenticado.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            conta.Nome              = alterarEntrada.Nome;
            conta.NomeInstituicao   = alterarEntrada.NomeInstituicao;
            conta.NumeroAgencia     = alterarEntrada.NumeroAgencia;
            conta.Numero            = alterarEntrada.Numero;
            conta.ValorSaldoInicial = alterarEntrada.ValorSaldoInicial;

            _contaRepositorio.Atualizar(conta);

            _uow.Commit();

            return new Saida(true, new[] { "Conta alterada com sucesso." }, new ContaSaida(conta));
        }

        public ISaida ExcluirConta(int idConta, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idConta, 0, $"O ID da conta informado ({idConta}) é inválido.");
            this.NotificarSeMenorOuIgualA(idUsuario, 0, $"O ID do usuário informado ({idUsuario}) é inválido.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var conta = _contaRepositorio.ObterPorId(idConta);

            // Verifica se a conta existe
            this.NotificarSeNulo(conta, $"A conta de ID {idConta} não existe.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a conta pertece ao usuário informado.
            this.NotificarSeDiferentes(conta.IdUsuario, idUsuario, "A conta não pertence ao usuário autenticado, por isso não é permitido excluí-la.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _contaRepositorio.Deletar(idConta);

            return new Saida(true, new[] { "Conta excluída com sucesso." }, new ContaSaida(conta));
        }
    }
}
