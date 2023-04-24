using System;
using System.IO;

namespace HellsysLibrary.Helpers
{
    public class RootHelper
    {
        public string RootDirectory { get => AppDomain.CurrentDomain.BaseDirectory; }

        /// <summary>
        /// 프로젝트 경로 + 폴더
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public string RootFoler(string folderName)
        {
            var result = Path.Combine(RootDirectory, folderName);
            return result;
        }
        /// <summary>
        /// 프로젝트 경로 + 파일
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string RootFile(string fileName)
        {
            var result = Path.Combine(RootDirectory, fileName);
            return result;
        }
        /// <summary>
        /// 프로젝트 경로 + 폴더 + 파일
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string RootFile(string folderName, string fileName)
        {
            var result = Path.Combine(RootDirectory, folderName, fileName);
            return result;
        }
        /// <summary>
        /// 폴더 + 파일명
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CustomPath(string folderName, string fileName)
        {
            var result = Path.Combine(RootDirectory, folderName, fileName);
            return result;
        }
        /// <summary>
        /// 프로젝트경로에 폴더생성
        /// </summary>
        /// <param name="folderName"></param>
        public void createFolder(string folderName)
        {
            DirectoryInfo directory = new DirectoryInfo(RootFoler(folderName));
            if (!directory.Exists) directory.Create();
        }
        /// <summary>
        /// 프로젝트에 생성된 폴더 경로에 Json 파일 생성
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        public void createJsonFile(string folderName, string fileName)
        {
            FileInfo file = new FileInfo(RootFile(folderName, fileName));
            if (!file.Exists) file.Create();
        }
    }
}
