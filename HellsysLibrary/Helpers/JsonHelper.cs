using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Documents;

namespace HellsysLibrary.Helpers
{
    public class JsonHelper
    {
        /// <summary>
        /// 리스트를 Json 파일로 저장
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="jsonFileName"></param>
        /// <returns></returns>
        public void SaveToJsonFile<T>(List<T> list, string jsonFileName)
        {
            string jsonString = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(jsonFileName, jsonString);
        }
        /// <summary>
        /// Json파일을 열어서 List로 반환
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonFileName"></param>
        /// <returns></returns>
        public List<T> GetJsonFileList<T>(string jsonFileName)
        {
            var list = new List<T>();
            using(StreamReader file = File.OpenText(jsonFileName))
            {
                var serializer = new JsonSerializer();
                list = (List<T>)serializer.Deserialize(file, typeof(List<T>));
            }
            return list;
        }
        public List<T> LoadJson<T>(T item, string jsonFileName)
        {
            if (File.Exists(jsonFileName))
            {
                var list = new List<T>();
                using(StreamReader file= File.OpenText(jsonFileName))
                {
                    var serializer = new JsonSerializer();
                    list = (List<T>)serializer.Deserialize(file, typeof(List<T>));
                }
            }
            return null;
        }
    }
}
