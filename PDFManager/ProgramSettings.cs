using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PDFManager
{
    public class ProgramSettings
    {
        /// <summary>
        /// A private instance of the settings document
        /// </summary>
        private static XmlDocument _settingsDocument = null;

        /// <summary>
        /// The XML document containing settings
        /// </summary>
        private static XmlDocument SettingsDocument
        {
            get
            {
                if (_settingsDocument == null)
                {
                    
                    _settingsDocument = new XmlDocument();
                    _settingsDocument.Load(SettingsDocumentPath);
                }

                return _settingsDocument;
            }
        }


        /// <summary>
        /// The path to the settings xml document
        /// </summary>
        protected static string SettingsDocumentPath
        {
            get
            {
                string _path = Path.Combine(RootFolder, "Settings.xml");
                return _path;
            }
        }
        

        /// <summary>
        /// Gets or sets the last used folder
        /// </summary>
        public static string LastFolder
        {
            get
            {
                try
                {
                    return SettingsDocument.DocumentElement.Attributes["lastFolder"].InnerText;
                }
                catch
                {
                    return "C:\\";
                }
            }
            set
            {
                try
                {
                    SettingsDocument.DocumentElement.Attributes["lastFolder"].InnerText = value;
                    SettingsDocument.Save(SettingsDocumentPath);
                }
                catch
                {

                }
            }
        }


        /// <summary>
        /// Gets the root folder of an executing .Net application (equivalent to Server.MapPath("~/") in ASP.NET)
        /// </summary>
        protected static string RootFolder
        {
            get
            {
                string exeFolder = Directory.GetCurrentDirectory();
                string[] parts = exeFolder.Split(Path.DirectorySeparatorChar);
                if (exeFolder.ToLower().Contains(@"\bin\debug") || exeFolder.ToLower().Contains(@"\bin\release"))
                    return string.Join("\\", parts.Take(parts.Length - 2));
                else
                    return exeFolder;
                
            }
        }

    }


    
}
