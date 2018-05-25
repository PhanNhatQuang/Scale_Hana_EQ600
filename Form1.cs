using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Termie
{

    public partial class Form1 : Form
    {
        /// <summary>
        /// Class to keep track of string and color for lines in output window.
        /// </summary>
        private class Line
        {
            public string Str;
            public Color ForeColor;

            public Line(string str, Color color)
            {
                Str = str;
                ForeColor = color;
            }
        };

        ArrayList lines = new ArrayList();

        Font origFont;
        Font monoFont;
        private string m_old_value;
        public Form1()
        {
            InitializeComponent();

            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer2.FixedPanel = FixedPanel.Panel2;

          
            CancelButton = button4; //Close


            Settings.Read();
            TopMost = Settings.Option.StayOnTop;

            // let form use multiple fonts
            origFont = Font;
            FontFamily ff = new FontFamily("Courier New");
            monoFont = new Font(ff, 8, FontStyle.Regular);
            Font = Settings.Option.MonoFont ? monoFont : origFont;

            CommPort com = CommPort.Instance;
            com.StatusChanged += OnStatusChanged;
            com.DataReceived += OnDataReceived;
            com.Open();
        }

        // shutdown the worker thread when the form closes
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        /// <summary>
        /// output string to log file
        /// </summary>
        /// <param name="stringOut">string to output</param>
        public void logFile_writeLine(string stringOut)
        {
            if (Settings.Option.LogFileName != "")
            {
                Stream myStream = File.Open(Settings.Option.LogFileName,
                    FileMode.Append, FileAccess.Write, FileShare.Read);
                if (myStream != null)
                {
                    StreamWriter myWriter = new StreamWriter(myStream, Encoding.UTF8);
                    myWriter.WriteLine(stringOut);
                    myWriter.Close();
                }
            }
        }

        #region Output window

        Color receivedColor = Color.Green;
        Color sentColor = Color.Blue;


        #endregion

        #region Event handling - data received and status changed

        /// <summary>
        /// Prepare a string for output by converting non-printable characters.
        /// </summary>
        /// <param name="StringIn">input string to prepare.</param>
        /// <returns>output string.</returns>
        private String PrepareData(String StringIn)
        {
            // The names of the first 32 characters
            string[] charNames = { "NUL", "SOH", "STX", "ETX", "EOT",
                "ENQ", "ACK", "BEL", "BS", "TAB", "LF", "VT", "FF", "CR", "SO", "SI",
                "DLE", "DC1", "DC2", "DC3", "DC4", "NAK", "SYN", "ETB", "CAN", "EM", "SUB",
                "ESC", "FS", "GS", "RS", "US", "Space"};
            string StringOut = "";
            foreach (char c in StringIn)
            {
                if (Settings.Option.HexOutput)
                {
                    StringOut = StringOut + String.Format("{0:X2} ", (int)c);
                }
                else if (c < 32 && c != 9)
                {
                    //StringOut = StringOut + "<" + charNames[c] + ">";
                    if (c == 2)
                    {
                        StringOut = "";
                    }
                    if (c == 3)
                    {
                        if (StringOut.Length == 14)
                        {
                            StringOut = StringOut.Substring(4);
                            if (StringOut[7].Equals('B'))
                            {
                                StringOut = StringOut.Split('B')[0] + " KG";
                                StringOut = StringOut.Insert(1,".");
                            }
                            else if (StringOut[7].Equals('A'))
                            {
                                StringOut = StringOut.Split('A')[0] + " G";
                                StringOut = StringOut.Remove(0,1);
                                StringOut = StringOut.Insert(3, ".");
                            }
                            this.m_old_value = StringOut;
                            return StringOut;
                        }
                    }
                    //Uglier "Termite" style
                    //StringOut = StringOut + String.Format("[{0:X2}]", (int)c);
                }
                else
                {
                    StringOut = StringOut + c;
                }
            }
            return this.m_old_value;
        }

        /// <summary>
        /// Add data to the output.
        /// </summary>
        /// <param name="StringIn"></param>
        /// <returns></returns>
        private void AddData(String StringIn)
        {
            String StringOut = PrepareData(StringIn);

            // if we have a partial line, add to it.
            //if (partialLine != null)
            //{
            //	// tack it on
            //	partialLine.Str = partialLine.Str + StringOut;
            //	outputList_Update(partialLine);
            //	return partialLine;
            //}            

            System.Threading.Thread.Sleep(20);
            this.btn_value.Text = StringOut;
            this.btn_value.ForeColor = receivedColor;
        }

		// delegate used for Invoke
		internal delegate void StringDelegate(string data);

		/// <summary>
		/// Handle data received event from serial port.
		/// </summary>
		/// <param name="data">incoming data</param>
		public void OnDataReceived(string dataIn)
        {
            //Handle multi-threading
            if (InvokeRequired)
            {
                Invoke(new StringDelegate(OnDataReceived), new object[] { dataIn });
                return;
            }

            // if we detect a line terminator, add line to output
            int index;
			while (dataIn.Length > 0 &&
				((index = dataIn.IndexOf("\r")) != -1 ||
				(index = dataIn.IndexOf("\n")) != -1))
            {
				String StringIn = dataIn.Substring(0, index);
				dataIn = dataIn.Remove(0, index + 1);

				//logFile_writeLine(StringIn);
                AddData(StringIn);
            }

			// if we have data remaining, add a partial line
			if (dataIn.Length > 0)
			{
				AddData(dataIn);
			}
		}

		/// <summary>
		/// Update the connection status
		/// </summary>
		public void OnStatusChanged(string status)
		{
			//Handle multi-threading
			if (InvokeRequired)
			{
                Invoke(new StringDelegate(OnStatusChanged), new object[] { status });
				return;
			}

			textBox1.Text = status;
        }

		#endregion

		#region User interaction


		/// <summary>
		/// Show settings dialog
		/// </summary>
		private void button1_Click(object sender, EventArgs e)
		{
			TopMost = false;

			Form2 form2 = new Form2();
			form2.ShowDialog();

			TopMost = Settings.Option.StayOnTop;
			Font = Settings.Option.MonoFont ? monoFont : origFont;
		}


		/// <summary>
		/// Show about dialog
		/// </summary>
		private void button3_Click(object sender, EventArgs e)
		{
			TopMost = false;

			AboutBox about = new AboutBox();
			about.ShowDialog();

			TopMost = Settings.Option.StayOnTop;
		}

		/// <summary>
		/// Close the application
		/// </summary>
		private void button4_Click(object sender, EventArgs e)
		{
            this.Close();
		}

        /// <summary>
        /// If character 0-9 a-f A-F, then return hex digit value ?
        /// </summary>
        private static int GetHexDigit(char c)
        {
            if ('0' <= c && c <= '9') return (c-'0');
            if ('a' <= c && c <= 'f') return (c-'a')+10;
            if ('A' <= c && c <= 'F') return (c-'A')+10;
            return 0;
        }

        /// <summary>
        /// Parse states for ConvertEscapeSequences()
        /// </summary>
        public enum Expecting : byte
        {
            ANY = 1,
            ESCAPED_CHAR,
            HEX_1ST_DIGIT,
            HEX_2ND_DIGIT
        };

        /// <summary>
        /// Convert escape sequences
        /// </summary>
        private string ConvertEscapeSequences(string s)
        {
            Expecting expecting = Expecting.ANY;

            int hexNum = 0;
            string outs = "";
            foreach (char c in s)
            {
                switch (expecting)
                {
                    case Expecting.ANY:
                        if (c == '\\')
                            expecting = Expecting.ESCAPED_CHAR;
                        else
                            outs += c;
                        break;
                    case Expecting.ESCAPED_CHAR:
                        if (c == 'x')
                        {
                            expecting = Expecting.HEX_1ST_DIGIT;
                        }
                        else
                        {
                            char c2 = c;
                            switch (c)
                            {
                                case 'n': c2 = '\n'; break;
                                case 'r': c2 = '\r'; break;
                                case 't': c2 = '\t'; break;
                            }
                            outs += c2;
                            expecting = Expecting.ANY;
                        }
                        break;
                    case Expecting.HEX_1ST_DIGIT:
                        hexNum = GetHexDigit(c)*16;
                        expecting = Expecting.HEX_2ND_DIGIT;
                        break;
                    case Expecting.HEX_2ND_DIGIT:
                        hexNum += GetHexDigit(c);
                        outs += (char)hexNum;
                        expecting = Expecting.ANY;
                        break;
                }
            }
            return outs;
        }



        #endregion

        private void btn_save__Click(object sender, EventArgs e)
        {
            MessageBox.Show(this.btn_value.Text);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CommPort com = CommPort.Instance;
            if (com.IsOpen)
            {

                e.Cancel = true; //cancel the fom closing

                Thread CloseDown = new Thread(new ThreadStart(CloseSerialOnExit)); //close port in new thread to avoid hang

                CloseDown.Start(); //close port in new thread to avoid hang

            }
        }
        private void CloseSerialOnExit()
        {
            CommPort com = CommPort.Instance;
            try
            {
                com.Close(); //close the serial port
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); //catch any serial port closing error messages
            }
            this.Invoke(new EventHandler(NowClose)); //now close back in the main thread

        }

        private void NowClose(object sender, EventArgs e)
        {
            this.Close(); //now close the form
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //CommPort com = CommPort.Instance;
            //if (com.IsOpen)
            //{
            //    com.Close();
            //}
            //else
            //{
            //    com.Open();
            //}
        }
    }
}
