namespace DemoApplication.Areas.Client.ViewModels.User
{
	public class UpdateViewModel
	{
		public int Id { get; set; }
		public string? Email { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Password { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
