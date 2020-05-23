using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;

namespace TestConsole
{
    public class T4Enine : ITextTemplatingEngineHost
    {
        protected string TemplateFileValue;
        private Encoding _FileEncodingValue = Encoding.UTF8;
        private string _FileExtensionValue = ".txt";
        protected string FileExtension
        {
            get { return this._FileExtensionValue; }
        }
        protected Encoding FileEncoding
        {
            get { return this._FileEncodingValue; }
        }
        protected CompilerErrorCollection Errors { get; private set; }

        #region ITextTemplatingEngineHost Members
        public string TemplateFile
        {
            get { return this.TemplateFileValue; }
        }
        public IList<string> StandardAssemblyReferences
        {
            get { return new[] {typeof (Uri).Assembly.Location}; }
        }
        public IList<string> StandardImports
        {
            get { return new[] {"System"}; }
        }

        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            content = String.Empty;
            location = String.Empty;

            if (!File.Exists(requestFileName))
                return false;
            content = File.ReadAllText(requestFileName);
            return true;
        }

        public object GetHostOption(string optionName)
        {
            object returnObject;
            switch (optionName)
            {
                case "CacheAssemblies":
                    returnObject = true;
                    break;
                default:
                    returnObject = null;
                    break;
            }
            return returnObject;
        }

        public string ResolveAssemblyReference(string assemblyReference)
        {
            if (File.Exists(assemblyReference))
                return assemblyReference;
            string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), assemblyReference);
            return File.Exists(candidate) ? candidate : "";
        }

        public Type ResolveDirectiveProcessor(string processorName)
        {
            if (string.Compare(processorName, "XYZ", StringComparison.OrdinalIgnoreCase) == 0)
            {
                //return typeof();
            }
            throw new Exception("Directive Processor not found");
        }

        public string ResolvePath(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");
            if (File.Exists(fileName))
                return fileName;
            string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), fileName);
            return File.Exists(candidate) ? candidate : fileName;
        }

        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            if (directiveId == null)
                throw new ArgumentNullException("directiveId");
            if (processorName == null)
                throw new ArgumentNullException("processorName");
            if (parameterName == null)
                throw new ArgumentNullException("parameterName");
            return String.Empty;
        }

        public void SetFileExtension(string extension)
        {
            this._FileExtensionValue = extension;
        }

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            this._FileEncodingValue = encoding;
        }

        public void LogErrors(CompilerErrorCollection errors)
        {
            this.Errors = errors;
        }

        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            return AppDomain.CreateDomain("Generation App Domain");
        }
        #endregion

        public static void ProcessTemplate(string templateFileName)
        {
            if (templateFileName == null)
                throw new ArgumentNullException("templateFileName");
            if (!File.Exists(templateFileName))
                throw new FileNotFoundException("the file cannot be found");
            var host = new T4Enine();
            var engine = new Engine();
            host.TemplateFileValue = templateFileName;
            string input = File.ReadAllText(templateFileName);
            string output = engine.ProcessTemplate(input, host);
            string outputFileName = Path.GetFileNameWithoutExtension(templateFileName);
            outputFileName = Path.Combine(Path.GetDirectoryName(templateFileName), outputFileName);
            outputFileName = outputFileName + "1" + host.FileExtension;
            File.WriteAllText(outputFileName, output, host.FileEncoding);

            foreach (CompilerError error in host.Errors)
                Console.WriteLine(error.ToString());
        }
    }
}