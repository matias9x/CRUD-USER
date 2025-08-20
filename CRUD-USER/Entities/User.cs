namespace CRUD_USER.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
//variaveis, parecido com o java, muito get igual no php