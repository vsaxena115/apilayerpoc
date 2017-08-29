using System;
namespace MailBox_Numverify_POC.Models
{
    public class Email
    {
        public string email { get; set;}
     	public string did_you_mean { get; set; }
		public string user { get; set; }
		public string domain { get; set; }
		public string format_valid { get; set; }
		public string mx_found { get; set; }
		public bool smtp_check { get; set; }
		public string catch_all { get; set; }
		public string role { get; set; }
		public string disposable { get; set; }
        public bool free { get; set; }
		public string score { get; set; }
	}
}
