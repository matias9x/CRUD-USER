using System.ComponentModel.DataAnnotations;
namespace CRUD_USER.Entities
{
    public class Fornecedor    //partindo pra parte do fornecedor, igual a user.cs
    {
        [Key]
        public int idFornecedor { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}
