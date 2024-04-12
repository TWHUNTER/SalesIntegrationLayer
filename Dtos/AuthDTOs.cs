namespace IntegracionDesarrollo3.Dtos
{
    public record LoginDTO(string username, string password);
    public record SignUpDTO(string username, string full_name, string user_password, string email, string phone_number, int profile_type);

}