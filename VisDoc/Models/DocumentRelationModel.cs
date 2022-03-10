using System.ComponentModel.DataAnnotations;

namespace VisDoc.Models
{
    public class DocumentRelationModel
    {
        [Key]
        public int DoocumentID {get; set;}
        
        [Key]
        public int ParentID { get; set;}
    }
}
