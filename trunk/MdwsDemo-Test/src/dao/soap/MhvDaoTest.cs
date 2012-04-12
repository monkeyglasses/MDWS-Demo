using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MdwsDemo.mhv;

namespace MdwsDemo.dao.soap
{
    [TestFixture]
    public class MhvDaoTest
    {
        [Test]
        public void testGetValidRecipients()
        {
            MhvDao dao = new MhvDao();
            TriageGroupsTO groups = dao.getValidRecipients("71"); // patient identifier found in SM database

            Assert.IsNotNull(groups);
            Assert.AreEqual(groups.count, 8);
            Assert.IsNull(groups.fault);
            Assert.IsNotNull(groups.triageGroups);
            Assert.IsTrue(groups.triageGroups[0].id > 0);
            Assert.IsFalse(String.IsNullOrEmpty(groups.triageGroups[0].name));
        }

        [Test]
        public void testGetSecureMessages()
        {
            MhvDao dao = new MhvDao();
            SecureMessageThreadsTO messages = dao.getSecureMessages("71");

            Assert.IsNotNull(messages);
            Assert.IsTrue(messages.count > 0);
            Assert.IsNull(messages.fault);
            Assert.IsNotNull(messages.messageThreads);
            Assert.IsTrue(messages.messageThreads[0].id > 0);
            Assert.IsFalse(String.IsNullOrEmpty(messages.messageThreads[0].subject));
            Assert.IsNotNull(messages.messageThreads[0].messages);
            Assert.IsNotNull(messages.messageThreads[0].messages[0]);
            Assert.IsFalse(String.IsNullOrEmpty(messages.messageThreads[0].messages[0].body));
        }

        [Test]
        public void testWriteNewCompleteMessageNewThread()
        {
            MhvDao dao = new MhvDao();

            TriageGroupsTO groups = dao.getValidRecipients("71");
            MessageTO message = dao.writeSecureMessage("", "", "71", Convert.ToString(groups.triageGroups[0].id), "Hello from the MDWS demo project!", "Message Thread Subject", "COMPLETE");

            Assert.IsNotNull(message);
            Assert.IsNull(message.fault);
            Assert.IsTrue(message.id > 0, "Message ID should be set");
            Assert.IsTrue(message.threadId > 0, "Should get the ID for the created thread also");
            Assert.IsTrue(String.Equals(groups.triageGroups[0].name, message.recipientName));
        } 

        [Test]
        public void testWriteNewCompleteMessageActiveThread()
        {
            MhvDao dao = new MhvDao();

            TriageGroupsTO groups = dao.getValidRecipients("71");
            MessageTO message = dao.writeSecureMessage("133274", "", "71", Convert.ToString(groups.triageGroups[0].id), "Hello from the MDWS demo project!", "Message Thread Subject", "COMPLETE");

            Assert.IsNotNull(message);
            Assert.IsNull(message.fault);
            Assert.IsTrue(message.id > 0, "Message ID should be set");
            Assert.IsTrue(message.threadId == 133274, "Message should be part of defined thread");
            Assert.IsTrue(String.Equals(groups.triageGroups[0].name, message.recipientName));
        }

        [Test]
        public void testWriteNewDraftMessageNewThread()
        {
            MhvDao dao = new MhvDao();

            TriageGroupsTO groups = dao.getValidRecipients("71");
            MessageTO message = dao.writeSecureMessage("", "", "71", Convert.ToString(groups.triageGroups[0].id), "Hello from the MDWS demo project!", "Message Thread Subject", "DRAFT");

            Assert.IsNotNull(message);
            Assert.IsNull(message.fault);
            Assert.IsTrue(message.id > 0, "Message ID should be set");
            Assert.IsTrue(message.threadId > 0, "Message should have a thread ID");
            Assert.IsFalse(message.completedDate.Year > 1, "Draft messages should not have a date set");
        } 

        [Test]
        public void testWriteNewDraftMessageActiveThread()
        {
            MhvDao dao = new MhvDao();

            TriageGroupsTO groups = dao.getValidRecipients("71");
            MessageTO message = dao.writeSecureMessage("133274", "", "71", Convert.ToString(groups.triageGroups[0].id), "Hello from the MDWS demo project!", "Message Thread Subject", "DRAFT");

            Assert.IsNotNull(message);
            Assert.IsNull(message.fault);
            Assert.IsTrue(message.id > 0, "Message ID should be set");
            Assert.IsTrue(message.threadId == 133274, "Message should be part of defined thread");
            Assert.IsFalse(message.completedDate.Year > 1, "Draft messages should not have a date set");
        }

        [Test]
        public void testUpdateMessageSetComplete()
        {
            MhvDao dao = new MhvDao();

            TriageGroupsTO groups = dao.getValidRecipients("71");
            MessageTO message = dao.writeSecureMessage("133274", "", "71", Convert.ToString(groups.triageGroups[0].id),
                "Hello from the MDWS demo project!", "Message Thread Subject", "DRAFT");

            Assert.IsNotNull(message);
            Assert.IsNull(message.fault);
            Assert.IsTrue(message.id > 0, "Message ID should be set");
            Assert.IsTrue(message.threadId == 133274, "Message should be part of defined thread");
            Assert.IsFalse(message.completedDate.Year > 1, "Draft messages should not have a date set");

            MessageTO completeMessage = dao.writeSecureMessage("133274", Convert.ToString(message.id), "71",
                Convert.ToString(groups.triageGroups[0].id), "Hello from the MDWS demo project!", "Message Thread Subject", "COMPLETE");

            Assert.IsNotNull(completeMessage);
            Assert.IsNull(completeMessage.fault);
            Assert.IsTrue(completeMessage.id == message.id, "Message ID should be same");
            Assert.IsTrue(completeMessage.threadId == 133274, "Message thread should be same");
            Assert.IsTrue(completeMessage.completedDate.Year == DateTime.Now.Year, "Completed messages should have a completed date set");
        }

        [Test]
        public void testUpdateMessageSetNewRecipient()
        {
            MhvDao dao = new MhvDao();

            TriageGroupsTO groups = dao.getValidRecipients("71");
            MessageTO message = dao.writeSecureMessage("133274", "", "71", Convert.ToString(groups.triageGroups[0].id),
                "Hello from the MDWS demo project!", "Message Thread Subject", "DRAFT");

            Assert.IsNotNull(message);
            Assert.IsNull(message.fault);
            Assert.IsTrue(message.id > 0, "Message ID should be set");
            Assert.IsTrue(message.threadId == 133274, "Message should be part of defined thread");
            Assert.IsFalse(message.completedDate.Year > 1, "Draft messages should not have a date set");

            MessageTO completeMessage = dao.writeSecureMessage("133274", Convert.ToString(message.id), "71",
                Convert.ToString(groups.triageGroups[1].id), "Hello from the MDWS demo project!", "Message Thread Subject", "COMPLETE");

            Assert.IsNotNull(completeMessage);
            Assert.IsNull(completeMessage.fault);
            Assert.IsTrue(completeMessage.id == message.id, "Message ID should be same");
            Assert.IsTrue(completeMessage.threadId == 133274, "Message thread should be same");
            Assert.IsFalse(String.Equals(message.recipientName, completeMessage.recipientName), "Recipient should be different after update");
        }
    }
}
