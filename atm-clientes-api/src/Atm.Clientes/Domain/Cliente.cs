using System.Collections.Generic;

namespace Atm.Clientes.Domain
{
    public class Cliente : Entity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Cep { get; set; }
        public virtual ICollection<Carro> Carros { get; set; }

        public Cliente()
        {
            Carros = new HashSet<Carro>();
        }
    }
}
