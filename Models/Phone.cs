namespace BankAPI.Models
{
    public class Phone
    {
        public int Id { get; set; }

        public string Number { get; set; } = string.Empty;

        public PhoneType Type { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; } = null!;
    }
}
