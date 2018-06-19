﻿using JNogueira.Bufunfa.Dominio.Entidades;

namespace JNogueira.Bufunfa.Dominio.Comandos.Saida
{
    /// <summary>
    /// Comando de sáida para as informações de um usuário
    /// </summary>
    public class UsuarioSaida
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Indica se o usuário está ativo
        /// </summary>
        public bool Ativo { get; }

        /// <summary>
        /// Permissões de acesso do usuário
        /// </summary>
        public string[] PermissoesAcesso { get; }

        public UsuarioSaida(Usuario usuario)
        {
            if (usuario == null)
                return;

            this.Id               = usuario.Id;
            this.Nome             = usuario.Nome.ToUpper();
            this.Email            = usuario.Email.ToLower();
            this.Ativo            = usuario.Ativo;
            this.PermissoesAcesso = usuario.PermissoesAcesso;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
