﻿using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de uma nova pessoa
    /// </summary>
    public class CadastrarPessoaEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string Nome { get; }

        public CadastrarPessoaEntrada(int idUsuario, string nome)
        {
            this.IdUsuario = idUsuario;
            this.Nome      = nome;

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido)
                .NotificarSeNuloOuVazio(this.Nome, PessoaMensagem.Nome_Obrigatorio_Nao_Informado);

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 200, PessoaMensagem.Nome_Tamanho_Maximo_Excedido);
        }
    }
}
