using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.IO;

namespace QWLadderAspWebApp
{
    public class IRCClient : IDisposable
    {
        private string host;
        private int port;

        private System.Net.Sockets.TcpClient socket;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~IRCClient() 
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing) 
            {
                // free managed resources
                if (socket != null) { socket.Close(); socket = null; }
                if (stream != null) { stream.Close(); stream = null; }
                if (reader != null) { reader.Close(); reader = null; }
                if (writer != null) { writer.Close(); writer = null; }
            }
            // free native resources if there are any.
            /*
            if (nativeResource != IntPtr.Zero) 
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
            */
        }

        public IRCClient(string newHost, int newPort)
        {
            this.host = newHost;
            this.port = newPort;
        }

        private void Write(string msg) //, NetworkStream serverStream)
        {
            //byte[] outStream = System.Text.Encoding.ASCII.GetBytes(msg);
            //serverStream.Write(outStream, 0, outStream.Length);
            //serverStream.Flush();
            System.Diagnostics.Debug.WriteLine("SEND: " + msg);
            this.writer.WriteLine(msg + "\r\n");
            this.writer.Flush();
        }

        private void LogEvent(string eventMessage, int style = 0)
        {
            /*if (style == 0)
                this.loggingControl.AppendText(eventMessage + "\r\n");
            else
            {
                //this.loggingControl.SelectionStart = IRClines.TextLength;
                //this.loggingControl.SelectionLength = 0;
                this.loggingControl.AppendText(eventMessage + "\r\n");
            }*/
            System.Diagnostics.Debug.WriteLine(eventMessage);
        }

        private int GetSeconds()
        {
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            return (int)t.TotalSeconds;
        }

        private string LoginWithQuakenet(string username, string password)
        {
            string guestNickname = "QWLadder" + RandomString(6);
            int startTime = GetSeconds();

            Write("NICK " + guestNickname);
            Write("PASS *");
            Write("USER guest 8 * :\"" + guestNickname + "\"");

            try
            {
                while (this.socket != null && this.socket.Connected
                    && startTime + 15 > GetSeconds())
                {
                    var line = this.reader.ReadLine();
                    if (line == null)
                        break;
                    LogEvent(line);
                    
                    if (line.StartsWith("PING"))
                    {
                        Write(line.Replace("PING", "PONG"));
                    }
                    if (line.Contains("001 " + guestNickname))
                    {
                        Write("PRIVMSG Q@CServe.quakenet.org :AUTH " + username + " " + password);
                    }
                    if (line.ToLower().Contains("username") && line.ToLower().Contains("password") && line.ToLower().Contains("incorrect"))
                    {
                        // todo: cleanly close thread here (and elsewhere? - or maybe give the thread a ttl of, say, 10 seconds?)
                        Write("QUIT");
                        return "Login incorrect";

                        //this.callback("Login failed");
                    }

                    if (line.Trim() == ":Q!TheQBot@CServe.quakenet.org NOTICE " + guestNickname
                        + " :You are now logged in as " + username + ".")
                    {
                        //this.callback("Logged in");
                        //Console.WriteLine("Logged in");
                        Write("QUIT");
                        return "Logged in";
                    }
                }
                if (!this.socket.Connected)
                {
                    this.socket.Close();
                    this.stream.Close();
                    //Console.WriteLine("Disconnected");
                    return "Disconnected";
                }
                /*                
                            IRCWrite(authUsername.Text, serverStream);

                            Console.WriteLine(IRCRead(serverStream));

                            string cmd = "/msg Q@CServe.quakenet.org AUTH " + authUsername.Text + " " + authPassword.Password;
                            IRCWrite(cmd, serverStream);
                            Console.WriteLine(cmd);
                            Console.WriteLine(IRCRead(serverStream));

                            clientSocket.Close();
                            serverStream.Close();*/
            }
            catch (Exception)
            {
            }
            return "Failure";
        }

        private string RandomString(int size)
        {
            Random rand = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = chars[rand.Next(chars.Length)];
            }
            return new string(buffer);
        }
        /*Try to remember login and password - none found - show login page
Try to login automatically to IRC - no success - show alert, and show login page
when login is executed, two things are saved:
* checkbox "login auto"
* login/pass info
*/

        public string Login(string username, string password)
        {
            /*
            // more info about settings here: http://msdn.microsoft.com/en-us/library/aa730869%28VS.80%29.aspx

            Properties.Settings.Default.Login = authUsername.Text;
            Properties.Settings.Default.Password = authPassword.Password;
            Properties.Settings.Default.AutoLogin = (bool)autoLogin.IsChecked;
            Properties.Settings.Default.Save();

            tabLadders.IsEnabled = true;
            tabMapPref.IsEnabled = true;
            tabLogin.Header = "Logged in as " + authUsername.Text.Trim();
            tabLadders.Focus();*/

            this.socket = new System.Net.Sockets.TcpClient();
            this.stream = default(NetworkStream);

            this.socket.Connect(this.host, this.port);
            this.stream = this.socket.GetStream();

            this.writer = new StreamWriter(this.stream, Encoding.Default);
            this.reader = new StreamReader(this.stream, Encoding.Default);

            //new Thread(ClientLoop).Start();
            //ClientLoop(); // figure out threads, in particular how to enable buttons after call stack:
            // external code -> calls
            // ClientLoop ->
            // LoginCallback (wpf controls not easily accessible?)
            username = username.Replace("@qwladder.azurewebsites.net", "");
            return LoginWithQuakenet(username, password);
        }
    }
}
