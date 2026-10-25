namespace MyFirstApi.Dto
{
    public class TokenDto                                      //when we log in ,a token is generated and it stays in tokendto.It is used for further api calls like update information and so on.
    {
        public string Token { get; set; } = string.Empty;       
        public string Message { get; set; }
    }
}
