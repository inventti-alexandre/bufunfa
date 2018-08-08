using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class CartaoCreditoServico : Notificavel, ICartaoCreditoServico
    {
        private readonly ICartaoCreditoRepositorio _cartaoCreditoRepositorio;
        private readonly IUow _uow;

        public CartaoCreditoServico(ICartaoCreditoRepositorio cartaoCreditoRepositorio, IUow uow)
        {
            _cartaoCreditoRepositorio = cartaoCreditoRepositorio;
            _uow = uow;
        }

        // TODO: Continuar refatoração daqui.

        public async Task<ISaida> ObterCartaoCreditoPorId(int idCartao, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idCartao, 0, string.Format(CartaoCreditoMensagem.Id_Cartao_Invalido, idCartao));
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var cartao = await _cartaoCreditoRepositorio.ObterPorId(idCartao);

            // Verifica se o cartão existe
            this.NotificarSeNulo(cartao, string.Format(CartaoCreditoMensagem.Id_Cartao_Nao_Existe, idCartao));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o cartão pertece ao usuário informado.
            this.NotificarSeDiferentes(cartao.IdUsuario, idUsuario, CartaoCreditoMensagem.Cartao_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            return new Saida(true, new[] { CartaoCreditoMensagem.Cartao_Encontrado_Com_Sucesso }, new CartaoCreditoSaida(cartao));
        }

        public async Task<ISaida> ObterCartoesCreditoPorUsuario(int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lstCartoesCredito = await _cartaoCreditoRepositorio.ObterPorUsuario(idUsuario);

            return lstCartoesCredito.Any()
                ? new Saida(true, new[] { CartaoCreditoMensagem.Cartoes_Encontrados_Com_Sucesso }, lstCartoesCredito.Select(x => new CartaoCreditoSaida(x)))
                : new Saida(true, new[] { CartaoCreditoMensagem.Nenhum_Cartao_Encontrado }, null);
        }

        public async Task<ISaida> CadastrarCartaoCredito(CadastrarCartaoCreditoEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (!cadastroEntrada.Valido())
                return new Saida(false, cadastroEntrada.Mensagens, null);

            // Verifica se o usuário já possui algum cartão com o nome informado
            this.NotificarSeVerdadeiro(await _cartaoCreditoRepositorio.VerificarExistenciaPorNome(cadastroEntrada.IdUsuario, cadastroEntrada.Nome), CartaoCreditoMensagem.Cartao_Com_Mesmo_Nome);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var cartao = new CartaoCredito(cadastroEntrada);

            await _cartaoCreditoRepositorio.Inserir(cartao);

            await _uow.Commit();

            return new Saida(true, new[] { CartaoCreditoMensagem.Cartao_Cadastrado_Com_Sucesso }, new CartaoCreditoSaida(cartao));
        }

        public async Task<ISaida> AlterarCartaoCredito(AlterarCartaoCreditoEntrada alterarEntrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (!alterarEntrada.Valido())
                return new Saida(false, alterarEntrada.Mensagens, null);

            var cartao = await _cartaoCreditoRepositorio.ObterPorId(alterarEntrada.IdCartaoCredito, true);

            // Verifica se o cartão existe
            this.NotificarSeNulo(cartao, string.Format(CartaoCreditoMensagem.Id_Cartao_Nao_Existe, alterarEntrada.IdCartaoCredito));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o cartão pertece ao usuário informado.
            this.NotificarSeDiferentes(cartao.IdUsuario, alterarEntrada.IdUsuario, CartaoCreditoMensagem.Cartao_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o usuário já possui algum cartão com o nome informado
            this.NotificarSeVerdadeiro(await _cartaoCreditoRepositorio.VerificarExistenciaPorNome(alterarEntrada.IdUsuario, alterarEntrada.Nome, alterarEntrada.IdCartaoCredito), CartaoCreditoMensagem.Cartao_Com_Mesmo_Nome);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            cartao.Alterar(alterarEntrada);

            _cartaoCreditoRepositorio.Atualizar(cartao);

            await _uow.Commit();

            return new Saida(true, new[] { CartaoCreditoMensagem.Cartao_Alterado_Com_Sucesso }, new CartaoCreditoSaida(cartao));
        }

        public async Task<ISaida> ExcluirCartaoCredito(int idCartao, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idCartao, 0, string.Format(CartaoCreditoMensagem.Id_Cartao_Invalido, idCartao));
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var cartao = await _cartaoCreditoRepositorio.ObterPorId(idCartao);

            // Verifica se o cartão existe
            this.NotificarSeNulo(cartao, string.Format(CartaoCreditoMensagem.Id_Cartao_Nao_Existe, idCartao));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o cartão pertece ao usuário informado.
            this.NotificarSeDiferentes(cartao.IdUsuario, idUsuario, CartaoCreditoMensagem.Cartao_Excluir_Nao_Pertence_Usuario);

            //TODO: Verificar se possui parcelas
            //TODO: Verificar se possui faturas
            //TODO: Verificar se possui agendamentos

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _cartaoCreditoRepositorio.Deletar(cartao);

            await _uow.Commit();

            return new Saida(true, new[] { CartaoCreditoMensagem.Cartao_Excluido_Com_Sucesso }, new CartaoCreditoSaida(cartao));
        }
    }
}
