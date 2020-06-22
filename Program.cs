using System;
using System.Text;
using System.Threading.Tasks;

namespace Aula_TPL
{
    class Program
    {
        public static string WordFirstLetterToEndplusayAndLower(string word)
        {

            var palavramodificada = word.Substring(1, word.Length - 1) + word.Substring(0, 1) + "ay";
            return palavramodificada.ToLower();

        }
        private static string[] Map(string sentence) => sentence.Split();


        private static string[] Process(string[] words)
        {
            for (int i = 0; i < words.Length; i++)
            {
                int index = i;
                Task.Factory.StartNew(() =>
                 {
                     words[index] = WordFirstLetterToEndplusayAndLower(words[index]);
                 }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);

            }
            return words;
        }
        private static string Reduce(string[] words)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var word in words)
            {
                sb.Append(word);
                sb.Append(' ');
            }
            return sb.ToString();
        }
        static void Main(string[] args)
        {
            var sentence = "the quick brown fox jumped over the lazy dog";
            var task = Task<string[]>.Factory.StartNew(() => Map(sentence))
               .ContinueWith<string[]>(t => Process(t.Result))
               .ContinueWith<string>(t => Reduce(t.Result));

            Console.WriteLine($"Result {task.Result}");
        }
    }
}
