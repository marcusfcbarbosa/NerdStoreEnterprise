using NSE.Core.DomainObjects;
using System;

namespace NSE.Cliente.API.Models
{
    public class Clientes : Entity, IAggregateRoot
    {
        protected Clientes() { }
        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public Cpf Cpf { get; private set; }
        public bool Status { get; private set; }
        public Endereco Endereco { get; private set; }
        public Clientes(Guid id,string nome, string email, string cpf)
        {
            Id = id;
            Nome = nome;
            Email = new Email(email);
            Cpf = new Cpf(cpf);
            Status = true;
        }

        public void TrocarEmail(string email)
        {
            Email = new Email(email);
        }
        public void AtribuirEndereco(Endereco endereco)
        {
            Endereco = endereco;
        }

    }
    public class Endereco : Entity
    {
        public Guid ClienteId { get; private set; }
        public Clientes Cliente { get; private set; }
        
        public string Logradouro { get; private set; }
        public string Nome { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string Cep { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        protected Endereco() { }
        public Endereco(string nome, string numero, string complemento,
            string bairro, string cep, string cidade, string estado, string logradouro)
        {
            Nome = nome;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cep = cep;
            Cidade = cidade;
            Estado = estado;
            Logradouro = logradouro;
        }
    }
}
