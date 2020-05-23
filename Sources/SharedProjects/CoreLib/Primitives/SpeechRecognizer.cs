using System;
using System.Globalization;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using Mohammad.Helpers.Console;

namespace Mohammad.Primitives
{
    public static class SpeechRecognizer
    {
        public static string Recognize()
        {
            string result =null;
            using (var recognizer = new SpeechRecognitionEngine(new CultureInfo("en-US")))
            using (var speech = new SpeechSynthesizer())
            {
                recognizer.LoadGrammar(new DictationGrammar());
                recognizer.SpeechRecognized += async (sender, e) =>
                {
                    result= e.Result.Text;
                    await Task.Run(() => speech.Speak(result));
                };
                recognizer.AudioStateChanged += (_, e) => e.AudioState.WriteLine();
                recognizer.EmulateRecognizeCompleted += (_, e) => e.Result.WriteLine();
                recognizer.SetInputToDefaultAudioDevice();
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
            return result;
        }
    }
}