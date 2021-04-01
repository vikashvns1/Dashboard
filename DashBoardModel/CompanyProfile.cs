namespace DashBoardModel
{
    public class CompanyProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public string BgColorCode { get; set; }
        public string ContanctNumber { get; set; }
        public string ContactPerson { get; set; }
        public string Plan { get; set; }
        public string Paid { get; set; }
        public bool Active { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
        public string CreateDate { get; set; }
        public string Optional1 { get; set; }
        public string Optional2 { get; set; }
        public string FromMail { get; set; }
        public string ToMail { get; set; }
        public string EmailPassword { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
    }
}
