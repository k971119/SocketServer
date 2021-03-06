﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace XMLCommand
{
    public class Command
    {
        public string _sPath = string.Empty;

        /// <summary>
        /// 2021-01-13<br></br>
        /// 고한열<br></br><br></br>
        /// 클래스 선언 및 xml파일 유무 확인후 파일 생성<br></br>
        /// <br></br><br></br>
        /// </summary>
        public Command(string path, string xmlFile,string Element)
        {
            _sPath = path + "\\" + xmlFile;

            if (!FileCheck(_sPath))
            {
                Console.WriteLine("참조파일을 생성합니다.");
                XmlTextWriter textWriter = new XmlTextWriter(_sPath,Encoding.UTF8);
                textWriter.Formatting = Formatting.Indented;

                textWriter.WriteStartDocument();
                textWriter.WriteStartElement(Element);

                textWriter.WriteEndDocument();

                textWriter.Close();
            }

        }

        /// <summary>
        /// 2021-01-13<br></br>
        /// 고한열<br></br><br></br>
        /// xml파일 유무 체크
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool FileCheck(string path)
        {
            return System.IO.File.Exists(path);
        }

        /// <summary>
        /// 2021-01-13<br></br>
        /// 고한열<br></br><br></br>
        /// XML 노드 읽기
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public string Read(string nodeName)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(_sPath);

            XmlElement root = xml.DocumentElement;

            XmlNodeList nodes = root.ChildNodes;
            foreach(XmlNode node in nodes)
            {
                if (node.Name == nodeName)
                {
                    return node.InnerText;
                }
            }
            return "";
        }

        /// <summary>
        /// 2021-01-16<br></br>
        /// 고한열<br></br><br></br>
        /// Element 지정 XML 노드 읽기
        /// </summary>
        /// <param name="element"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public string Read(string element, string nodeName)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(_sPath);

            XmlElement root = xml.GetElementById(element);

            XmlNodeList nodes = xml.GetElementsByTagName(element);//root.ChildNodes;
            foreach (XmlNode node in nodes)
            {
                if (node.Name == nodeName)
                {
                    return node.InnerText;
                }
            }
            return "";
        }

        /// <summary>
        /// 2021-01-13<br></br>
        /// 고한열<br></br><br></br>
        /// XML 노드 생성
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Write(string name, string value)
        {
            Debug.WriteLine("xml 추가");
            XmlDocument xml = new XmlDocument();
            xml.Load(_sPath);

            XmlElement root = xml.DocumentElement;

            XmlNode node = xml.CreateElement(string.Empty, name, string.Empty);
            node.InnerXml = value;
            root.AppendChild(node);
            xml.Save(_sPath);

        }

        /// <summary>
        /// 2021-01-16<br></br>
        /// 고한열<br></br><br></br>
        /// Element 지정 XML 노드 생성
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Write(string element, string name, string value)
        {
            Debug.WriteLine("xml 추가");
            XmlDocument xml = new XmlDocument();
            xml.Load(_sPath);

            XmlElement root = xml.GetElementById(element);

            XmlNode node = xml.CreateElement(string.Empty, name, string.Empty);
            node.InnerXml = value;
            root.AppendChild(node);
            xml.Save(_sPath);

        }

        /// <summary>
        /// 2021-01-13<br></br>
        /// 고한열<br></br>
        /// xml 수정
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Modify(string name, string value)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(_sPath);

            XmlElement root = xml.DocumentElement;

            XmlNode node = root.SelectSingleNode(name);
            node.InnerText = value;
            xml.Save(_sPath);
        }
    }
}
