using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MdwsDemo.domain;
using MdwsDemo.utils;

namespace MdwsDemo.dao.rest
{
    public class CrudDao
    {
        HttpClient _client;

        public CrudDao(String baseUri)
        {
            _client = new HttpClient(baseUri);
        }

        public VistaRecordTO read(String siteId, String vistaFile, String recordId, String fields = null)
        {
            if (String.IsNullOrEmpty(fields))
            {
                return JsonUtils.Deserialize<VistaRecordTO>(_client.makeRequest(String.Format("{0}/{1}/{2}", siteId, vistaFile, recordId)));
            }
            else
            {
                return JsonUtils.Deserialize<VistaRecordTO>(_client.makeRequest(String.Format("{0}/{1}/{2}/{3}", siteId, vistaFile, recordId, fields)));
            }
        }

        public TextTO delete(String siteId, String vistaFile, String recordId)
        {
            return JsonUtils.Deserialize<TextTO>(_client.makeRequest(HttpRequestType.DELETE, String.Format("{0}/{1}/{2}", siteId, vistaFile, recordId)));
        }

        public TextTO create(String siteId, String vistaFile, String iens, Dictionary<String, String> fieldsAndValues)
        {
            VistaRecordTO recordToCreate = new VistaRecordTO()
            {
                file = new VistaFileTO() { number = vistaFile },
                iens = iens,
                siteId = siteId,
                fields = new VistaFieldTO[fieldsAndValues.Count]
            };
            
            IList<VistaFieldTO> fieldsToSet = new List<VistaFieldTO>();
            foreach (String key in fieldsAndValues.Keys)
            {
                fieldsToSet.Add(new VistaFieldTO() { number = key, value = fieldsAndValues[key] });
            }

            fieldsToSet.CopyTo(recordToCreate.fields, 0);

            return JsonUtils.Deserialize<TextTO>(_client.postOrPut(HttpRequestType.POST, JsonUtils.Serialize<VistaRecordTO>(recordToCreate)));
        }

        public TextTO update(String siteId, String vistaFile, String iens, Dictionary<String, String> fieldsAndValues)
        {
            VistaRecordTO recordToUpdate = new VistaRecordTO()
            {
                file = new VistaFileTO() { number = vistaFile },
                iens = iens,
                siteId = siteId,
                fields = new VistaFieldTO[fieldsAndValues.Count]
            };

            IList<VistaFieldTO> fieldsToSet = new List<VistaFieldTO>();
            foreach (String key in fieldsAndValues.Keys)
            {
                fieldsToSet.Add(new VistaFieldTO() { number = key, value = fieldsAndValues[key] });
            }

            fieldsToSet.CopyTo(recordToUpdate.fields, 0);

            return JsonUtils.Deserialize<TextTO>(_client.postOrPut(HttpRequestType.PUT, JsonUtils.Serialize<VistaRecordTO>(recordToUpdate)));
        }
    }
}