using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UMA;

namespace UMAEditor
{
	public static class UMADNAHelperTools {
	    static DictionaryCustomFormatter customFormatter;
	    class Template
	    {
	        public String Format;
	        public StringBuilder sb;
	        public String Name;
	        public static IFormatProvider formatter;
	        public void Append(IDictionary<string, object> data)
	        {
	            sb.AppendFormat(formatter, Format, data);
	        }
	    }


	    [MenuItem("UMA/Create DNA Helper Code")]
		static void CreateDNAHelperCode() 
	    {
	        var destDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets/UMA/UMA_Generated/DnaHelpers");
	        var sourceDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets/UMA/Editor/Templates");
	        var baseTemplate = File.ReadAllText(Path.Combine(sourceDir,"UmaDna_Template.cs.txt"));
	        var pageTemplate = File.ReadAllText(Path.Combine(sourceDir, "UmaDnaChild_Template.cs.txt"));

	        var templates = ParseTemplates(sourceDir, baseTemplate);
	        var pageTemplates = ParseTemplates(sourceDir, pageTemplate);

	        customFormatter = new DictionaryCustomFormatter();
	        Template.formatter = customFormatter;

	        if (!Directory.Exists(destDir))
	        {
	            Debug.Log("Creating Directory: " + destDir);
	            Directory.CreateDirectory(destDir);
	        }

            var baseDnaType = typeof(UMADna);
	        var customData = new Dictionary<string, object>();
	        customData.Add("ClassName", "");
	        
	        foreach(var dnaType in baseDnaType.Assembly.GetTypes())
	        {
	            if ( DerivesFrom(dnaType, baseDnaType))
	            {
	                customData["ClassName"] = dnaType.Name;
	                foreach (var template in templates)
	                {
	                    template.Append(customData);
	                }
	                CreateDNAHelperCode(dnaType, destDir, pageTemplate, pageTemplates);
	            }
	        }

	        foreach (var template in templates)
	        {
	            customData.Add(template.Name, template.sb);
	        }

	        CreateBaseDNAExtension(destDir, baseTemplate, customData);
	        AssetDatabase.Refresh();	    
		}

        private static bool DerivesFrom(Type type, Type baseType)
        {
            Type parent = type.BaseType;
            while (parent != null)
            {
                if (parent == baseType) return true;
                parent = parent.BaseType;
            }
            return false;
        }

	    private static Template[] ParseTemplates(string sourceDir, string pageTemplate)
	    {
	        List<Template> res = new List<Template>();
	        foreach (var line in pageTemplate.Split('\r', '\n'))
	        {
	            if (line.StartsWith("//#TEMPLATE "))
	            {
	                var parsedLine = line.Split(' ');
	                if (parsedLine.Length == 3)
	                {
	                    var filename = Path.Combine(sourceDir, parsedLine[2]);
	                    if (!File.Exists(filename))
	                    {
	                        Debug.LogError("File not found: " + filename);
	                    }
	                    res.Add(new Template() { Format = File.ReadAllText(filename), sb = new StringBuilder(), Name = parsedLine[1] });
	                }
	            }
	        }
	        return res.ToArray();
	    }

	    private static void CreateBaseDNAExtension(string destination, string formatString, Dictionary<string, object> customData)
	    {
	        File.WriteAllText(Path.Combine(destination, "UMADna_Generated.cs"), String.Format(customFormatter, formatString, customData));
	    }

	    private static void CreateDNAHelperCode(Type dnaType, string destination, string formatString, Template[] templates)
	    {
	        var customData = new Dictionary<string, object>();
	        customData.Add("ClassName", dnaType.Name);
	        customData.Add("FieldName","");
	        customData.Add("Index",0);

	        int index = 0;
	        var fields = dnaType.GetFields();
	        customData.Add("DnaEntries", fields.Length);
	        foreach (var field in fields)
	        {
	            customData["FieldName"] = field.Name;
	            customData["Index"] = index;
	            foreach (var template in templates)
	            {
	                template.Append(customData);
	            }
	            index++;
	        }
	        foreach (var template in templates)
	        {
	            customData.Add(template.Name, template.sb);
	        }
	        File.WriteAllText(Path.Combine(destination, dnaType.Name + "_Generated.cs"), String.Format(customFormatter, formatString, customData));
	    }
	}
}
