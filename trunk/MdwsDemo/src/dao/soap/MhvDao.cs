using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MdwsDemo.mhv;

namespace MdwsDemo.dao.soap
{
    public class MhvDao
    {
        MhvService _svc;

        public MhvDao()
        {
            _svc = new MhvService();
        }

        public PatientArray cdwLookup(string ssn)
        {
            return _svc.cdwLookup("myPassword", ssn);
        }

        //public SecureMessageThreadsTO getSecureMessages(string userId)
        //{
        //    return _svc.getSecureMessages(_appPwd, userId);
        //}

        //public MessageTO writeSecureMessage(string threadId, string messageId, string from, string to, string message, string subject, string messageStatus)
        //{
        //    return _svc.writeSecureMessage(_appPwd, threadId, messageId, from, to, message, subject, messageStatus);
        //}

        //public TriageGroupsTO getValidRecipients(string userId)
        //{
        //    return _svc.getValidRecipients(_appPwd, userId);
        //}
    }
}