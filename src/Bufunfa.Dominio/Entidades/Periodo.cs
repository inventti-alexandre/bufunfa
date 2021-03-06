﻿using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using System;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa uma período
    /// </summary>
    public class Periodo
    {
        /// <summary>
        /// ID do período
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Nome da período
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Data início do período
        /// </summary>
        public DateTime DataInicio { get; private set; }

        /// <summary>
        /// Data fim do período
        /// </summary>
        public DateTime DataFim { get; private set; }


        private Periodo()
        {
        }

        public Periodo(CadastrarPeriodoEntrada cadastrarEntrada)
        {
            if (cadastrarEntrada.Invalido)
                return;

            this.IdUsuario  = cadastrarEntrada.IdUsuario;
            this.Nome       = cadastrarEntrada.Nome;
            this.DataInicio = cadastrarEntrada.DataInicio;
            this.DataFim    = cadastrarEntrada.DataFim;
        }

        public void Alterar(AlterarPeriodoEntrada alterarEntrada)
        {
            if (alterarEntrada.Invalido || alterarEntrada.IdPeriodo != this.Id)
                return;

            this.Nome       = alterarEntrada.Nome;
            this.DataInicio = alterarEntrada.DataInicio;
            this.DataFim    = alterarEntrada.DataFim;
        }

        public override string ToString()
        {
            return $"{this.Nome} - {this.DataInicio.ToString("dd/MM/yyyy")} até {this.DataFim.ToString("dd/MM/yyyy")}";
        }
    }
}