using System.Collections.Generic;

namespace Atm.Clientes.Domain
{
    public class Carro : Entity
    {
        public string Placa { get; set; }
        public string Descricao { get; set; }
        public long? Quilometragem { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public short? Ano { get; set; }
        public virtual ICollection<Cliente> Clientes { get; set; }

        public Carro()
        {
            Clientes = new HashSet<Cliente>();
        }
    }
}
