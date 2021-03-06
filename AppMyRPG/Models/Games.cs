//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppMyRPG.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Games
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Games()
        {
            this.Comment = new HashSet<Comment>();
        }
    
        public int GameID { get; set; }
        public string GameName { get; set; }
        public System.DateTime GameTime { get; set; }
        public string GameDec { get; set; }
        public string GameFace { get; set; }
        public Nullable<int> GameCmtCount { get; set; }
        public Nullable<int> GameHot { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> TypeID { get; set; }
        public string LinkID { get; set; }
        public string GameImg1 { get; set; }
        public string GameImg2 { get; set; }
        public string GameImg3 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual Link Link { get; set; }
        public virtual GameType GameType { get; set; }
        public virtual UserInfo UserInfo { get; set; }
    }
}
