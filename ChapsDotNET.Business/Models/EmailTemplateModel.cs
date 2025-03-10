using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
// using Microsoft.CodeAnalysis.CSharp;
// using Microsoft.CodeAnalysis.Text;

namespace ChapsDotNET.Business.Models
{
    public class EmailTemplateModel
    {
        public int EmailTemplateID { get; set; }

        public int? CorrespondenceTypeID { get; set; }
        // Note:  Stages, CorrespondenceTypes, and Correspondence items are not yet defined in the upgrade
        public int? StageID { get; set; }
        public bool Chaser { get; set; }
        
        public required string Subject { get; set; }

        public required string BodyText { get; set; }

        // TODO public virtual CorrespondenceType CorrespondenceType { get; set; }
        // TODO public virtual Stage Stage { get; set; }

       /* TODO public virtual string mailTo(string recipients, Correspondence, item)
        {
            const string HTML_TAG_PATTERN = "<.*?>";

            string subject = this.Subject.ParseText(item);
            string body = Regex.Replace(this.BodyText.Replace("<FONT face=Arial>\r\n<P>", "").Replace("<font face=Arial>\r\n<p>", "").Replace("&lt;", "[").Replace("&gt;", "]").Replace("<BR>", "%0D%0A").Replace("<br>", "%0D%0A").Replace("\r\n", "%0D%0A").Replace("</P>", "%0D%0A").Replace("</p>", "%0D%0A").Replace("&nbsp;", " ").Replace(" ", "%20").Replace("@", "%40").Replace("<li>", "").Replace("</li>", "%0D%0A").Replace("<u>", "").Replace("</u>", "").ParseText(item), HTML_TAG_PATTERN, string.Empty);
            return string.Format("mailto:{0}?subject={1}&body={2}", recipients, subject, body);
        } */
    }
}