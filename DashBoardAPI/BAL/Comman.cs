using System;
using System.Collections.Generic;
using System.Xml;

namespace DashBoardModel.BAL
{
    public static class Comman
    {
        public static List<Param> ReadXml(string xml)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);

            //XmlElement root = xdoc.DocumentElement;

            XmlNodeList Xparam = xdoc.GetElementsByTagName("Param");
            List<Param> listParam = new List<Param>();

            foreach (XmlNode n in Xparam)
            {
                if (n is XmlElement)
                {
                    listParam.Add(new Param
                    {
                        Control = (n as XmlElement).GetAttribute("Control").ToString(),
                        Label = (n as XmlElement).GetAttribute("Label").ToString(),
                        Label1 = (n as XmlElement).GetAttribute("Label1").ToString(),
                        Type = (n as XmlElement).GetAttribute("Type").ToString(),
                        Sp = (n as XmlElement).GetAttribute("Sp").ToString(),
                        XValue = (n as XmlElement).GetAttribute("XValue").ToString(),
                        YValue = (n as XmlElement).GetAttribute("YValue").ToString(),
                        YValue1 = (n as XmlElement).GetAttribute("YValue1").ToString(),
                        YValue2 = (n as XmlElement).GetAttribute("YValue2").ToString(),
                        YValue3 = (n as XmlElement).GetAttribute("YValue3").ToString(),
                        YValue4 = (n as XmlElement).GetAttribute("YValue4").ToString(),

                        ddlData = (n as XmlElement).GetAttribute("ddlData").ToString(),
                        ddlValue = (n as XmlElement).GetAttribute("ddlValue").ToString(),
                        Multiple = (n as XmlElement).GetAttribute("Multiple").ToString(),
                        PlaceHolder = (n as XmlElement).GetAttribute("PlaceHolder").ToString(),
                        Position = (n as XmlElement).GetAttribute("Position").ToString(),
                        YearRange = (n as XmlElement).GetAttribute("YearRange").ToString(),
                        QueryOrSp = (n as XmlElement).GetAttribute("QueryOrSp").ToString(),
                        IsSp = Convert.ToInt32((n as XmlElement).GetAttribute("IsSp").ToString()),
                    });
                }
            }

            return listParam;
        }
    }
}

