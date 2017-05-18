using System;

using System.IO;

namespace Wodeyun.Gf.Tools.SvcFixer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1) throw new ArgumentException();
                if (File.Exists(args[0]) == false) throw new FileNotFoundException();

                string code = File.ReadAllText(args[0]);
                string old = "[System.ServiceModel.OperationContractAttribute";
                string updated = "[System.ServiceModel.Web.WebInvoke(BodyStyle = System.ServiceModel.Web.WebMessageBodyStyle.Wrapped)]" + Environment.NewLine + string.Empty.PadLeft(8) + old;

                code = code.Replace(old, updated);

                File.WriteAllText(args[0], code);
            }
            catch (ArgumentException)
            {
                Console.Write("Please specify a *.cs filename.");
            }
            catch (FileNotFoundException)
            {
                Console.Write("File is not found: " + args[0]);
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
            }
        }
    }
}
