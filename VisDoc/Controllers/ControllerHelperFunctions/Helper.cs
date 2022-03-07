using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VisDoc.Models;
using System.Data.SqlClient;
using VisDoc.Data;

namespace VisDoc.ControllerHelperFunctions
{
    public class Helper
    {
        private static List<string> dictNameIntCout(string tempFileName)
        {
            if (tempFileName[tempFileName.Length -1] == ')')
            {
                for (int character = tempFileName.Length - 2; character == 0; character--)
                {
                    if (tempFileName[character] == '(')
                    {
                        int subStringLength = (tempFileName.Length -1) - character;
                        string copyItrCout = tempFileName.Substring(character+1, subStringLength);
                        string nameLikeNamePure = tempFileName.Substring(0, subStringLength - 1);
                        return new List<string>(){ nameLikeNamePure, copyItrCout };
                    }
                }
            }
            return null;
        }
        public static string newName(List<DocumentModel> nameLikeList, IFormFile file)
        {
            List<int> copyList = new List<int>(); 
            string clientFileName = file.Name;
            foreach (var nameLike in nameLikeList)
            {
                string serverFileName = nameLike.Name;

                if (clientFileName == serverFileName) { copyList.Add(0); }


                var cliDict = Helper.dictNameIntCout(clientFileName);
                var srvDict = Helper.dictNameIntCout(serverFileName);

                if(cliDict == null || srvDict == null) { continue; }
                if(cliDict[0] == srvDict[0])
                {
                    copyList.Add(Int32.Parse(srvDict[1]));
                }
            }
            if (copyList.Count() == 0) { return file.FileName; }
            return String.Format(file.FileName+"({0})",copyList[copyList.Count()-1] + 1);
        }
    }
}
