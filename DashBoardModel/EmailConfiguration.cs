using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DashBoardModel
{
    public class EmailConfiguration
    {
        [Required(ErrorMessage = "Please Enter From EmailId")]
        //[EmailAddress(ErrorMessage = "Please Enter Valid EmailId")]
        public string From { get; set; }
        [Required(ErrorMessage = "SmtpServer Required")]
        public string SmtpServer { get; set; }
        [Required(ErrorMessage = "Port Required")]
        public string Port { get; set; }
        [Required(ErrorMessage = "UserName Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please Enter Valid EmailId")]
        public string[] To { get; set; }
        [Required(ErrorMessage = "Mail Body Required")]
        public string MailBody { get; set; }
        [Required(ErrorMessage = "Subject Required")]
        public string Subject { get; set; }
    }
}
