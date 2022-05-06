namespace Atm.Fornecedor.Domain
{
    public class Fornecedor : Entity
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; }
        public string Endereco { get; set; }
    }
}