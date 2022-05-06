namespace Atm.Atendimento.Domain
{
    public class ModoPagamento : Entity
    {
        public bool CartaoCredito { get; set; }
        public bool CartaoDebito { get; set; }
        public bool Dinheiro { get; set; }
        public bool Pix { get; set; }
    }
}
