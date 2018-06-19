﻿using JNogueira.Bufunfa.Dominio.Entidades;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    public interface IUsuarioRepositorio
    {
        /// <summary>
        /// Obtém um usuário a partir do seu e-mail e senha
        /// </summary>
        Usuario ObterPorEmailSenha(string email, string senha);
    }
}
