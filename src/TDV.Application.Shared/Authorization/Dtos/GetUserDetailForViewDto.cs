namespace TDV.Authorization.Dtos
{
    public class GetUserDetailForViewDto
    {
        public UserDetailDto UserDetail { get; set; }

        public string UserName { get; set; }

        public string ContactDisplayProperty { get; set; }

    }
}