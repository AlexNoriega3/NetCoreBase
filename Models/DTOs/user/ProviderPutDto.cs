namespace Models.DTOs.user
{
    public class ProviderPutDto : UserPutDto
    {
        public string LocalId { get; set; }
        public List<string>? SubDepartments { get; set; }
    }
}