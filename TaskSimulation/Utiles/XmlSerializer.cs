using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace TaskSimulation.Utiles
{
    public class XmlUtils
    {
        public static void Save<T>(string fileName, T tObject)
        {
            try
            {
                using (var streamWriter = new StreamWriter(fileName))
                {
                    var xmlSerializer = new XmlSerializer(tObject.GetType());
                    xmlSerializer.Serialize(streamWriter, tObject);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch (Exception ex) when (ex is IOException)
            {
                Log.Err($"File not found in the given path: {fileName}.\n");
            }
            catch (Exception ex) when (ex is InvalidOperationException)
            {
                Log.Err($"File {fileName} is in wrong format, failed to deserialize.\n");
            }
            catch (Exception)
            {
                Log.Err($"Something whent wrong with loading the file, please verify the file.\n");
            }
        }

        public static T Load<T>(string fileName)
        {
            var result = default(T);
            var fullPath = fileName;
            try
            {
                if (!File.Exists(fullPath))
                {
                    var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    if (currentDirectory == null) throw new Exception();
                    fullPath = Path.Combine(currentDirectory, fileName);
                    if (!File.Exists(fullPath)) throw new FileNotFoundException();
                }
                using (var streamReader = File.OpenText(fullPath))
                {
                    var xmlSerializer = new XmlSerializer(typeof (T));
                    result = (T) xmlSerializer.Deserialize(streamReader);
                }

            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                Log.Err($"File not found in the given path: {fileName}.\n");
            }
            catch (Exception ex) when (ex is InvalidOperationException)
            {
                Log.Err($"File {fileName} is in wrong format, failed to deserialize.\n");
            }
            catch (Exception)
            {
                Log.Err($"Something whent wrong with loading the file, please verify the file.\n");
            }


            return result;
        }

        public static void AddXmlEntryDocumentation(string filePath, string xmlEntry, string comment)
        {
            if (File.Exists(filePath))
            {
                var commentStr = $"<!-- {comment} -->";
                var lines = File.ReadAllLines(filePath);

                for (int i = 0; i < lines.Length - 1; i++)
                {
                    // if line Contains xmlEntry exactly on time
                    if (lines[i].ToLower().Contains(comment.ToLower())) break;

                    if (lines[i].ToLower().Contains(xmlEntry.ToLower()))
                    {
                        lines[i] += commentStr;
                        break;
                    }

                }

                using (var sr = new StreamWriter(filePath))
                {
                    try
                    {
                        foreach (string line in lines)
                        {
                            sr.WriteLine(line);
                        }
                        sr.Flush();
                    }
                    finally 
                    {
                        sr?.Close();
                    }
                }
            }
        }
    }
}
