using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Threading;

using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Device.Gsms
{
    public class Commander : IDisposable
    {
        public event EventHandler DataReceived;
        public event EventHandler SendResult;

        private Clienter _Clienter = null;
        private Reader _Reader = null;
        private string _Sca = string.Empty;
        private IList<string> _Commands = new List<string>();
        private IList<object[]> _Argses = new List<object[]>();
        private bool _State = false;
        private Thread _ExecuteCommandThread = null;

        public Commander(Clienter clienter, Reader reader, string sca)
        {
            this._Clienter = clienter;
            this._Reader = reader;
            this._Sca = sca;

            this._State = true;
            this._ExecuteCommandThread = new Thread(new ThreadStart(this.ExecuteCommand));
            this._ExecuteCommandThread.Start();
        }

        public void AddCommand(string command, object[] args)
        {
            if (command == "ListMessages")
            {
                if (this._Commands.Count > 0 && this._Commands[this._Commands.Count - 1] == "ListMessages") return;
            }

            this._Commands.Add(command);
            this._Argses.Add(args);
        }

        private void ExecuteCommand()
        {
            while (this._State == true)
            {
                if (this._Commands.Count != 0)
                {
                    try
                    {
                        string command = this._Commands[0];
                        object[] args = this._Argses[0];

                        if (command == "SetFormat") this.SetFormat();
                        if (command == "ListMessages")
                        {
                            this.SetFormat();
                            this.ListMessages();
                        }
                        if (command == "DeleteMessage") this.DeleteMessage(args[0].ToInt32());
                        if (command == "SendMessage") this.SendMessage(args[0].ToString(), args[1].ToString(), args[2].ToBoolean());

                        this._Commands.RemoveAt(0);
                        this._Argses.RemoveAt(0);
                    }
                    catch (Exception ex)
                    {
                        ex = WriteExceptLog(ex);

                        Thread.Sleep(1000);
                    }
                }
                else Thread.Sleep(1000);
            }
        }

        private static Exception WriteExceptLog(Exception ex)
        {
            string msg = "";
            string traMessage = "";
            while (ex != null && ex.InnerException != null) ex = ex.InnerException;
            msg = ex.Message;
            traMessage = ex.StackTrace;
            DateTime nowDateTime = DateTime.Now;
            StringBuilder errBuild = new StringBuilder();
            errBuild.Append("#############################################################################################");
            errBuild.Append(Environment.NewLine);
            errBuild.Append(Environment.NewLine);
            errBuild.AppendFormat("时间：{0}", nowDateTime.ToString());
            errBuild.Append(Environment.NewLine);
            errBuild.AppendFormat("Message：{0}", msg);
            errBuild.Append(Environment.NewLine);
            errBuild.AppendFormat("StackTrace：{0}", traMessage);
            errBuild.Append(Environment.NewLine);
            errBuild.Append(Environment.NewLine);
            errBuild.Append("#############################################################################################");

            string filePath = string.Format("{0}\\Except_{1}.txt", System.Windows.Forms.Application.StartupPath, nowDateTime.ToString("yyyy_MM_dd"));
            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                sw.Write(errBuild);
                sw.Close();
            }
            return ex;
        }

        private void SetFormat()
        {
            this._Reader.Clear();

            this.SendCommand("+++");
            Thread.Sleep(1000);
            this.SendCommand("at");
            Thread.Sleep(1000);

            DateTime begin = DateTime.Now;
            while (this._State == true)
            {
                if (this.GetResult() == true) break;
                if ((DateTime.Now - begin).TotalMinutes > 1) break;

                Thread.Sleep(100);
            }

            Thread.Sleep(100);
        }

        private void ListMessages()
        {
            this._Reader.Clear();

            this.SendCommand("at+cmgl=4");
            Thread.Sleep(100);

            DateTime begin = DateTime.Now;
            while (this._State == true)
            {
                if (this.GetResult() == true) break;
                if ((DateTime.Now - begin).TotalMinutes > 1) break;

                Thread.Sleep(100);
            }

            Thread.Sleep(100);
        }

        private void DeleteMessage(int index)
        {
            this._Reader.Clear();

            this.SendCommand("at+cmgd=" + index.ToString());
            Thread.Sleep(100);

            DateTime begin = DateTime.Now;
            while (this._State == true)
            {
                if (this.GetResult() == true) break;
                if ((DateTime.Now - begin).TotalMinutes > 1) break;

                Thread.Sleep(100);
            }

            Thread.Sleep(100);
        }

        private void SendMessage(string mobile, string text, bool success)
        {
            string pdu = (new Encoder()).GetPdu(this._Sca, mobile, text);
            int length = (pdu.Length - Convert.ToInt32(pdu.Substring(0, 2), 16) * 2 - 2) / 2;
            int times = 0;

            while (this._State == true)
            {
                this._Reader.Clear();
                
                this.SendCommand("at+cmgs=" + length.ToString());
                Thread.Sleep(1000);
                this.SendPdu(pdu);
                Thread.Sleep(1000);

                DateTime begin = DateTime.Now;
                while (this._State == true)
                {
                    Nullable<bool> result = this.GetResult();

                    if (result == true)
                    {
                        if (success == true && this.SendResult != null) this.SendResult(true, new EventArgs());

                        times = 3;
                        break;
                    }
                    if (result == false)
                    {
                        times = times + 1;
                        if (times == 3)
                        {
                            if (success == true && this.SendResult != null) this.SendResult(false, new EventArgs());
                        }

                        break;
                    }

                    if ((DateTime.Now - begin).TotalMinutes > 1) break;

                    Thread.Sleep(100);
                }

                if (times == 3) break;
            }

            Thread.Sleep(100);
        }

        private void SendCommand(string command)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(command + "\r");

            this._Clienter.Write(bytes, 0, bytes.Length);
        }

        private void SendPdu(string pdu)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pdu + (char)26);

            this._Clienter.Write(bytes, 0, bytes.Length);
        }

        private Nullable<bool> GetResult()
        {
            Nullable<bool> result = this._Reader.GetResult();
            Dictionary<int, string> values = this._Reader.GetValues();

            if (result == true && values.Count != 0)
            {
                if (this.DataReceived != null) this.DataReceived(values, new EventArgs());
            }

            return result;
        }

        public void Dispose()
        {
            this._State = false;

            try
            {
                this._ExecuteCommandThread.Abort();
            }
            catch { }

            this._Clienter.Dispose();
            this._Reader.Dispose();
        }
    }
}
