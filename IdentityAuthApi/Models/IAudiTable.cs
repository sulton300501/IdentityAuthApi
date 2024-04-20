namespace IdentityAuthApi.Models
{
    public interface IAudiTable
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedData  { get; set; }
        public DateTimeOffset DeleteDate { get; set; }
        public bool IsDeleted { get; set; }

    }   
}
