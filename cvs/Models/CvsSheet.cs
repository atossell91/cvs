﻿using cvs.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;

namespace cvs.Models
{
    public class CvsSheet
    {
        public EstablishmentInfo EstablishmentInfo { get; set; }

        public int WeekNumber { get; set; }
        public ObservableCollection<CvsTask> Tasks { get; set; } = new ObservableCollection<CvsTask>();

        public string FormDataVersion { get; set; }
        public string TemplateId { get; set; }
        public string Revision { get; set; }
        public string RevisionString { get; set; }
        public string Name { get; set; }

        public DateTime ReportIssueDate { get; set; } = DateTime.MinValue;
        public string ReportInspector { get; set; }
        public string InspectorSignature { get; set; }

        public int CompletedRowCount
        {
            get
            {
                return 21;
            }
        }
        public int WorksheetCount { get; set; } = 1;
        public int PageCount { get; set; } = 1;

        public CvsSheet()
        {
        }

    }

    public static class CvsSheetFactory
    {
        public static CvsSheet Create(string EstablishmentNum, string EstablishmentName,
            DateTime date)
        {
            EstablishmentInfo establishmentInfo = new EstablishmentInfo();
            establishmentInfo.ID = "599";
            establishmentInfo.Name = "K&R Poultry";
            establishmentInfo.Address = "2447 Townline Road";
            establishmentInfo.City = "Abbotsford";
            establishmentInfo.Province = "British Columbia";
            establishmentInfo.Country = "Canada";
            establishmentInfo.PostalCode = "V2T 6L6";

            CvsSheet sheet = new CvsSheet();

            sheet.FormDataVersion = "2";
            sheet.TemplateId = "CFIA / ACIA 5470 E";
            sheet.Revision = "69.11.2";
            sheet.RevisionString = "Revision / Révision : 1.5";
            sheet.Name = "Verification worksheet & report";

            sheet.WeekNumber = WeekNumberCalculator.CalcWeekNumber(date);
            sheet.EstablishmentInfo = establishmentInfo;

            int subtractVal = (int)date.DayOfWeek - 1;
            DateTime mondayDate = date.AddDays(-subtractVal);

            for (int n =0; n < 5; ++n)
            {
                DateTime currentDate = mondayDate.AddDays(n);
                sheet.Tasks.Add(new CvsTask
                {
                    Date = currentDate,
                    Shift = 1,
                    TimeIn = new TimeSpan(6, 0, 0),
                    TimeOut = new TimeSpan(14, 0, 0),
                    ActivityCode = "9.1.12",
                    TaskRating = "C",
                    ActivityConducted = "",
                    ItemsNeedingCorrection = "",
                    HoursSpent = 0,
                    MinutesSpent = 30,
                    InspectorNames = "NAME"
                });
                sheet.Tasks.Add(new CvsTask
                {
                    Date = currentDate,
                    Shift = 2,
                    TimeIn = new TimeSpan(11, 0, 0),
                    TimeOut = new TimeSpan(19, 0, 0),
                    ActivityCode = "9.1.13",
                    TaskRating = "C",
                    ActivityConducted = "Kill did not go past 18:00",
                    ItemsNeedingCorrection = "",
                    HoursSpent = 0,
                    MinutesSpent = 30,
                    InspectorNames = "NAME"
                });
            }

            CvsSheetXmlSerializer.Serialize(sheet);

            return sheet;
        }
    }

    public class CvsSheetXmlSerializer
    {

        private static XmlElement addNewXmlNode(XmlDocument doc, XmlElement parent, string name, string data)
        {
            var node = doc.CreateElement(name);

            if (data != null)
            {
                node.InnerText = data;
            }

            parent.AppendChild(node);
            return node;
        }

        private static void addNodeAttribute(XmlDocument doc, XmlElement node, string name, string value)
        {
            var attrib = doc.CreateAttribute(name);
            attrib.InnerText = value;
            node.Attributes.Append(attrib);
        }

        private static void RepeatSimpleNode(XmlDocument doc, XmlElement node, params string[] names)
        {
            int rowWidth = 4;
            int len = names.Length;
            if (len % rowWidth != 0)
            {
                throw new BadArrayLengthException(
                    "Array length must be a multiple of " + rowWidth
                    + " (current array length is " + names.Length + ")");
            }

            for (int i = 0; i < len/rowWidth; ++i)
            {
                int indexBase = i * rowWidth;
                var tempNode = addNewXmlNode(doc, node, names[indexBase], names[indexBase+1]);

                string attrName = names[indexBase + 2];
                string attrValue = names[indexBase + 3];

                if (attrName != null && attrValue != null)
                {
                    addNodeAttribute(doc, tempNode, names[indexBase + 2], names[indexBase + 3]);
                }
            }
        }

        public static string SerializeDate(DateTime date, string dateFormat)
        {
            if (date > DateTime.MinValue)
            {
                return date.ToString(dateFormat);
            }
            else
            {
                return String.Empty;
            }
        }

        public static string Serialize(CvsSheet sheet)
        {
            string dateFormat = "yyyy-MM-dd";
            string timeFormat = "hh\\:mm";

            XmlDocument xmlDoc = new XmlDocument();
            //XmlElement xmlRoot = xmlDoc.CreateElement("root");
            //xmlDoc.AppendChild(xmlRoot);

            var declaration = xmlDoc.CreateXmlDeclaration("1.0", null, null);
            xmlDoc.InsertBefore(declaration, xmlDoc.DocumentElement);

            int indexCounter = 1;

            var dataNode = xmlDoc.CreateElement("formData");
            addNodeAttribute(xmlDoc, dataNode, "version", sheet.FormDataVersion);

            xmlDoc.AppendChild(dataNode);

            var shana = addNewXmlNode(xmlDoc, dataNode, "shana", string.Empty);
            
            var templateNode = addNewXmlNode(xmlDoc, shana, "template", null);
            addNodeAttribute(xmlDoc, templateNode, "id", sheet.TemplateId);
            addNodeAttribute(xmlDoc, templateNode, "rev", sheet.Revision);
            addNodeAttribute(xmlDoc, templateNode, "name", sheet.Name);

            var modelNode = addNewXmlNode(xmlDoc, dataNode, "model", String.Empty);

            RepeatSimpleNode(xmlDoc, modelNode,
                "string", null, "name", "establishment_no",
                "string", null, "name", "establishment_name",
                "string", null, "name", "week_num"
                );

            var groupNode = addNewXmlNode(xmlDoc, modelNode, "group", String.Empty);
            addNodeAttribute(xmlDoc, groupNode, "name", "table4");
            addNodeAttribute(xmlDoc, groupNode, "maxOccurs", "*");

            RepeatSimpleNode(xmlDoc, groupNode,
                "date",     null, "name", "visit_date",
                "string",   null, "name", "shift",
                "string",   null, "name", "Operating",
                "string",   null, "name", "visited",
                "time",     null, "name", "time_in",
                "time",     null, "name", "time_out",
                "string",   null, "name", "task_num",
                "string",   null, "name", "task_rating",
                "string",   null, "name", "activities",
                "string",   null, "name", "items_req_attn",
                "string",   null, "name", "rowcount_info",
                "string",   null, "name", "hours",
                "string",   null, "name", "minutes",
                "string",   null, "name", "inspector_name"
                );

            RepeatSimpleNode(xmlDoc, modelNode,
                "string", null, "name", "establishment_no1",
                "string", null, "name", "establishment_name1",
                "date", null, "name", "report_issued_date",
                "string", null, "name", "report_inspector",
                "string", null, "name", "inspector_signature3",
                "string", null, "name", "report_inspector2",
                "string", null, "name", "operator_signature",
                "string", null, "name", "facility_name_lu",
                "string", null, "name", "address1_lu",
                "string", null, "name", "address2_lu",
                "string", null, "name", "postal_code_lu",
                "string", null, "name", "city_lu",
                "string", null, "name", "province_lu",
                "string", null, "name", "country_lu"
                );

            var secondGroupNode = addNewXmlNode(xmlDoc, modelNode, "group", null);
            addNodeAttribute(xmlDoc, secondGroupNode, "name", "table19");
            addNodeAttribute(xmlDoc, secondGroupNode, "maxOccurs", "*");
            RepeatSimpleNode(xmlDoc, secondGroupNode,
                "date", null, "name", "visit_date1",
                "string", null, "name", "shift1",
                "string", null, "name", "task_num1",
                "string", null, "name", "items_req_attn1",
                "string", null, "name", "activities_conducted",
                "string", null, "name", "result",
                "date", null, "name", "Cell43",
                "string", null, "name", "inspector_name1"
                );

            var thirdGroupNode = addNewXmlNode(xmlDoc, modelNode, "group", null);
            addNodeAttribute(xmlDoc, thirdGroupNode, "name", "table20");
            addNodeAttribute(xmlDoc, thirdGroupNode, "maxOccurs", "*");
            RepeatSimpleNode(xmlDoc, thirdGroupNode,
                "string", null, "name", "Informed_PrintPage",
                "string", null, "name", "page_num"
                );

            RepeatSimpleNode(xmlDoc, modelNode,
                "string", null, "name", "number_rows_completed",
                "string", null, "name", "number_pages_worksheet",
                "string", null, "name", "Cell41",
                "string", null, "name", "number_rows_completed1",
                "string", null, "name", "number_pages_report",
                "string", null, "name", "Cell42",
                "boolean", null, "name", "print_instructions_ws",
                "boolean", null, "name", "print_instructions_rep",
                "boolean", null, "name", "print_instructions_ws_rep",
                "boolean", null, "name", "print_worksheet",
                "boolean", null, "name", "print_report",
                "string", null, "name", "printsettings_tab",
                "string", null, "name", "print_control"
                );

            var fourthGroup = addNewXmlNode(xmlDoc, modelNode, "group", null);
            addNodeAttribute(xmlDoc, fourthGroup, "name", "table34");
            addNodeAttribute(xmlDoc, fourthGroup, "maxOccurs", "*");
            RepeatSimpleNode(xmlDoc, fourthGroup,
                "string", null, "name", "code",
                "string", null, "name", "attn_req_desc",
                "string", null, "name", "task_num_req",
                "string", null, "name", "visit_date_req",
                "string", null, "name", "Cell46",
                "string", null, "name", "shift_req"
                );

            RepeatSimpleNode(xmlDoc, modelNode,
                "string", null, "name", "revision_no",
                "string", null, "name", "revision_no1",
                "string", null, "name", "shift_1",
                "string", null, "name", "shift_2",
                "string", null, "name", "Cell33",
                "boolean", null, "name", "Cell34",
                "boolean", null, "name", "Cell35",
                "boolean", null, "name", "Cell36",
                "boolean", null, "name", "Cell37",
                "boolean", null, "name", "Cell38",
                "boolean", null, "name", "Cell39",
                "boolean", null, "name", "Cell40",
                "string", null, "name", "week_num1"
                );

            var fifthGroup = addNewXmlNode(xmlDoc, modelNode, "group", null);
            addNodeAttribute(xmlDoc, fifthGroup, "name", "table48");
            addNodeAttribute(xmlDoc, fifthGroup, "maxOccurs", "*");
            RepeatSimpleNode(xmlDoc, fifthGroup,
                "string", null, "name", "cover_up"
                );

            RepeatSimpleNode(xmlDoc, modelNode,
                "boolean", null, "name", "Cell49",
                "stickyNotes", null, "name", "stickyNotes",
                "attachments", null, "name", "attachments"
                );

            var comment = xmlDoc.CreateComment("Start of main table");
            //modelNode.AppendChild(comment);

            var instanceNode = addNewXmlNode(xmlDoc, dataNode, "instance", null);
            addNodeAttribute(xmlDoc, instanceNode, "index", "1");
            addNodeAttribute(xmlDoc, instanceNode, "valid", "1");
            RepeatSimpleNode(xmlDoc, instanceNode,
                "establishment_no", sheet.EstablishmentInfo.ID, null, null,
                "establishment_name", sheet.EstablishmentInfo.Name, null, null,
                "week_num", sheet.WeekNumber.ToString(), null, null
                );

            foreach (var task in sheet.Tasks)
            {
                var instanceElem = addNewXmlNode(xmlDoc, instanceNode, "table4", null);
                addNodeAttribute(xmlDoc, instanceElem, "index", indexCounter.ToString());

                addNewXmlNode(xmlDoc, instanceElem, "visit_date", task.Date.ToString(dateFormat));
                addNewXmlNode(xmlDoc, instanceElem, "shift", task.Shift.ToString());
                addNewXmlNode(xmlDoc, instanceElem, "operating", String.Empty);
                addNewXmlNode(xmlDoc, instanceElem, "visited", String.Empty);
                addNewXmlNode(xmlDoc, instanceElem, "time_in", task.TimeIn.ToString(timeFormat));
                addNewXmlNode(xmlDoc, instanceElem, "time_out", task.TimeOut.ToString(timeFormat));
                addNewXmlNode(xmlDoc, instanceElem, "task_num", task.ActivityCode);
                addNewXmlNode(xmlDoc, instanceElem, "task_rating", task.TaskRating);
                addNewXmlNode(xmlDoc, instanceElem, "activities", task.ActivityConducted);
                addNewXmlNode(xmlDoc, instanceElem, "items_req_attn", task.ItemsNeedingCorrection);
                // rowcount_info (below) used to be set to the value of Task.ActivityConducted
                addNewXmlNode(xmlDoc, instanceElem, "rowcount_info", String.Empty);
                addNewXmlNode(xmlDoc, instanceElem, "hours", task.HoursSpent.ToString());
                addNewXmlNode(xmlDoc, instanceElem, "minutes", task.MinutesSpent.ToString());
                addNewXmlNode(xmlDoc, instanceElem, "inspector_name", task.InspectorNames);

                ++indexCounter;
            }

            addNewXmlNode(xmlDoc, instanceNode, "establishment_no1", sheet.EstablishmentInfo.ID);
            addNewXmlNode(xmlDoc, instanceNode, "establishment_name1", sheet.EstablishmentInfo.Name);
            addNewXmlNode(xmlDoc, instanceNode, "report_issued_date", string.Empty);
            addNewXmlNode(xmlDoc, instanceNode, "report_inspector", string.Empty);
            addNewXmlNode(xmlDoc, instanceNode, "inspector_signature3", string.Empty);
            addNewXmlNode(xmlDoc, instanceNode, "report_inspector2", string.Empty);
            addNewXmlNode(xmlDoc, instanceNode, "operator_signature", string.Empty);
            addNewXmlNode(xmlDoc, instanceNode, "facility_name_lu", sheet.EstablishmentInfo.Name);
            addNewXmlNode(xmlDoc, instanceNode, "address1_lu", sheet.EstablishmentInfo.Address);
            addNewXmlNode(xmlDoc, instanceNode, "address2_lu", String.Empty);
            addNewXmlNode(xmlDoc, instanceNode, "postal_code_lu", sheet.EstablishmentInfo.PostalCode);
            addNewXmlNode(xmlDoc, instanceNode, "city_lu", sheet.EstablishmentInfo.Address);
            addNewXmlNode(xmlDoc, instanceNode, "province_lu", sheet.EstablishmentInfo.Province);
            addNewXmlNode(xmlDoc, instanceNode, "country_lu", sheet.EstablishmentInfo.Country);

            int repCount = 8;
            for (int n =0; n < repCount; ++n)
            {
                int index = n + 1;
                var table20Node = addNewXmlNode(xmlDoc, instanceNode, "table20", String.Empty);
                addNodeAttribute(xmlDoc, table20Node, "index", index.ToString());

                addNewXmlNode(xmlDoc, table20Node, "Informed_PrintPage", String.Empty);
                addNewXmlNode(xmlDoc, table20Node, "page_num", index.ToString());
            }

            RepeatSimpleNode(xmlDoc, instanceNode,
                "number_rows_completed", sheet.CompletedRowCount.ToString(), null, null,
                "number_pages_worksheet", sheet.PageCount.ToString(), null, null,
                "Cell41", String.Empty, null, null,
                "number_rows_completed1", sheet.CompletedRowCount.ToString(), null, null,
                "number_pages_report", sheet.PageCount.ToString(), null, null,
                "Cell42", String.Empty, null, null,
                "print_instructions_ws", 0.ToString(), null, null,
                "print_instructions_rep", 0.ToString(), null, null,
                "print_instructions_ws_rep", 0.ToString(), null, null,
                "print_worksheet", 0.ToString(), null, null,
                "print_report", 0.ToString(), null, null,
                "printsettings_tab", string.Empty, null, null,
                "print_control", "no print", null, null,
                "revision_no", sheet.RevisionString, null, null,
                "revision_no1", sheet.RevisionString, null, null,
                "shift_1", String.Empty, null, null,
                "shift_2", String.Empty, null, null,
                "Cell33", String.Empty, null, null,
                "Cell34", 0.ToString(), null, null,
                "Cell35", 0.ToString(), null, null,
                "Cell36", 0.ToString(), null, null,
                "Cell37", 0.ToString(), null, null,
                "Cell38", 0.ToString(), null, null,
                "Cell39", 0.ToString(), null, null,
                "Cell40", 0.ToString(), null, null,
                "week_num1", sheet.WeekNumber.ToString(), null, null,
                "Cell49", 1.ToString(), null, null,
                "stickynotes", String.Empty, null, null
                );

            return xmlDoc.InnerXml;
        }

        private static CvsTask deserializeTask(XmlNode taskNode)
        {
            DateTime date;
            if (!DateTime.TryParse(taskNode["visit_date"].InnerText, out date)) { }

            int shift;
            if (!int.TryParse(taskNode["shift"].InnerText, out shift)) { }

            TimeSpan timeIn;
            if (!TimeSpan.TryParse(taskNode["time_in"].InnerText, out timeIn)) {
                Debug.WriteLine("Time In Parse failed");
            }

            TimeSpan timeOut;
            if (!TimeSpan.TryParse(taskNode["time_out"].InnerText, out timeOut))
            {
                Debug.WriteLine("Time Out Parse failed");
            }

            string activityCode = taskNode["task_num"].InnerText;

            string taskRating = taskNode["task_rating"].InnerText;

            string activityConducted = taskNode["activities"].InnerText;

            string itemsNeedingCorrection = taskNode["items_req_attn"].InnerText;

            int hours;
            if (!int.TryParse(taskNode["hours"].InnerText, out hours)) { }

            int minutes;
            if (!int.TryParse(taskNode["minutes"].InnerText, out minutes)) { }

            string inspectorNames = taskNode["inspector_name"].InnerText;

            CvsTask task = new CvsTask()
            {
                Date = date,
                Shift = shift,
                TimeIn = timeIn,
                TimeOut = timeOut,
                ActivityCode = activityCode,
                TaskRating = taskRating,
                ActivityConducted = activityConducted,
                ItemsNeedingCorrection = itemsNeedingCorrection,
                HoursSpent = hours,
                MinutesSpent = minutes,
                InspectorNames = inspectorNames
            };

            return task;
        }

        public static CvsSheet Deserialize(string filePath)
        {
            CvsSheet sheet = new CvsSheet();

            string text = File.ReadAllText(filePath);
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(text);

            var templateNode = xd.SelectSingleNode("formData/shana/template");
            sheet.FormDataVersion = xd.SelectSingleNode("formData/@version").Value;
            sheet.TemplateId = templateNode.Attributes["id"].Value;
            sheet.Revision = templateNode.Attributes["rev"].Value;
            sheet.Name = templateNode.Attributes["name"].Value;

            EstablishmentInfo establishmentInfo = new EstablishmentInfo();
            establishmentInfo.ID = xd.SelectSingleNode("/formData/instance/establishment_no").InnerText;
            establishmentInfo.Name = xd.SelectSingleNode("/formData/instance/establishment_name").InnerText;
            establishmentInfo.Address = xd.SelectSingleNode("/formData/instance/address1_lu").InnerText;
            establishmentInfo.PostalCode = xd.SelectSingleNode("/formData/instance/postal_code_lu").InnerText;
            establishmentInfo.City = xd.SelectSingleNode("/formData/instance/city_lu").InnerText;
            establishmentInfo.Province = xd.SelectSingleNode("/formData/instance/province_lu").InnerText;
            establishmentInfo.Country = xd.SelectSingleNode("/formData/instance/country_lu").InnerText;

            sheet.EstablishmentInfo = establishmentInfo;

            int weekNum;
            if (!int.TryParse(xd.SelectSingleNode("/formData/instance/week_num").InnerText, out weekNum)) { }
            else { sheet.WeekNumber = weekNum; }

            sheet.RevisionString = xd.SelectSingleNode("formData/instance/revision_no").InnerText;
            
            var taskNodes = xd.SelectNodes("formData/instance/table4");
            foreach (XmlNode node in taskNodes)
            {
                sheet.Tasks.Add(deserializeTask(node));
            }

            return sheet;
        }
    }
}
