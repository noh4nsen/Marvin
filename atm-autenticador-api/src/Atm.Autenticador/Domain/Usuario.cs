namespace Atm.Autenticador.Domain
{
    public class Usuario : Entity
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Token { get; set; }
    }
}
