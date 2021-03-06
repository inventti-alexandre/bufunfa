﻿using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para alterar uma parcela
    /// </summary>
    public class AlterarParcelaEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário da conta
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Id da parcela
        /// </summary>
        public int IdParcela { get; }

        /// <summary>
        /// Data da parcela
        /// </summary>
        public DateTime Data { get; }

        /// <summary>
        /// Valor da parcela
        /// </summary>
        public decimal Valor { get; }

        /// <summary>
        /// Observação da parcela
        /// </summary>
        public string Observacao { get; }

        public AlterarParcelaEntrada(
            int idParcela,
            DateTime data,
            decimal valor,
            int idUsuario,
            string observacao = null)
        {
            this.IdParcela  = idParcela;
            this.IdUsuario  = idUsuario;
            this.Data       = data;
            this.Valor      = valor;
            this.Observacao = observacao;

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, this.IdUsuario))
                .NotificarSeMenorOuIgualA(this.IdParcela, 0, string.Format(ParcelaMensagem.Id_Parcela_Invalido, this.IdParcela));

            if (!string.IsNullOrEmpty(this.Observacao))
                this.NotificarSePossuirTamanhoSuperiorA(this.Observacao, 500, ParcelaMensagem.Observacao_Tamanho_Maximo_Excedido);
        }
    }
}
