﻿namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um usuário
    /// </summary>
    public class Usuario
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
        /// Senha do usuário
        /// </summary>
        public string Senha { get; internal set; }

        /// <summary>
        /// Indica se o usuário está ativo
        /// </summary>
        public bool Ativo { get; }

        /// <summary>
        /// Permissões de acesso do usuário
        /// </summary>
        public string[] PermissoesAcesso { get; internal set; }

        private Usuario()
        {
            
        }

        public Usuario(string nome, string email, bool ativo = true)
            : this()
        {
            this.Nome = nome;
            this.Email = email;
            this.Ativo = ativo;
        }

        public override string ToString()
        {
            return this.Nome.ToUpper();
        }
    }
}
