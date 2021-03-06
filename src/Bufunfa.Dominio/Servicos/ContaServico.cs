﻿
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
    public class ContaServico : Notificavel, IContaServico
    {
        private readonly IContaRepositorio _contaRepositorio;
        private readonly IUow _uow;

        public ContaServico(
            IContaRepositorio contaRepositorio,
            IUow uow)
        {
            _contaRepositorio = contaRepositorio;
            _uow              = uow;
        }

        public async Task<ISaida> ObterContaPorId(int idConta, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idConta, 0, ContaMensagem.Id_Conta_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var conta = await _contaRepositorio.ObterPorId(idConta);

            // Verifica se a conta existe
            this.NotificarSeNulo(conta, ContaMensagem.Id_Conta_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a conta pertece ao usuário informado.
            this.NotificarSeDiferentes(conta.IdUsuario, idUsuario, ContaMensagem.Conta_Nao_Pertence_Usuario);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { ContaMensagem.Conta_Encontrada_Com_Sucesso }, new ContaSaida(conta));
        }

        public async Task<ISaida> ObterContasPorUsuario(int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lstContas = await _contaRepositorio.ObterPorUsuario(idUsuario);

            return lstContas.Any()
                ? new Saida(true, new[] { ContaMensagem.Contas_Encontradas_Com_Sucesso }, lstContas.Select(x => new ContaSaida(x)))
                : new Saida(true, new[] { ContaMensagem.Nenhuma_Conta_Encontrada }, null);
        }

        public async Task<ISaida> CadastrarConta(CadastrarContaEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (cadastroEntrada.Invalido)
                return new Saida(false, cadastroEntrada.Mensagens, null);

            // Verifica se o usuário já possui alguma conta com o nome informado
            this.NotificarSeVerdadeiro(await _contaRepositorio.VerificarExistenciaPorNome(cadastroEntrada.IdUsuario, cadastroEntrada.Nome), ContaMensagem.Conta_Com_Mesmo_Nome);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var conta = new Conta(cadastroEntrada);

            await _contaRepositorio.Inserir(conta);

            await _uow.Commit();

            return _uow.Invalido 
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ContaMensagem.Conta_Cadastrada_Com_Sucesso }, new ContaSaida(conta));
        }

        public async Task<ISaida> AlterarConta(AlterarContaEntrada alterarEntrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (alterarEntrada.Invalido)
                return new Saida(false, alterarEntrada.Mensagens, null);

            var conta = await _contaRepositorio.ObterPorId(alterarEntrada.IdConta, true);

            // Verifica se a conta existe
            this.NotificarSeNulo(conta, string.Format(ContaMensagem.Id_Conta_Nao_Existe, alterarEntrada.IdConta));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a conta pertece ao usuário informado.
            this.NotificarSeDiferentes(conta.IdUsuario, alterarEntrada.IdUsuario, ContaMensagem.Conta_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o usuário já possui alguma conta com o nome informado
            this.NotificarSeVerdadeiro(await _contaRepositorio.VerificarExistenciaPorNome(alterarEntrada.IdUsuario, alterarEntrada.Nome, alterarEntrada.IdConta), ContaMensagem.Conta_Com_Mesmo_Nome);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            conta.Alterar(alterarEntrada);

            _contaRepositorio.Atualizar(conta);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ContaMensagem.Conta_Alterada_Com_Sucesso }, new ContaSaida(conta));
        }

        public async Task<ISaida> ExcluirConta(int idConta, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idConta, 0, ContaMensagem.Id_Conta_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var conta = await _contaRepositorio.ObterPorId(idConta);

            // Verifica se a conta existe
            this.NotificarSeNulo(conta, ContaMensagem.Id_Conta_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a conta pertece ao usuário informado.
            this.NotificarSeDiferentes(conta.IdUsuario, idUsuario, ContaMensagem.Conta_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _contaRepositorio.Deletar(conta);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ContaMensagem.Conta_Excluida_Com_Sucesso }, new ContaSaida(conta));
        }
    }
}
