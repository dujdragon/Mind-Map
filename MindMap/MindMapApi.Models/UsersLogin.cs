//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MindMapApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UsersLogin
    {
        public int id { get; set; }
        public string username { get; set; }
        public string pwd { get; set; }
        public System.DateTime createdate { get; set; }
        public Nullable<long> files { get; set; }
    
        public virtual UsersFiles UsersFiles { get; set; }
    }
}
