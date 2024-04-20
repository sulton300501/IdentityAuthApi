using Microsoft.AspNetCore.Identity;

namespace IdentityAuthApi.Models
{
    public class AppUser : IdentityUser , IAudiTable
    {
        public string FullName { get; set; }
        public string Status {  get; set; }
        public int age { get; set; }
        public DateTimeOffset CreatedDate { get; set; }= DateTimeOffset.UtcNow;
        public DateTimeOffset ModifiedData { get; set; }
        public DateTimeOffset DeleteDate { get; set; }
        public bool IsDeleted { get ; set; } = false;

    }
}
