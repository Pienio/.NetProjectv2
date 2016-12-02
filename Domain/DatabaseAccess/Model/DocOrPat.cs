using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Model
{
    [DataContract]
    public enum DocOrPat
    {
        [DataMember]
        Doctor = 0,
        [DataMember]
        Patient = 1,
    }
}
