﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Business.Models
{
    public class Produto : Entity
    {
        public Guid FornecedorId { get; set; }

        public string Nome { get; set; }
        
        public string Descricao { get; set; }

        public string Imagem { get; set; }

        public decimal Valor { get; set; }
        
        public DateTime DataCadastro { get; set; }

        public bool Ativo { get; set; }

        /*EF relation*/
        public Fornecedor Fornecedor { get; set; }

    }
}
