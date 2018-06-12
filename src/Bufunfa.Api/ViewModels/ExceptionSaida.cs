namespace JNogueira.Bufunfa.Api.ViewModels
{
    /// <summary>
    /// Classe que retorna as informações de uma exception encontrada
    /// </summary>
    public class ExceptionSaida
    {
        /// <summary>
        /// Mensagem da exception
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// Mensagem da base exception
        /// </summary>
        public string BaseException { get; set; }

        /// <summary>
        /// Source da exception
        /// </summary>
        public string Source { get; set; }
    }
}
