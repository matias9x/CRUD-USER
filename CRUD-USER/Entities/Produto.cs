using System.ComponentModel.DataAnnotations;
namespace CRUD_USER.Entities
{
    public class Produto   //igual a user.cs e fornecedor.cs
    {
        [Key]
        public int IdProduto { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // vou criar uma fkey pro fornecedor 
        public int IdFornecedor { get; set; }
        public required Fornecedor Fornecedor { get; set; }  //relacionamento com fornecedor usei o required pq é obrigatorio,
                                                             //e quando ele for criar um produto ele tem que ter um fornecedor

        //criar uma fkey agora pro user
        public int IdUsuarioCriador { get; set; }
        public required User UsuarioCriador { get; set; }  //relacionamento com user, mesma coisa do fornecedor, puro BD

    }
}
    