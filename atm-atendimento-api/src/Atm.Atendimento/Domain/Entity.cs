﻿using System;

namespace Atm.Atendimento.Domain
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
