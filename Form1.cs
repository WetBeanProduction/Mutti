using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using System.Diagnostics;

namespace Mutti
{
    public partial class Form1 : Form
    {
        //Makes a string from files, to code the commands and response from the bot
        string[] grammarFile = (File.ReadAllLines(@"C:\Users\crazy\OneDrive\Skrivebord\Programmering\Mutti\bin\Debug\grammar.txt.txt"));
        string[] responseFile = (File.ReadAllLines(@"C:\Users\crazy\OneDrive\Skrivebord\Programmering\Mutti\bin\Debug\response.txt.txt"));

        //speech synthesis
        SpeechSynthesizer speech = new SpeechSynthesizer();
        //speech recognition
        Choices grammerList = new Choices();
        SpeechRecognitionEngine speechRec = new SpeechRecognitionEngine();

        public Form1()
        {
            //init Grammar
            grammerList.Add(grammarFile);
            Grammar grammar = new Grammar(new GrammarBuilder(grammerList));
            //try loop if something goes wrong
            try
            {
                speechRec.RequestRecognizerUpdate();
                speechRec.LoadGrammar(grammar);
                speechRec.SpeechRecognized += Rec_SpeechRecognized;
                speechRec.SetInputToDefaultAudioDevice();
                speechRec.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch { return; }

            //custom speech synthesizer
            speech.SelectVoiceByHints(VoiceGender.Female);

            InitializeComponent();
        }


        //with this method we make the bot talk
        private void Say(String text)
        {
            speech.SpeakAsync(text);

        }

        //this method gives the bot a way of accessing the files, and making sure it is the correct line of time 
        private void Rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string result = e.Result.Text;
            int resp = Array.IndexOf(grammarFile, result);

            if(responseFile[resp].IndexOf('+') == 0)
            {
                List<string> responses = responseFile[resp].Replace('+', ' ').Split('/').Reverse().ToList();
                Random r = new Random();
                Say(responses[r.Next(responses.Count)]);
            }

            else
            {
                
                if (responseFile[resp].IndexOf('*') == 0)
                {
                    if (result.Contains("google"))
                    {
                        Process.Start("Http://www.google.com");
                    }

                    else if (result.Contains("teams"))
                    {
                        Process.Start("C:\\Users\\crazy\\AppData\\Local\\Microsoft\\Teams\\Teams.exe");
                    }

                    else if (result.Contains("time"))
                    {
                        Say(DateTime.Now.ToString(@"hh\:mm tt"));
                    }

                    else
                    {
                        Say(responseFile[resp]);
                    }

                    
                }
               
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
