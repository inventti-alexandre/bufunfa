﻿using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class PessoaServico : Notificavel, IPessoaServico
    {
        private readonly IPessoaRepositorio _pessoaRepositorio;
        private readonly IUow _uow;

        public PessoaServico(IPessoaRepositorio pessoaRepositorio, IUow uow)
        {
            _pessoaRepositorio = pessoaRepositorio;
            _uow               = uow;
        }

        public async Task<ISaida> ObterPessoaPorId(int idPessoa, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idPessoa, 0, PessoaMensagem.Id_Pessoa_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var pessoa = await _pessoaRepositorio.ObterPorId(idPessoa);

            // Verifica se a pessoa existe
            this.NotificarSeNulo(pessoa, PessoaMensagem.Id_Pessoa_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a pessoa pertece ao usuário informado.
            this.NotificarSeDiferentes(pessoa.IdUsuario, idUsuario, PessoaMensagem.Pessoa_Nao_Pertence_Usuario);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { PessoaMensagem.Pessoa_Encontrada_Com_Sucesso }, new PessoaSaida(pessoa));
        }

        public async Task<ISaida> ProcurarPessoas(ProcurarPessoaEntrada procurarEntrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            return procurarEntrada.Invalido
                ? new Saida(false, procurarEntrada.Mensagens, null)
                : await _pessoaRepositorio.Procurar(procurarEntrada);
        }

        public async Task<ISaida> CadastrarPessoa(CadastrarPessoaEntrada cadastroEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (cadastroEntrada.Invalido)
                return new Saida(false, cadastroEntrada.Mensagens, null);

            // Verifica se já existe uma pessoa com o mesmo nome informado
            this.NotificarSeVerdadeiro(await _pessoaRepositorio.VerificarExistenciaPorNome(cadastroEntrada.IdUsuario, cadastroEntrada.Nome), PessoaMensagem.Pessoa_Com_Mesmo_Nome);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var pessoa = new Pessoa(cadastroEntrada);

            await _pessoaRepositorio.Inserir(pessoa);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { PessoaMensagem.Pessoa_Cadastrada_Com_Sucesso }, new PessoaSaida(pessoa));
        }

        public async Task<ISaida> AlterarPessoa(AlterarPessoaEntrada alterarEntrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (alterarEntrada.Invalido)
                return new Saida(false, alterarEntrada.Mensagens, null);

            var pessoa = await _pessoaRepositorio.ObterPorId(alterarEntrada.IdPessoa, true);

            // Verifica se a pessoa existe
            this.NotificarSeNulo(pessoa, string.Format(PessoaMensagem.Id_Pessoa_Nao_Existe, alterarEntrada.IdPessoa));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a pessoa pertece ao usuário informado.
            this.NotificarSeDiferentes(pessoa.IdUsuario, alterarEntrada.IdUsuario, PessoaMensagem.Pessoa_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe uma pessoa com o mesmo nome informado
            this.NotificarSeVerdadeiro(await _pessoaRepositorio.VerificarExistenciaPorNome(alterarEntrada.IdUsuario, alterarEntrada.Nome, alterarEntrada.IdPessoa), PessoaMensagem.Pessoa_Com_Mesmo_Nome);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            pessoa.Alterar(alterarEntrada);

            _pessoaRepositorio.Atualizar(pessoa);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { PessoaMensagem.Pessoa_Alterada_Com_Sucesso }, new PessoaSaida(pessoa));
        }

        public async Task<ISaida> ExcluirPessoa(int idPessoa, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idPessoa, 0, PessoaMensagem.Id_Pessoa_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var pessoa = await _pessoaRepositorio.ObterPorId(idPessoa);

            // Verifica se a pessoa existe
            this.NotificarSeNulo(pessoa, PessoaMensagem.Id_Pessoa_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a pessoa pertece ao usuário informado.
            this.NotificarSeDiferentes(pessoa.IdUsuario, idUsuario, PessoaMensagem.Pessoa_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _pessoaRepositorio.Deletar(pessoa);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { PessoaMensagem.Pessoa_Excluida_Com_Sucesso }, new PessoaSaida(pessoa));
        }
    }
}
