using System;

namespace LexAZ
{
    public class FileReader
    {
        public string ReadFile(string fileName)
        {/*     In this updated version of the ReadFile() method, the current directory and the parent 
      *     directory three levels higher, plus 4 levels higher, are first obtained using the Directory.GetCurrentDirectory() 
      *     and Directory.GetParent() methods. Then the File.ReadAllText method is used to try reading 
      *     the file with the passed filename, if the file is not found it throws a FileNotFoundException 
      *     which is caught by the catch block. The catch block then tries to add .txt to the end of the 
      *     file name and search again. If the file is still not found it tries to search in the parent 
      *     folder three folders higher up with both versions of the name before giving up and giving the 
      *     not found message.  */

            string currentDirectory = Directory.GetCurrentDirectory();            
            string parentDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
            string doubleParentDirectory = Directory.GetParent(currentDirectory).Parent.Parent.Parent.FullName;
            Console.WriteLine("The following directories will be searched:\n" +
                " '" + currentDirectory + "'\n '" + parentDirectory + "'\n '" + doubleParentDirectory + "'\n");
            try
            {
                string fileContent = File.ReadAllText(fileName);
                return fileContent;
            }
            catch (FileNotFoundException)
            {
                try
                {
                    string fileContent = File.ReadAllText(fileName + ".txt");
                    return fileContent;
                }
                catch (FileNotFoundException)
                {
                    try
                    {
                        string fileContent = File.ReadAllText(Path.Combine(parentDirectory, fileName));
                        return fileContent;
                    }
                    catch (FileNotFoundException)
                    {
                        try
                        {
                            string fileContent = File.ReadAllText(Path.Combine(parentDirectory, fileName + ".txt"));
                            return fileContent;
                        }
                        catch (FileNotFoundException)
                        {
                            try
                            {
                                string fileContent = File.ReadAllText(Path.Combine(currentDirectory, "Morse1", fileName));
                                return fileContent;
                            }
                            catch (FileNotFoundException)
                            {
                                try
                                {
                                    string fileContent = File.ReadAllText(Path.Combine(currentDirectory, "Morse1", fileName + ".txt"));
                                    return fileContent;
                                }
                                catch (FileNotFoundException)
                                {
                                    try
                                    {
                                        string fileContent = File.ReadAllText(Path.Combine(doubleParentDirectory, fileName));
                                        return fileContent;
                                    }
                                    catch (FileNotFoundException)
                                    {
                                        try
                                        {
                                            string fileContent = File.ReadAllText(Path.Combine(doubleParentDirectory, fileName + ".txt"));
                                            return fileContent;
                                        }
                                        catch (FileNotFoundException)
                                        {
                                            Console.WriteLine("File not found: " + fileName);
                                            return string.Empty;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
