﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace JNogueira.Bufunfa.Api.Controllers
{
    [Produces("application/json")]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Obtém do token JWT, o ID do usuário
        /// </summary>
        public int ObterIdUsuarioClaim() => Convert.ToInt32(User.Claims.First(x => x.Type == "IdUsuario").Value);
    }
}